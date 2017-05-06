using cuddly_chainsaw.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace cuddly_chainsaw.ViewModels
{
        /*
         * 用户类
         * 分为普通用户及管理员
         * 普通用户可以查看个人信息
         * 普通用户可以修改个人信息（修改昵称，修改密码）
         * 
         * 管理员可以查看个人信息
         * 管理员可以修改个人信息（修改昵称，修改密码）
         * 管理员可以修改所有人的用户名
         * 
         * 二者使用role来区分，role为真时是管理员，role为假时是普通用户。
         */
    class UserViewModel
    {
        //给管理员提供的用户列表
        private ObservableCollection<Models.UserMeta> userItems;
        public ObservableCollection<Models.UserMeta> UserItems { get { return this.userItems; } }

        /*
         * 方便普通用户和管理员修改信息,用户和管理员都指向自身。
         */
        private Models.User selectedUser = default(Models.User);
        public Models.User SelectedUser { get { return selectedUser; } set { this.selectedUser = value; } }

        //init()从服务器端得到user信息。
        public void init()
        {
            ResponseError meg = null;
            List<UserMeta> user = null;
            string str = null;
            Action<ResponseError, List<UserMeta>, string> action
                = delegate (ResponseError re, List<UserMeta> userList, string s1)
                {
                    meg = re;
                    user = userList;
                    str = s1;
                };
            this.selectedUser.getAllUsers(action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            userItems = new ObservableCollection<UserMeta>(user);
        }

        //修改昵称或密码
        public Boolean UpdateforUser(string uid, string nickname, string password)
        {
            ResponseError meg = null;
            Models.UserMeta user = null;
            string str = null;
            Action<ResponseError, Models.UserMeta, string> action
                = delegate (ResponseError re, Models.UserMeta user1, string s1)
                {
                    meg = re;
                    user = user1;
                    str = s1;
                };

            this.selectedUser.updateProfile(nickname, password, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }

            if (user != null)
            {
                if (nickname != null) this.selectedUser.setNickname(nickname);
                if (password != null) this.selectedUser.setPassword(password);
            }
            return (user != null);
        }

        //登录
        public Boolean logIn(string userName, string password)
        {
            ResponseError meg = null;
            Models.User user = null;
            string str = null;
            Action<ResponseError, Models.User, string> action
                = delegate (ResponseError re, Models.User user1, string s1)
                {
                    meg = re;
                    user = user1;
                    str = s1;
                };

            Models.User.login(userName, password, action);

            if(meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }

            if(user != null)
            {
                this.selectedUser = user;
            }
            return (user != null);
        }

        //注册
        public void logOn(string userName, string password, string nickname, string email)
        {
            userItems.Add(new Models.User(userName, password, nickname, email));
        }

        //管理员更新用户名
        public Boolean UpdateUserName(string uid, string userName,string password)
        {
            ResponseError meg = null;
            Models.User user = null;
            string str = null;
            Action<ResponseError, Models.User, string> action
                = delegate (ResponseError re, Models.User user1, string s1)
                {
                    meg = re;
                    user = user1;
                    str = s1;
                };
            this.selectedUser.updateUser(uid, userName, password, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            //本地
            if (user != null)
            {
                this.selectedUser = user;
            }
            return (user != null);
        }

        //管理员删除用户
        public void RemoveUser(string uid)
        {
            ResponseError meg = null;
            Models.User user = null;
            string str = null;
            Action<ResponseError, Models.User, string> action
                = delegate (ResponseError re, Models.User user1, string s1)
                {
                    meg = re;
                    user = user1;
                    str = s1;
                };

            this.selectedUser.deleteOne(uid,action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            //本地，集合删除。
            userItems.Remove(user);
        }
    }
}
