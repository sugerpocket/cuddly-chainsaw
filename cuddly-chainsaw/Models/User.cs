using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cuddly_chainsaw.Models
{
    class UserMeta : INotifyPropertyChanged
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

        public string getId()
        {
            return uid;
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                this.OnPropertyChanged();
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Nickname
        {
            get { return nickname; }
        }

        public void setUsername(string userName)
        {
            username = userName;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    class User : UserMeta
    {
        [JsonProperty]
        private string password = null;
        [JsonProperty]
        private string email = null;

        public User() : base() { }

        public User(string Uid, string Username, string Password, string Nickname, string Email, Boolean Role) : base(Uid, Username, Nickname, Role)
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

        public string getPassword()
        {
            return password;
        }

        public string getEmail()
        {
            return email;
        }

        public Boolean isAdmin()
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

        public static async Task save(User t_user, string identifyingCode, Action<ResponseError, User, string> cb)
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
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                User user = res.getData();
                cb(null, user, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
            return;
        }

        public static async Task login(string username, string password, Action<ResponseError, User, string> cb)
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
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                User user = res.getData();
                cb(null, user, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
            return;
        }

        //user
        public async Task updateProfile(string nickname, string password, Action<ResponseError, UserMeta, string> cb)
        {
            if (!this.role)
            {
                cb(null, null, "没有权限的操作");
                return;
            };

            var body = new Dictionary<string, string> { { "uid", uid } };
            if (nickname != null) body["nickname"] = nickname;
            if (password != null) body["password"] = password;
            string info = JsonConvert.SerializeObject(body, Formatting.Indented);
            string result = "";
            ServerResponse<UserMeta> res = null;
            try
            {
                result = await Server.getInstance().put("api/user/update", info);
            }
            catch (Exception e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<UserMeta>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                UserMeta data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
            return;
        }

        //admin
        public async Task deleteOne(string uid, Action<ResponseError, User, string> cb)
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
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                User data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
            return;
        }

        public async Task getAllUsers(Action<ResponseError, List<UserMeta>, string> cb)
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
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<List<UserMeta>>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                List<UserMeta> list = res.getData();
                cb(null, list, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
            return;
        }

        public async Task updateUser(string uid, string username, string password, Action<ResponseError, User, string> cb)
        {
            if (!this.role)
            {
                cb(null, null, "没有权限的操作");
                return;
            };

            var body = new Dictionary<string, string> { { "uid", uid } };
            if (username != null) body["username"] = username;
            if (password != null) body["password"] = password;
            string info = JsonConvert.SerializeObject(body, Formatting.Indented);
            string result = "";
            ServerResponse<User> res = null;
            try
            {
                result = await Server.getInstance().put("api/admin/update", info);
            }
            catch (COMException e)
            {
                cb(new ResponseError("NETWORK_ERROR", "无法连接服务器"), null, "无法连接服务器");
                return;
            }
            try
            {
                res = JsonConvert.DeserializeObject<ServerResponse<User>>(result);
            }
            catch (Exception e)
            {
                cb(new ResponseError("BAD_DATA", "无法解析服务器返回的信息"), null, "无法解析服务器返回的信息: " + result);
                return;
            }
            if (res.isSuccess())
            {
                User data = res.getData();
                cb(null, data, res.getMessage());
            }
            else cb(res.getError(), null, res.getMessage());
            return;
        }
    }
}
