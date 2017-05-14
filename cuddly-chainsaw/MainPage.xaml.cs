using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using System.Collections.ObjectModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 主界面，用于实现侧边栏控制界面之间的跳转
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;

        static Type currentPage;

        ObservableCollection<bool> navSelect = new ObservableCollection<bool>();

        public static Frame view = null;

        public MainPage()
        {
            this.InitializeComponent();
            MainPage.view = this.viewFrame;
            view.Navigate(typeof(AssignmentsListPage), UserModel);
            viewFrame.Navigated += viewChange;
            navSelect.Add(true);
            navSelect.Add(false);
            navSelect.Add(false);
            navSelect.Add(false);
            navSelect.Add(false);
            navSelect.Add(false);
        }

        private void viewChange(object sender, NavigationEventArgs e)
        {
            currentPage = viewFrame.CurrentSourcePageType;
        }

        /// <summary>
        /// 页面初始化的UI逻辑
        /// 对于普通用户，隐藏 新建作业界面 的跳转和 所有用户信息界面 的跳转。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.GetType() == typeof(UserViewModel))
            {
                UserModel = (UserViewModel)e.Parameter;
            }
            else
            {
                UserModel = new UserViewModel();
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
        /// 导航，用于导航栏右边部分页面的跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = (StackPanel)e.ClickedItem;
            if (temp.Parent == AssignmentsListPage)
            {
                view.Navigate(typeof(AssignmentsListPage), UserModel);
            }
            else if (temp.Parent == infoPage)
            {
                view.Navigate(typeof(InfoPage), UserModel);
            }
            else if (temp.Parent == assignmentPage)
            {
                UserModel.SelectedAssignment = null;
                view.Navigate(typeof(AssignmentPage), UserModel);
            }
            else if (temp.Parent == userViewPage)
            {
                view.Navigate(typeof(UserViewPage), UserModel);
            }
        }


        private void SpliteView_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = (splitView.IsPaneOpen == true) ? false : true;
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
