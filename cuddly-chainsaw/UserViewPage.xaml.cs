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
        /// TEST 下面只是一个测试能否修改头像的函数
        /// BUG 出现的主要是管理员修改完，并没有同步更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void select(object sender, RoutedEventArgs e)
        {
            // 设置文件选择器
            Windows.Storage.Pickers.FileOpenPicker open = new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            open.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            // 过滤以包括文件类型的示例子集。
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            // 打开文件选择器
            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            // 如果用户取消则file为null
            if (file != null)
            {
                var temp = file;
                using (Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    await userViewModel.UpdateAvatar(temp);
                }
            }
        }
    }
}
