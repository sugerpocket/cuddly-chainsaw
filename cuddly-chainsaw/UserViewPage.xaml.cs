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
    /// 所有用户界面
    /// 主要处理逻辑在：查看用户，修改用户名，删除用户。
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

        /// <summary>
        /// 修改用户名时的页面逻辑：使修改界面可见，用户列表界面不可见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 删除用户时的页面逻辑：使删除界面可见，用户列表界面不可见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void ToggleHand(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void TogglePointer(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }
    }
}
