using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuddly_chainsaw.Models
{
    class UserMeta
    {
        [JsonProperty]
        protected string username = null;
        [JsonProperty]
        protected string uid = null;
        [JsonProperty]
        protected string nickname = null;
        [JsonProperty]
        protected Boolean role = false;

        public UserMeta() { }

        public UserMeta(UserMeta userMeta)
        {
            username = userMeta.username;
            uid = userMeta.uid;
            nickname = userMeta.nickname;
            role = userMeta.role;
        }

        public UserMeta(string Uid, string Username, string Nickname, Boolean Role)
        {
            username = Username;
            nickname = Nickname;
            uid = Uid;
            role = Role;
        }
    }

    class User: UserMeta
    {
        [JsonProperty]
        private string password = null;
        [JsonProperty]
        private string email = null;

        public User() : base() { }

        public User(string Uid, string Username, string Password, string Nickname, string Email, Boolean Role): base(Uid, Username, Nickname, Role)
        {
            email = Email;
            password = Password;
        }

        public User(UserMeta userMeta, string Email, string Password) : base(userMeta)
        {
            email = Email;
            password = Password;
        }

        public User(string Username, string Password, string Nickname, string Email) : base(null, Username, Nickname, false)
        {
            password = Password;
            email = Email;
        }

        public User(string Uid, string Username, string Nickname, string Email, Boolean Role) : base(Uid, Username, Nickname, Role)
        {
            email = Email;
        }

        public User(UserMeta userMeta, string Email) : base(userMeta)
        {
            email = Email;
        }
        
        public string getUsername()
        {
            return username;
        }

        public string getPassword()
        {
            return password;
        }

        public string getId()
        {
            return uid;
        }

        public string getNickname()
        {
            return nickname;
        }

        public Boolean getIsTeacher()
        {
            return role;
        }

        public void setNickname(string Nickname)
        {
            nickname = Nickname;
        }

        public void setPassword(string Password)
        {
            password = Password;
        }

        private string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static async void save(User t_user, string identifyingCode, Action<ResponseError, User, string> cb)
        {
            var body = new Dictionary<string, string>
            {
                { "username", t_user.username },
                { "password", t_user.password },
                { "nickname", t_user.nickname },
                { "email", t_user.email },
                { "identifyingCode", identifyingCode}
            };
            string info = JsonConvert.SerializeObject(body, Formatting.Indented);
            string result = "";
            ServerResponse<User> res = null;
            try
            {
                result = await Server.getInstance().post("auth/register", info);
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "发生未知错误，或许是连接不上服务器"), null, "发生未知错误，或许是连接不上服务器");
                return;
            }
            if (res.isSuccess())
            {
                User user = res.getData();
                cb(null, user, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public static async void login(string username, string password, Action<ResponseError, User, string> cb)
        {
            var body = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };
            string info = JsonConvert.SerializeObject(body, Formatting.Indented);
            string result = "";
            ServerResponse<User> res = null;
            try
            {
                result = await Server.getInstance().post("auth/login", info);
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "发生未知错误，或许是连接不上服务器"), null, "发生未知错误，或许是连接不上服务器");
                return;
            }
            if (res.isSuccess())
            {
                User user = res.getData();
                cb(null, user, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        //user
        public async void updateProfile(string nickname, string password, Action<ResponseError, bool?, string> cb)
        {
            if (!this.role)
            {
                cb(null, false, "没有权限的操作");
                return;
            };

            var body = new Dictionary<string, string> { { "uid", uid } };
            if (nickname != null) body["nickname"] = nickname;
            if (password != null) body["password"] = password;
            string info = JsonConvert.SerializeObject(body, Formatting.Indented);
            string result = "";
            ServerResponse<bool?> res = null;
            try
            {
                result = await Server.getInstance().put("api/user/update", info);
                res = JsonConvert.DeserializeObject<ServerResponse<bool?>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "发生未知错误，或许是连接不上服务器"), false, "发生未知错误，或许是连接不上服务器");
                return;
            }
            if (res.isSuccess())
            {
                bool? data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), false, res.getMessage());
        }

        //admin
        public async void deleteOne(string uid, Action<ResponseError, User, string> cb)
        {
            if (!this.role)
            {
                cb(null, null, "没有权限的操作");
                return;
            };
            string result = "";
            ServerResponse<User> res = null;
            try
            {
                result = await Server.getInstance().delete("api/admin/delete/" + uid);
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "发生未知错误，或许是连接不上服务器"), null, "发生未知错误，或许是连接不上服务器");
                return;
            }
            if (res.isSuccess()) {
                User data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public async void getAllUsers(Action<ResponseError, List<UserMeta>, string> cb)
        {
            if (!this.role)
            {
                cb(null, null, "没有权限的操作");
                return;
            };
            
            string result = "";
            ServerResponse<List<UserMeta>> res = null;
            try
            {
                result = await Server.getInstance().get("api/admin/users");
                res = JsonConvert.DeserializeObject<ServerResponse<List<UserMeta>>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "发生未知错误，或许是连接不上服务器"), null, "发生未知错误，或许是连接不上服务器");
                return;
            }
            if (res.isSuccess())
            {
                List<UserMeta> list = res.getData();
                cb(null, list, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
        }

        public async void updateUser(string uid, string username, string password, Action<ResponseError, bool?, string> cb)
        {
            if (!this.role)
            {
                cb(null, false, "没有权限的操作");
                return;
            };

            var body = new Dictionary<string, string>{{ "uid", uid }};
            if (username != null) body["username"] = username;
            if (password != null) body["password"] = password;
            string info = JsonConvert.SerializeObject(body, Formatting.Indented);
            string result = "";
            ServerResponse<bool?> res = null;
            try
            {
                result = await Server.getInstance().put("api/admin/update", info);
                res = JsonConvert.DeserializeObject<ServerResponse<bool?>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "发生未知错误，或许是连接不上服务器"), false, "发生未知错误，或许是连接不上服务器");
                return;
            }
            if (res.isSuccess())
            {
                bool? data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), false, res.getMessage());
        }
    }
}
