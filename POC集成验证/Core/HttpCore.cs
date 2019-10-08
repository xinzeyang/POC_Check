using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace POC集成验证.Core
{
    class HttpCore
    {
        
        private void PaserHeader(string headers,Dictionary<string,string> HeaderDic)
        {
            String[] lines = headers.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //Dictionary<string, string> HeaderDic=null;
            //var HeaderDic = new Dictionary<string, string>();
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
                    HeaderDic.Add(fieldName, fieldValue);
                }
                catch { }
                
            }

        }
        //public string GET(string method, string url, string header, string data)
        //{
        //    string ret = "";
        //    try
        //    {
        //        Http.Get(url).OnSuccess(result =>
        //        {
        //            ret = result;
        //        }).OnFail(webexception =>
        //        {
        //            ret = webexception.Message;
        //        }).Go();

        //    }
        //    catch {; }
        //    return ret;
        //}
        public static async Task<string> Post(string method, string url,Dictionary<string,string> HeaderDic,string Datas)
        {
           
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;
            //req.ContentType = "application/x-www-form-urlencoded";
            //req.UserAgent = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            //格式化header
            foreach (string key in HeaderDic.Keys)
            {
                req.Headers.Add(key, HeaderDic[key]);
            }
            byte[] data = Encoding.UTF8.GetBytes(Datas);//把字符串转换为字节
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
            //增加异步，防止阻塞
            var resp = await req.GetResponseAsync();
            var stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.Default))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

    }
}
