using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;


namespace cuddly_chainsaw.Models
{
    /// <summary>
    /// 服务器单例类
    /// </summary>
    class Server
    {
        /// <summary>
        /// 服务器地址(本地: localhost 远程: www.sugerpocket.cn)
        /// </summary>
        private string location = null;

        /// <summary>
        /// server 实例
        /// </summary>
        private static Server instance = null;

        /// <summary>
        /// http 服务器实例
        /// </summary>
        private HttpClient client = null;

        /// <summary>
        /// Server 单例构造函数
        /// </summary>
        public Server()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
            location = "http://www.sugerpocket.cn:3005/";
        }

        /// <summary>
        /// GET 方法请求发送
        /// </summary>
        /// <param name="url">请求相对路径</param>
        /// <returns>执行成功为异步任务否为null</returns>
        public async Task<string> get(string url)
        {
            HttpResponseMessage res = null;
            res = await client.GetAsync(new Uri(location + url));
            if (res.IsSuccessStatusCode)
            {
                string result = res.Content.ReadAsStringAsync().GetResults();
                return result;
            }

            else return "";
        }

        /// <summary>
        /// POST 方法请求发送
        /// </summary>
        /// <param name="url">请求相对路径</param>
        /// <param name="body">请求主体</param>
        /// <returns>执行成功为异步任务否为null</returns>
        public async Task<string> post(string url, string body)
        {
            HttpResponseMessage res = null;
            HttpStringContent content = new HttpStringContent(body, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            res = await client.PostAsync(new Uri(location + url), content);
            if (res.IsSuccessStatusCode)
            {
                string result = res.Content.ReadAsStringAsync().GetResults();
                return result;
            }

            else return res.StatusCode.ToString();
        }

        /// <summary>
        /// DELETE 方法请求发送
        /// </summary>
        /// <param name="url">请求相对路径</param>
        /// <param name="body">请求主体</param>
        /// <returns>执行成功为异步任务否为null</returns>
        public async Task<string> delete(string url)
        {
            HttpResponseMessage res = null;
            res = await client.DeleteAsync(new Uri(location + url));
            if (res.IsSuccessStatusCode)
            {
                string result = res.Content.ReadAsStringAsync().GetResults();
                return result;
            }

            else return "";
        }

        /// <summary>
        /// PUT 方法请求发送
        /// </summary>
        /// <param name="url">请求相对路径</param>
        /// <param name="body">请求主体</param>
        /// <returns>执行成功为异步任务否为null</returns>
        public async Task<string> put(string url, string body)
        {
            HttpResponseMessage res = null;
            HttpStringContent content = new HttpStringContent(body, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            res = await client.PutAsync(new Uri(location + url), content);
            if (res.IsSuccessStatusCode)
            {
                string result = res.Content.ReadAsStringAsync().GetResults();
                return result;
            }

            else return "";
        }

        /// <summary>
        /// POST 上传文件
        /// </summary>
        /// <param name="url">请求相对路径</param>
        /// <param name="file">发送的file</param>
        /// <returns>执行成功为异步任务否为null</returns>
        public async Task<string> file(string url, StorageFile file)
        {
            HttpResponseMessage res = null;
            HttpMultipartFormDataContent form = new HttpMultipartFormDataContent();
            IInputStream fileStream = await file.OpenSequentialReadAsync();
            HttpStreamContent content = new HttpStreamContent(fileStream);
            form.Add(content, "file", file.Name);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(location + url));
            request.Content = form;

            res = await client.SendRequestAsync(request);

            if (res.IsSuccessStatusCode)
            {
                string result = res.Content.ReadAsStringAsync().GetResults();
                return result;
            }

            else return "";
        }

        /// <summary>
        /// 返回 server 单例的实例
        /// </summary>
        /// <returns>instance</returns>
        public static Server getInstance()
        {
            if (instance == null)
                instance = new Server();

            return instance;
        }
    }
}
