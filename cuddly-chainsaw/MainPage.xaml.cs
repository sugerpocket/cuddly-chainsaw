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
using Newtonsoft.Json.Converters;
using Windows.Storage.Pickers;
using Windows.Storage;
using cuddly_chainsaw.ViewModels;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍
/// <summary>
/// Programmer: 高晨
/// git:Nerotan
/// Conclusion:App的主界面，根据用户是user或admin决定提供的服务
/// Version:1.0
/// </summary>
namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;

        public MainPage()
        {
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
                assignmentPage.Visibility = Visibility.Collapsed;
                userViewPage.Visibility = Visibility.Collapsed;
                if (UserModel.CurrentUser == null)
                {
                    infoPage.Visibility = Visibility.Collapsed;
                }
            }


        }

        /// <summary>
        /// homePage: MainPage, 用户登录后显示的主页
        /// infoPage：查看user或admin的个人信息
        /// （admin）AssignmentPage： 在此用作创建新的Assignmet，还可以用来查看作业详情
        ///  AssignmentModel.SelectedAssignment == null， 创建新的Assignment； AssignmentModel.SelectedAssignment != null, 查看Assignment详情
        /// （admin）userViewPage： 查看现有的所有用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = (StackPanel)e.ClickedItem;
            Frame root = Window.Current.Content as Frame;
            if (temp == mainPage)
            {
                root.Navigate(typeof(MainPage), UserModel);
            }
            else if (temp == infoPage)
            {
                root.Navigate(typeof(InfoPage), UserModel);
            }
            else if (temp == assignmentPage)
            {
                UserModel.SelectedAssignment = null;
                root.Navigate(typeof(AssignmentPage), UserModel);
            }
            else if (temp == userViewPage)
            {
                root.Navigate(typeof(UserViewPage), UserModel);
            }
        }


        private void SpliteView_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = (splitView.IsPaneOpen == true) ? false : true;
        }

        private void Assignment_Clicked(object sender, ItemClickEventArgs e)
        {
            UserModel.SelectedAssignment = (Assignment)e.ClickedItem;
            Frame root = Window.Current.Content as Frame;
            root.Navigate(typeof(AssignmentPage), UserModel);
        }

        private void doingAssignmentsButton_Click(object sender, RoutedEventArgs e)
        {
            DoingBox.Visibility = Visibility.Visible;
            DoneBox.Visibility = Visibility.Collapsed;


        }

        private void doneAssignmentsButton_Click(object sender, RoutedEventArgs e)
        {
            DoingBox.Visibility = Visibility.Collapsed;
            DoneBox.Visibility = Visibility.Visible;
        }
    }
}
