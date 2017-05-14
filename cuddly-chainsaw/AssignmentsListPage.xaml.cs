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
using Windows.UI.Xaml.Media.Animation;

namespace cuddly_chainsaw
{
    /// <summary>
    /// Conclusion: 查看所有作业，根据用户是user或admin决定提供的服务
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
            }

            AssignmentModel = new AssignmentViewModel();
            this.InitializeComponent();
            if (UserModel.CurrentUser == null || !UserModel.CurrentUser.isAdmin())
            {
            }
        }

        private void DetailPointerEnter(object sender, PointerRoutedEventArgs e)
        {
            var target = (Grid)sender;
            target.Opacity = 1;
            ToggleHand(sender, e);
        }

        private void DetailPointOut(object sender, PointerRoutedEventArgs e)
        {
            var target = (Grid)sender;
            target.Opacity = 0;
            TogglePointer(sender, e);
        }



        private void Assignment_Clicked(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            UserModel.SelectedAssignment = (Assignment)item;
            Frame root = MainPage.view;
            root.Navigate(typeof(AssignmentPage), UserModel);
        }

        /// <summary>
        /// 用于切换 已完成作业和 未完成作业之间的视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
