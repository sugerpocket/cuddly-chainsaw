using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using cuddly_chainsaw.Models;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        User loginUser = null;
        string lastUid = null;

        private void login_click(object sender, RoutedEventArgs e)
        {

            Action<ResponseError, User, string> callback = (ResponseError, User, msg) =>
            {
                loginUser = User;
                return;
            };
            Models.User.login("15331060", "dtlzdxyw0126.", callback);
            return;
        }

        private void register_click(object sender, RoutedEventArgs e)
        {
            Models.User user = new Models.User("15331061", "dtlzdxyw0126.", "dw123456", "123@gmail.com");

            Action<ResponseError, User, string> callback = (ResponseError, User, msg) =>
            {
                if (User != null) lastUid = User.getId();
                return;
            };
            Models.User.save(user, "2468", callback);
            return;
        }

        private void getAllUser_click(object sender, RoutedEventArgs e)
        {

            Action<ResponseError, List<UserMeta>, string> callback = (ResponseError, Users, msg) =>
            {
                return;
            };
            if (loginUser != null) loginUser.getAllUsers(callback);
            return;
        }

        private void deleteUser_click(object sender, RoutedEventArgs e)
        {
            Action<ResponseError, User, string> callback = (err, user, msg) =>
            {
                return;
            };
            if (loginUser != null) loginUser.deleteOne(lastUid, callback);
            return;
        }

        private void updateUser_click(object sender, RoutedEventArgs e)
        {
            Action<ResponseError, bool?, string> callback = (err, success, msg) =>
            {
                 return;
            };
            if (loginUser != null) loginUser.updateUser(lastUid, "15331059", "123456", callback);
            return;
        }

        private void updateProfile_click(object sender, RoutedEventArgs e)
        {
            Action<ResponseError, bool?, string> callback = (err, success, msg) =>
            {
                return;
            };
            if (loginUser != null) loginUser.updateProfile(null, "123456", callback);
            return;
        }

        private void newAssignment_click(object sender, RoutedEventArgs e)
        {
            Action<ResponseError, User, string> callback = (err, user, msg) =>
            {
                return;
            };
            Models.User.login("15331060", "dtlzdxyw0126.", callback);
            return;
        }

        private void getAllAssignments_click(object sender, RoutedEventArgs e)
        {
            Action<ResponseError, User, string> callback = (err, User, msg) =>
            {
                return;
            };
            Models.User.login("15331060", "dtlzdxyw0126.", callback);
            return;
        }
    }
}
