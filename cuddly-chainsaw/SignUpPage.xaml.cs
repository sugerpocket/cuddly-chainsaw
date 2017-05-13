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
using Windows.UI.Xaml.Media.Animation;
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
        PaneThemeTransition PaneAnim = new PaneThemeTransition { Edge = EdgeTransitionLocation.Right };
        public SignUpPage()
        {
            //this.InitializeComponent();
            ManipulationCompleted += AppleAnimationPage_ManipulationCompleted;
            Transitions = new TransitionCollection();
            Transitions.Add(PaneAnim);
            ManipulationMode = ManipulationModes.TranslateX;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PaneAnim.Edge = e.NavigationMode == NavigationMode.Back ? EdgeTransitionLocation.Left : EdgeTransitionLocation.Right;
            base.OnNavigatedTo(e);
            this.InitializeComponent();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            PaneAnim.Edge = e.NavigationMode != NavigationMode.Back ? EdgeTransitionLocation.Left : EdgeTransitionLocation.Right;
        }

        private void AppleAnimationPage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var trans = e.Cumulative.Translation;
            double DeltaX = Math.Abs(trans.X);
            if (Math.Abs(trans.Y) * 3 < DeltaX && DeltaX > ActualWidth / 2)
            {
                if (trans.X > 0)
                {
                    if (Frame.CanGoBack)
                        Frame.GoBack();
                }
                else
                {
                    if (Frame.CanGoForward)
                        Frame.GoForward();
                }
            }
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
