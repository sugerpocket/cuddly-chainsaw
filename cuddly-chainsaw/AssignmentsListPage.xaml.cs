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
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI;

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
    public sealed partial class AssignmentsListPage : Page
    {
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;

        public AssignmentsListPage()
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
            }
        }

        private void Assignment_Clicked(object sender, ItemClickEventArgs e)
        {
            UserModel.SelectedAssignment = (Assignment)e.ClickedItem;
            Frame root = MainPage.view;
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
