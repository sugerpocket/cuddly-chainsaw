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
        //在跳转到该页面进行一系列的判断。
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            userViewModel = ((ViewModels.UserViewModel)e.Parameter);
            await userViewModel.init();
            InitializeComponent();
        }

        public async void EditUsername(object sender, RoutedEventArgs e)
        {
            dynamic context = e.OriginalSource;
            userViewModel.SelectedUser = (Models.UserMeta)context.DataContext;
            await userViewModel.UpdateUserName(userViewModel.SelectedUser.getId(), "15333334", null);
        }

        public void ListView_ItemClick(object sender, RoutedEventArgs e)
        {

        }

        public void SpliteView_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
