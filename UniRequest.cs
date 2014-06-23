using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Net;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;

namespace Xupt_lib
{
    public class UniRequest
    {
        public delegate void HandleResult(string result);
        private HandleResult handle;

        HttpWebRequest request;

        HttpWebResponse response;

        Dictionary<string, object> Params = new Dictionary<string, object>();

        byte[] POSTBin;

        string ResponseContent;

        /// <summary>
        /// 构造函数，获取URL、方法
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="method">请求方法POST或GET</param>
        public UniRequest(string url, string method = "GET")
        {
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Request Created Failed!" + e.Message);
                return;
            }
            request.UserAgent = "XiyouLibrary_Windows_Phone_Client";
            switch (method)
            {
                case "POST":
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    break;
                case "GET": request.Method = "GET"; break;
                default: request.Method = "GET"; break;
            }
        }

        /// <summary>
        /// 添加请求参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AddParams(string key, object value)
        {
            if (request.Method == "GET")
            {
                Debug.WriteLine("Method Error!");
                return;
            }
            Params.Add(key, value);
        }

        /// <summary>
        /// 开始请求
        /// </summary>
        /// <param name="handle">请求结果</param>
        public void StartRequest(HandleResult handle)
        {
            this.handle = handle;
            switch (request.Method)
            {
                default:
                case "GET": request.BeginGetResponse(new AsyncCallback(GetResponse), request); break;
                case "POST": request.BeginGetRequestStream(new AsyncCallback(UploadParam), request); break;
            }
        }

        void UploadParam(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            using (Stream stream = request.EndGetRequestStream(result))
            {
                string tmp = "";
                for (int i = 0; i < Params.Count; i++)
                {
                    tmp += string.Format("{0}={1}", Params.ElementAtOrDefault(i).Key, Params.ElementAtOrDefault(i).Value);
                    if (i + 1 != Params.Count)
                    {
                        tmp += "&";
                    }
                }
                POSTBin = Encoding.UTF8.GetBytes(tmp);
                stream.Write(POSTBin, 0, POSTBin.Length);
            }
            request.BeginGetResponse(new AsyncCallback(GetResponse), request);
        }

        void GetResponse(IAsyncResult result)
        {
            try
            {
                request = (HttpWebRequest)result.AsyncState;
                response = (HttpWebResponse)request.EndGetResponse(result);
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    ResponseContent = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                ResponseContent = "Request Error!";
                Debug.WriteLine(e.InnerException.ToString());
            }
            Deployment.Current.Dispatcher.BeginInvoke(() => { handle(ResponseContent); });
        }
    }
}
