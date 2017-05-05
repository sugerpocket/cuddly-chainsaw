using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace cuddly_chainsaw.Models
{
    class ResponseError
    {
        public ResponseError(string code, string msg)
        {
            error_code = code;
            res_msg = msg;
        }
        string res_msg;
        string error_code;
    }

    class ServerResponse<T>
    {
        [JsonProperty]
        private string status = null;
        [JsonProperty]
        private UserMeta userMeta = null;
        [JsonProperty]
        T data = default(T);
        [JsonProperty]
        private string msg = null;

        static Dictionary<string, string> server_errors = new Dictionary<string, string>();

        public ServerResponse()
        {
            if (!server_errors.ContainsKey("BAD_DATA")) server_errors.Add("BAD_DATA", "数据格式错误或丢失");
            if (!server_errors.ContainsKey("DATABASE_ERROR")) server_errors.Add("DATABASE_ERROR", "数据库错误");
            if (!server_errors.ContainsKey("UNKNOW_ERROR")) server_errors.Add("UNKNOW_ERROR", "未知错误");
            if (!server_errors.ContainsKey("AUTHENTICATION_ERROR")) server_errors.Add("AUTHENTICATION_ERROR", "认证失败，请重新登陆");
        }

        public bool isSuccess()
        {
            if (status == null) return false;
            return status == "OK";
        }

        public string getMessage()
        {
            return msg;
        }

        public UserMeta getRequesterMeta()
        {
            return userMeta;
        }

        public T getData()
        {
            return data;
        }

        public ResponseError getError()
        {
            if (status == "OK") return null;
            if (server_errors.ContainsKey(status) == true)
                return new ResponseError(status, msg == null ? server_errors[status] : msg);
            else return new ResponseError("UNKNOW_ERROR", server_errors["UNKNOW_ERROR"]);
        }
    }
}
