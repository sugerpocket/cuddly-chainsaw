using cuddly_chainsaw.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using cuddly_chainsaw.Converters;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;
        StorageFile file;
        public InfoPage()
        {
            //this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.GetType() == typeof(UserViewModel))
            {
                UserModel = (UserViewModel)e.Parameter;
            }
            else
            {
                UserModel = new UserViewModel();
                //测试用，之后应该删除
                //await UserModel.logIn("15331060", "123456");
                //await UserModel.init();
            }

            AssignmentModel = new AssignmentViewModel();
            this.InitializeComponent();
            if (UserModel.CurrentUser == null || !UserModel.CurrentUser.isAdmin())
            {
            }
            //显示用户个人信息
            if (UserModel.CurrentUser != null)
            {
                username.Text = UserModel.CurrentUser.Username;
                mail.Text = UserModel.CurrentUser.getEmail();
                nickname.Text = UserModel.CurrentUser.Nickname;
                string uid = this.UserModel.CurrentUser.getId();
                string imageUri = "http://www.sugerpocket.cn:3005/api/user/avatar?uid=" + uid;
                ava.ImageSource = new BitmapImage(new Uri(imageUri));
            }
        }

        //点击确认修改个人信息
        private async void sure_Click(object sender, RoutedEventArgs e)
        {
            string nick = nickname.Text;
            string psw = password.Password;
            string uid = this.UserModel.CurrentUser.getId();
            await this.UserModel.UpdateforUser(uid, nick, psw);
            if (file != null)
            {
                await UserModel.UpdateAvatar(file);
            }
        }

        //更新显示的图片
        private async void Ellipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker openPicker =
                new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            openPicker.ViewMode =
                Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");

            file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                    await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    ava.ImageSource = bitmapImage;
                }
            }
        }
    }
}
