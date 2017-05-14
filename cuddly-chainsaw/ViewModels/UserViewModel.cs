using cuddly_chainsaw.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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
         * 方便管理员修改信息,指向被点击的用户
         */
        private Models.UserMeta selectedUser = default(Models.UserMeta);
        public Models.UserMeta SelectedUser { get { return selectedUser; } set { this.selectedUser = value; } }

        /// <summary>
        /// 指向被选中的作业
        /// </summary>
        private Models.Assignment selectedAssignment = default(Models.Assignment);
        public Models.Assignment SelectedAssignment { get { return selectedAssignment; } set { this.selectedAssignment = value; } }

        /// <summary>
        /// 指向自身
        /// </summary>
        private Models.User currentUser = default(Models.User);
        public Models.User CurrentUser { get { return currentUser; } set { this.currentUser = value; } }

        //init()从服务器端得到user信息。
        public async Task init()
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
            await this.currentUser.getAllUsers(action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            userItems = new ObservableCollection<UserMeta>(user);
        }

        //修改昵称或密码
        public async Task<Boolean> UpdateforUser(string uid, string nickname, string password)
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

            await this.currentUser.updateProfile(nickname, password, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }

            if (user != null)
            {
                if (nickname != null) this.currentUser.setNickname(nickname);
                if (password != null) this.currentUser.setPassword(password);
            }

            return (user != null);
        }

        //登录
        public async Task<Boolean> logIn(string userName, string password)
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

            await User.login(userName, password, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }

            if (user != null)
            {
                this.currentUser = user;
            }
            return (user != null);
        }

        //注册
        public async Task logOn(string userName, string password, string nickname, string email, string identifyingCode)
        {
            Models.User newUser = new User(userName, password, nickname, email);

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
            await Models.User.save(newUser, identifyingCode, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            if (user != null)
            {
                await logIn(userName, password);
            }
        }

        //管理员更新用户名
        public async Task<Boolean> UpdateUserName(string uid, string userName, string password)
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
            await this.currentUser.updateUser(uid, userName, password, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            //本地
            if (user != null)
            {
                if (this.currentUser.isAdmin() == true)
                {
                    UserItems[FindUser(uid)] = (Models.UserMeta)user;
                }
            }

            return (user != null);
        }

        //管理员删除用户
        public async Task RemoveUser(string uid)
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

            await this.currentUser.deleteOne(uid, action);

            if (meg != null)
            {
                var i = new MessageDialog(str).ShowAsync();
            }
            //本地，集合删除。
            userItems.RemoveAt(FindUser(uid));
        }

        /// <summary>
        /// 修改自身的头像
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sf"></param>
        /// <returns></returns>
        public async Task<Boolean> UpdateAvatar(StorageFile sf)
        {
            ResponseError meg = null;
            string str = null;
            Action<ResponseError, string> action
                = delegate (ResponseError re, string s1)
                {
                    meg = re;
                    str = s1;
                };
            await this.currentUser.updateAvatar(sf, action);
            var i = new MessageDialog(str).ShowAsync();
            return (meg == null);
        }

        /// <summary>
        /// 根据id寻找对应的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int FindUser(string id)
        {
            int length = UserItems.Count;
            int i = 0;
            for (; i < length; i++)
            {
                if (UserItems[i].getId() == id)
                {
                    break;
                }
            }
            return i;
        }
    }
}
