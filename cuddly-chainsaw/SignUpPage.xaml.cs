using cuddly_chainsaw.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SignUpPage : Page
    {
        //AssignmentViewModel AssignmentModel;
        //UserViewModel UserModel;
        public SignUpPage()
        {
            //this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.InitializeComponent();
        }

        private async void signUp_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel UserModel;
            UserModel = new UserViewModel();
            string user = username.Text;
            string psw = password.Password;
            string email = mail.Text;
            string nick = nickname.Text;
            await UserModel.logOn(user, psw, nick, email, "2468");
            Boolean isLogin = await UserModel.logIn(user, psw);
            if (isLogin)
            {
                this.Frame.Navigate(typeof(MainPage), UserModel);
            }
        }

        private void signIn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
