using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class UserViewPage : Page
    {
        public UserViewPage()
        {

        }

        ViewModels.UserViewModel userViewModel { get; set; }
        ViewModels.AssignmentViewModel AssignmentModel { get; set; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            userViewModel = ((ViewModels.UserViewModel)e.Parameter);
            AssignmentModel = new ViewModels.AssignmentViewModel();
            await userViewModel.init();
            InitializeComponent();
        }

        public void EditUser(object sender, RoutedEventArgs e)
        {
            dynamic context = e.OriginalSource;
            userViewModel.SelectedUser = (Models.UserMeta)context.DataContext;
            UserGridView.Visibility = Visibility.Collapsed;
            OriginalID_1.Text = userViewModel.SelectedUser.Username.ToString();
            ID.Text = "";
            EditUserInfo.Visibility = Visibility.Visible;
        }

        public async void EditUsername(object sender, RoutedEventArgs e)
        {
            string username = ID.Text;
            await userViewModel.UpdateUserName(userViewModel.SelectedUser.getId(), username, null);
            UserGridView.Visibility = Visibility.Visible;
            EditUserInfo.Visibility = Visibility.Collapsed;
        }

        public void DeleteUser(object sender, RoutedEventArgs e)
        {
            dynamic context = e.OriginalSource;
            userViewModel.SelectedUser = (Models.UserMeta)context.DataContext;
            OriginalID_2.Text = userViewModel.SelectedUser.Username.ToString();
            UserGridView.Visibility = Visibility.Collapsed;
            DeleteUserinfo.Visibility = Visibility.Visible;
        }

        public async void RemoveUser(object sender, RoutedEventArgs e)
        {
            await userViewModel.RemoveUser(userViewModel.SelectedUser.getId());
            UserGridView.Visibility = Visibility.Visible;
            DeleteUserinfo.Visibility = Visibility.Collapsed;
        }

        public void CancelEdit(object sender, RoutedEventArgs e)
        {
            userViewModel.SelectedUser = null;
            UserGridView.Visibility = Visibility.Visible;
            EditUserInfo.Visibility = Visibility.Collapsed;
        }

        public void CancelDelete(object sender, RoutedEventArgs e)
        {
            userViewModel.SelectedUser = null;
            UserGridView.Visibility = Visibility.Visible;
            DeleteUserinfo.Visibility = Visibility.Collapsed;
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
                root.Navigate(typeof(MainPage), userViewModel);
            }
            else if (temp == infoPage)
            {
                root.Navigate(typeof(InfoPage), userViewModel);
            }
            else if (temp == assignmentPage)
            {
                AssignmentModel.SelectedAssignment = null;
                root.Navigate(typeof(AssignmentPage), userViewModel);
            }
            else if (temp == userViewPage)
            {
                root.Navigate(typeof(UserViewPage), userViewModel);
            }
        }

        private void SpliteView_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = (splitView.IsPaneOpen == true) ? false : true;
            //userAvatar.Visibility = (userAvatar.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
