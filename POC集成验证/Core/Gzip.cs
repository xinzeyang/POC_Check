using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC集成验证.Core
{
    class Gzip
    {
        public static class Decode
        {




            public static byte[] getFileByte(String path)
            {
                FileStream fs = new FileStream(path, FileMode.Open);

                //获取文件大小
                long size = fs.Length;

                byte[] array = new byte[size];

                //将文件读到byte数组中
                fs.Read(array, 0, array.Length);

                fs.Close();
                return array;
            }
            public static void writeFileByte(String path, byte[] array)
            {
                //创建一个文件流
                FileStream fs = new FileStream(path, FileMode.Create);

                //将byte数组写入文件中
                fs.Write(array, 0, array.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fs.Close();
            }
            public static byte[] Compress(byte[] rawData)
            {
                MemoryStream ms = new MemoryStream();
                GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
                compressedzipStream.Write(rawData, 0, rawData.Length);
                compressedzipStream.Close();
                return ms.ToArray();
            }
            public static byte[] Decompress(byte[] inputBytes)
            {

                using (MemoryStream inputStream = new MemoryStream(inputBytes))
                {
                    using (MemoryStream outStream = new MemoryStream())
                    {
                        using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                        {
                            zipStream.CopyTo(outStream);
                            zipStream.Close();
                            return outStream.ToArray();
                        }
                    }

                }
            }
            public static String DecompressToString(byte[] inputBytes, Encoding encoding)
            {

                using (MemoryStream ms = new MemoryStream(inputBytes))
                {
                    using (GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress))
                    using (StreamReader sr = new StreamReader(zipStream, encoding))
                        return sr.ReadToEnd();
                }
            }
        }
    }
}
