﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace POC集成验证.Core
{
    class HttpApi
    {
        public static int timeOut = 15000;

        public static HttpResult Get(String url)
        {
            return BaseConn(url, "GET", null, "UTF-8", null);
        }

        public static HttpResult Get(String url, String encode)
        {
            return BaseConn(url, "GET", null, encode, null);
        }

        public static HttpResult Get(String url, String encode, Dictionary<String, String> headers)
        {
            return BaseConn(url, "GET", null, encode, headers);
        }

        public static HttpResult Post(String url, String data)
        {
            return BaseConn(url, "POST", data, "UTF-8", null);
        }

        public static HttpResult Post(String url, String data, String encode)
        {
            return BaseConn(url, "POST", data, encode, null);
        }


        public static HttpResult Post(String url, String data, String encode, Dictionary<String, String> headers)
        {
            return BaseConn(url, "POST", data, encode, headers);
        }


        public static List<String> deathHost = new List<string>();
        public static HttpResult BaseConn(String url, String method, String data, String encode, Dictionary<String, String> headers)
        {
            Uri uri = new Uri(url);
            String host = uri.Host;
            if (deathHost.Contains(host))
            {
                return null;
            }
            int port = ((uri.Port == -1) ? (url.ToLower().StartsWith("https") ? 443 : 80) : uri.Port);
            //构建数据
            StringBuilder httpData = new StringBuilder();
            if (method.ToUpper().Equals("GET") && !String.IsNullOrEmpty(data))
            {
                url = url + "?" + data;
            }
            httpData.Append(method.ToUpper()).Append(" ").Append(uri.PathAndQuery)
                    .Append(" HTTP/1.1\r\n");
            httpData.Append("Host: " + host + "\r\n");
            if (headers == null)
            {
                httpData.Append("Accept").Append(": ").Append("*/*").Append("\r\n");
                httpData.Append("User-Agent").Append(": ").Append("Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36").Append("\r\n");
            }
            else
            {
                foreach (string key in headers.Keys)
                {
                    httpData.Append(key).Append(": ").Append(headers[key]).Append("\r\n");
                }
            }
            if (method.ToUpper().Equals("POST") || method.ToUpper().Equals("PUT"))
            {
                httpData.Append("Content-Length: ").Append(Encoding.GetEncoding(encode).GetBytes(data).Length).Append("\r\n");
            }
            httpData.Append("\r\n");
            if (method.ToUpper().Equals("POST") || method.ToUpper().Equals("PUT"))
            {
                httpData.Append(data);
            }
            byte[] bytes = Encoding.GetEncoding(encode).GetBytes(httpData.ToString());
            if (uri.Scheme.ToLower().Equals("https"))
            {
                return httpsSendData(host, port, timeOut, encode, bytes);
            }
            return httpSendData(host, port, timeOut, encode, bytes);
        }





        static uint dummy = 0;
        static byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];

        public static HttpResult httpSendData(String host, int port, int connectTimeout, String encode, byte[] data)
        {
            TcpClient socket = null;
            try
            {
                socket = new TcpClient(host, port);
                socket.ReceiveTimeout = connectTimeout;
                socket.SendTimeout = connectTimeout;
                if (!socket.Connected)
                {
                    deathHost.Add(host);
                    return null;
                }
                socket.Client.Send(data, SocketFlags.None);
                socket.GetStream().Flush();
                HttpResult result = HttpResultParse.loadSockerStream(socket.GetStream(), encode, host);
                return result;
            }
            catch (SocketException)
            {
                deathHost.Add(host);
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                try
                {
                    if (socket != null && socket.Connected) { socket.Close(); }

                }
                catch { }

            }
        }

        public static HttpResult httpsSendData(String host, int port, int connectTimeout, String encode, byte[] data)
        {
            TcpClient socket = null;
            try
            {
                socket = new TcpClient(host, port);
                socket.ReceiveTimeout = connectTimeout;
                socket.SendTimeout = connectTimeout;
                if (!socket.Connected)
                {
                    deathHost.Add(host);
                    return null;
                }
                SslStream sslStream = new SslStream(socket.GetStream(), true
                    , new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors)
                        =>
                    {
                        return sslPolicyErrors == SslPolicyErrors.None;
                    }
                        ), null);
                sslStream.ReadTimeout = connectTimeout;
                sslStream.WriteTimeout = connectTimeout;
                X509Store store = new X509Store(StoreName.My);
                sslStream.AuthenticateAsClient(host, store.Certificates, System.Security.Authentication.SslProtocols.Tls, false);
                sslStream.Write(data, 0, data.Length);
                sslStream.Flush();
                HttpResult result = HttpResultParse.loadSockerStream(sslStream, encode, host);
                return result;
            }
            catch (SocketException)
            {
                deathHost.Add(host);
                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
            finally
            {
                try
                {
                    if (socket != null && socket.Connected) { socket.Close(); }
                }
                catch { }
            }
        }


        public class HttpResult
        {

            private void parseHeader()
            {
                //解析状态码
                String[] lines = header.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                Regex regex = new Regex("[0-9]{3}");
                Match match = regex.Match(lines[0]);
                this.code = Convert.ToInt32(match.Value);
                //解析Header
                headDics = new Dictionary<string, string>();
                for (int index = 1; index < lines.Length; index++)
                {
                    if (!lines[index].Contains(":"))
                    {
                        continue;
                    }
                    try
                    {
                        Int32 splitModder = lines[index].IndexOf(":");
                        String fieldName = lines[index].Substring(0, splitModder);
                        String fieldValue = lines[index].Substring(splitModder + 1, lines[index].Length - splitModder - 1).Trim();
                        headDics.Add(fieldName, fieldValue);
                    }
                    catch { }
                }
                //判断是否分段加载
                if (headDics.ContainsKey("Transfer-Encoding") && headDics["Transfer-Encoding"].EndsWith("chunked", StringComparison.OrdinalIgnoreCase))
                {
                    this.isChunk = true;
                }
                //判断是否压缩
                if (headDics.ContainsKey("Content-Encoding") && headDics["Content-Encoding"].Contains("gzip"))
                {
                    this.isGzip = true;
                }
                //判断响应长度
                if (headDics.ContainsKey("Content-Length"))
                {
                    try
                    {
                        resultLength = Convert.ToInt32(headDics["Content-Length"].Trim());
                    }
                    catch { }
                }
                //判断编码格式
                if (headDics.ContainsKey("Content-Type"))
                {
                    String charstModder = "charset=";
                    if (headDics["Content-Type"].Contains(charstModder))
                    {
                        try
                        {
                            Int32 splitModder = headDics["Content-Type"].IndexOf(charstModder);
                            String encode = headDics["Content-Type"].Substring(splitModder + charstModder.Length, headDics["Content-Type"].Length - splitModder - charstModder.Length);
                            this.encode = encode;
                        }
                        catch { }
                    }

                }

            }

            private String encode;

            public String Encode
            {
                get { return encode; }
                set { encode = value; }
            }

            private Dictionary<String, String> headDics;

            public Dictionary<String, String> HeadDics
            {
                get { return headDics; }
                set { headDics = value; }
            }

            private String header;

            public String Header
            {
                get { return header; }
                set
                {

                    header = value;
                    parseHeader();
                }
            }

            private Int32 resultLength = 0;

            public Int32 ResultLength
            {
                get { return resultLength; }
                set { resultLength = value; }
            }

            private String cookie;

            public String Cookie
            {
                get
                {
                    if (String.IsNullOrEmpty(cookie))
                    {
                        if (!headDics.ContainsKey("Set-Cookie"))
                        {
                            return null;
                        }
                        this.cookie = headDics["Set-Cookie"];
                        return cookie;
                    }

                    return cookie;
                }
                set { cookie = value; }
            }

            private bool isChunk = false;

            public bool IsChunk
            {
                get { return isChunk; }
                set { isChunk = value; }
            }

            private bool isGzip = false;

            public bool IsGzip
            {
                get { return isGzip; }
                set { isGzip = value; }
            }

            private Int32 code = -1;

            public Int32 Code
            {
                get { return code; }
                set { code = value; }
            }


            public void addChunk(String line)
            {
                chunks.Add(line);
            }

            private List<String> chunks = new List<String>();

            public List<String> Chunks
            {
                get { return chunks; }
                set { chunks = value; }
            }
            private String body;

            public String Body
            {
                get { return body; }
                set { body = value; }
            }

        }
        public class HttpResultParse
        {
            public static HttpResult loadSockerStream(Stream stream, String encode, String host)
            {
                HttpResult httpResult = null;
                try
                {
                    StringBuilder result = new StringBuilder();
                    Encoding encoder = Encoding.GetEncoding(encode);
                    //接受Header
                    String line = readLine(stream, encoder);
                    if (String.IsNullOrEmpty(line))
                    {
                        return null;
                    }
                    while (!String.IsNullOrEmpty(line) && !line.Equals("\r\n"))
                    {
                        result.Append(line);
                        line = readLine(stream, encoder);

                    }
                    httpResult = new HttpResult();
                    httpResult.Header = result.ToString();
                    byte[] bteReceive = null;
                    if (!httpResult.IsChunk)
                    {
                        bteReceive = readBytesBatch(stream, httpResult.ResultLength);
                        if (httpResult.IsGzip)
                        {
                            try
                            {
                                bteReceive = Gzip.Decode.Decompress(bteReceive);
                            }
                            catch
                            {
                            }
                        }
                        httpResult.Body = encoder.GetString(bteReceive, 0, bteReceive.Length);
                        return httpResult;
                    }
                    Int32 chunkLength = getChunkLength(stream, encoder);
                    MemoryStream memoryStream = new MemoryStream();
                    while (chunkLength > 0)
                    {
                        try
                        {
                            bteReceive = readBytesBatch(stream, chunkLength);
                            if (bteReceive == null)
                            {
                                break;
                            }
                            memoryStream.Write(bteReceive, 0, chunkLength);
                        }
                        catch
                        {
                            chunkLength = 0;
                        }
                        finally
                        {
                            if (chunkLength > 0)
                            {
                                chunkLength = getChunkLength(stream, encoder);
                            }
                        }
                    }
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    bteReceive = new byte[memoryStream.Length];
                    memoryStream.Read(bteReceive, 0, bteReceive.Length);
                    memoryStream.Close();
                    if (httpResult.IsGzip)
                    {
                        try
                        {
                            bteReceive = Gzip.Decode.Decompress(bteReceive);
                            //bteReceivex=
                        }
                        catch { }
                    }
                    httpResult.Body = encoder.GetString(bteReceive, 0, bteReceive.Length);
                    return httpResult;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    try
                    {
                        stream.Close();
                    }
                    catch { }
                }
            }

            private static Int32 getChunkLength(Stream stream, Encoding encoder)
            {
                while (true)
                {
                    String line = readLine(stream, encoder);
                    if (line == null)
                    {
                        return -1;
                    }
                    line = line.Trim();
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    try
                    {
                        return Convert.ToInt32(line, 16);
                    }
                    catch
                    {
                        return -1;
                    }
                }

            }

            public static byte[] readBytesBatch(Stream stream, Int32 readSize)
            {

                byte[] bytes = new byte[readSize];
                int readLength = 0;
                int remainLength = readSize;
                while (readLength < bytes.Length)
                {
                    int chr = stream.Read(bytes, readLength, remainLength);
                    readLength += chr;
                    if (chr < 1)
                    {
                        break;
                    }
                    remainLength = readSize - readLength;
                }
                if (readLength <= 0)
                {
                    return null;
                }
                return bytes;
            }
            public static String readLine(Stream stream, Encoding encode)
            {

                List<byte> bytes = new List<byte>();
                try
                {
                    int bye = 0;
                    do
                    {
                        bye = stream.ReadByte();
                        if (bye != -1)
                        {
                            bytes.Add((byte)bye);
                        }
                    } while (bye != 10 && bye != 0 && bye != -1);
                    if (bytes.Count == 0)
                    {
                        return null;
                    }
                    return encode.GetString(bytes.ToArray());
                }
                catch
                {
                    return null;
                }
                finally
                {
                    bytes = null;
                }
            }
        }
    }
}
