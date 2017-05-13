using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using cuddly_chainsaw.ViewModels;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using cuddly_chainsaw.Models;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AssignmentPage : Page
    {
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;
        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

        public AssignmentPage()
        {
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.GetType() == typeof(UserViewModel))
            {
                UserModel = (UserViewModel)e.Parameter;
            }
            else
            {
                UserModel = new UserViewModel();
                await UserModel.init();
            }
            AssignmentModel = new AssignmentViewModel();
            this.InitializeComponent();
            initializeView(UserModel.SelectedAssignment);
            DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= DataTransferManager_DataRequested;
            UserModel.SelectedAssignment = null;
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

        /// <summary>
        /// 当目前登陆的用户是admin时，才能修改Assignment的信息
        /// 否则用户是普通user，禁用修改信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initializeView(Assignment asg)
        {
            if (asg != null)
            {
                textBlock.Text = asg.Title;
                titleTextBox.Text = asg.Title;
                detailsTextBox.Text = asg.getContent();
                //startBox.Date = new DateTime(asg.Start.Ticks);
                ddlBox.Date = new DateTime(asg.DDL.Ticks);
                createButton.Icon = new SymbolIcon(Symbol.Upload);
                string temp;
                if (asg.Type != 0)
                {
                    backgroundImage.Source = new BitmapImage(new Uri("ms-appx:Assets/" + asg.Type + ".jpg"));
                }
                else if (asg.isEnded())
                {
                    backgroundImage.Source = new BitmapImage(new Uri("ms-appx:Assets/done.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    backgroundImage.Source = new BitmapImage(new Uri("ms-appx:Assets/doing.png", UriKind.RelativeOrAbsolute));
                }
            }
            if (UserModel.CurrentUser == null || !UserModel.CurrentUser.isAdmin())
            {
                assignmentPage.Visibility = Visibility.Collapsed;
                userViewPage.Visibility = Visibility.Collapsed;
                titleTextBox.IsReadOnly = true;
                detailsTextBox.IsReadOnly = true;
                ddlBox.IsEnabled = false;
                createButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                selectFileButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            AssignmentModel.SelectedAssignment = UserModel.SelectedAssignment;
            if (AssignmentModel.SelectedAssignment != null)
            {
                updateButton_Click(sender, e);
                return;
            }
            Frame root = Window.Current.Content as Frame;
            Assignment newAsg = new Assignment(titleTextBox.Text, detailsTextBox.Text, (uint)AsgType.SelectedIndex, 0, new DateTime(ddlBox.Date.Ticks));
            await AssignmentModel.newAssignments(newAsg);
            UserModel.SelectedAssignment = AssignmentModel.SelectedAssignment;
            root.Navigate(typeof(MainPage), UserModel);
        }

        private async void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            AssignmentModel.SelectedAssignment.setTitle(titleTextBox.Text);
            AssignmentModel.SelectedAssignment.setContent(detailsTextBox.Text);
            AssignmentModel.SelectedAssignment.DDL = new DateTime(ddlBox.Date.Ticks);
            AssignmentModel.SelectedAssignment.Type = (uint)AsgType.SelectedIndex;
            await AssignmentModel.updateAssignments();
            UserModel.SelectedAssignment = AssignmentModel.SelectedAssignment;
            root.Navigate(typeof(MainPage), UserModel);
        }

        private async void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            AssignmentModel.SelectedAssignment = UserModel.SelectedAssignment;
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".zip");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var temp = file;
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    flag = await AssignmentModel.submitAssignments(temp);
                }
            }
            if (flag)
            {
                var i = new Windows.UI.Popups.MessageDialog("上传成功！").ShowAsync();
            }
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserModel.SelectedAssignment == null)
            {
                return;
            }
            DataTransferManager.ShowShareUI();
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            Assignment temp = UserModel.SelectedAssignment;
            request.Data.Properties.Title = UserModel.SelectedAssignment.Title;
            request.Data.Properties.Description = UserModel.SelectedAssignment.getContent();
            request.Data.SetText("Title: " + temp.Title + "\n" + "Content: " + temp.getContent() + "\n" +
               "开始日期：" + temp.Start.ToString() + "\n" + "截止日期：" + temp.DDL.ToString());
        }
    }
}
