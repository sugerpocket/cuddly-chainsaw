using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Windows.Storage;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace cuddly_chainsaw.Models
{
    class File_entry
    {
        public File_entry()
        {
            allowable = false;
            maxSize = 0;
            nameRegExp = null;
        }

        public File_entry(bool allow, uint size, string nameRegExp)
        {
            allowable = allow;
            maxSize = size;
            this.nameRegExp = nameRegExp;
        }

        [JsonProperty]
        bool? allowable;
        [JsonProperty]
        uint? maxSize;
        [JsonProperty]
        string nameRegExp;
    }

    class Assignment
    {
        [JsonProperty]
        private string aid;
        [JsonProperty]
        private string title;
        [JsonProperty]
        private string content;
        [JsonProperty]
        private DateTime? ddl;
        [JsonProperty]
        private DateTime? start;
        [JsonProperty]
        private uint? week;
        [JsonProperty]
        private uint? type;
        [JsonProperty]
        private UserMeta promulgatorMeta;
        [JsonProperty]
        File_entry fileEntry;


        public Assignment()
        {
            type = 0;
            week = 0;
            start = null;
            ddl = null;
            content = "";
            title = "";
            aid = null;
            fileEntry = new File_entry();
        }

        public Assignment(string title, string content, uint type, uint week, DateTime deadline)
        {
            this.title = title;
            this.content = content;
            this.type = type;
            this.week = week;
            ddl = deadline;
            start = null;
            aid = null;
            fileEntry = new File_entry();
        }

        public Assignment(string title, string content, uint type, uint week, DateTime deadline, File_entry fileEntry)
        {
            this.title = title;
            this.content = content;
            this.type = type;
            this.week = week;
            ddl = deadline;
            start = null;
            aid = null;
            this.fileEntry = fileEntry;
        }

        private DateTime toUTC(DateTime dt)
        {
            return dt.AddHours(-8);
        }

        private DateTime toGMT(DateTime dt)
        {
            return dt.AddHours(8);
        }

        public string getTitle()
        {
            return title;
        }
        public string getContent()
        {
            return content;
        }

        public string getAssignmentId()
        {
            return aid;
        }

        public void setTitle(string newTitle)
        {
            title = newTitle;
        }

        public void setContent(string newContent)
        {
            content = newContent;
        }

        public static async void getAll(Action<ResponseError, List<Assignment>, string> cb)
        {
            string result = "";
            ServerResponse<List<Assignment>> res = null;
            try
            {
                result = await Server.getInstance().get("api/assignment/list");
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<List<Assignment>>>(result, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                List<Assignment> data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public static async void getOne(string aid, Action<ResponseError, Assignment, string> cb)
        {
            string result = "";
            ServerResponse<Assignment> res = null;
            try
            {
                result = await Server.getInstance().get("api/assignment/one/" + aid);
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<Assignment>>(result, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                Assignment data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public static async void delete(string aid, Action<ResponseError, Assignment, string> cb)
        {
            string result = null;
            ServerResponse<Assignment> res = null;
            if (aid == null)
            {
                cb(new ResponseError("BAD_DATA", "服务器上不存在此任务"), null, "服务器上不存在此任务");
                return;
            }
            try
            {
                result = await Server.getInstance().delete("api/assignment/one/" + aid + "/delete");
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<Assignment>>(result, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                Assignment data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public async void save(Action<ResponseError, Assignment, string> cb)
        {
            string body = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
            string result = "";
            ServerResponse<Assignment> res = null;
            try
            {
                result = await Server.getInstance().post("api/assignment/new", body);
                res = JsonConvert.DeserializeObject<ServerResponse<Assignment>>(result, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<Assignment>>(result, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                Assignment data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        } 

        public async void update(Action<ResponseError, Assignment, string> cb)
        {
            string body = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
            string result = "";
            ServerResponse<Assignment> res = null;
            if (aid == null)
            {
                cb(new ResponseError("BAD_DATA", "服务器上不存在此任务"), null, "服务器上不存在此任务");
                return;
            }
            try
            {
                result = await Server.getInstance().put("api/assignment/one/" + aid + "/update", body);
                
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<Assignment>>(result, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                Assignment data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public async void submit(StorageFile file, Action<ResponseError, string, string> cb)
        {
            string result = "";
            ServerResponse<string> res = null;
            if (aid == null)
            {
                cb(new ResponseError("BAD_DATA", "服务器上不存在此任务"), null, "服务器上不存在此任务");
                return;
            }
            try
            {
                result = await Server.getInstance().file("api/assignment/one/" + aid + "/file", file);
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<string>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                string data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }
    }
}
