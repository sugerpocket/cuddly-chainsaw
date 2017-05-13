using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using cuddly_chainsaw.ViewModels;
using cuddly_chainsaw.Models;
using System.Threading;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace cuddly_chainsaw
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //="{StaticResource ResourceKey=Light}"

            //  user.init();
            
        }
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        AssignmentViewModel temp;

        async void test()
        {
            Assignment ass1 = new Assignment("test12", "doing", 0, 1, new DateTime(2020, 1, 1));

            //Assignment ass2 = new Assignment("test9", "doing", 0, 1, new DateTime(2022, 1, 1));
            //Assignment ass3 = new Assignment("test10", "done", 0, 1, new DateTime(2033, 1, 1));
            //Assignment ass4 = new Assignment("test11", "done", 0, 1, new DateTime(2035, 1, 1));

            //await user.logIn("15331060", "123456");
            DateTime tempTime = DateTime.Now;
            Boolean flag = true;
            while (tempTime.AddSeconds(3.0).CompareTo(DateTime.Now) > 0)
                if (flag)
                {
                    temp = new AssignmentViewModel();
                    flag = false;
                }
            //allDone.Set();

            //add newAssignments

            //allDone.WaitOne();

            //temp.newAssignments(ass2);
            //temp.newAssignments(ass3);
            //temp.newAssignments(ass4);

            //update Assignments test4
            tempTime = DateTime.Now;
            flag = true;
            while (tempTime.AddSeconds(3.0).CompareTo(DateTime.Now) > 0)
                if(flag)
                {
                    flag = await temp.newAssignments(ass1);
                }
            flag = true;
            temp.SelectedAssignment = temp.AllAssignments.Last();
            temp.SelectedAssignment.setTitle("ChangedTest4");
            //temp.SelectedAssignment = ass4;
            tempTime = DateTime.Now;
            while (tempTime.AddSeconds(3.0).CompareTo(DateTime.Now) > 0)
                if(flag)
                {
                    flag = await temp.updateAssignments();
                }
            flag = true;
            temp.SelectedAssignment = temp.AllAssignments.First();
            tempTime = DateTime.Now;
            while (tempTime.AddSeconds(3.0).CompareTo(DateTime.Now) > 0)
                if (flag)
                {
                    flag = await temp.deleteAssignments();
                }
            ////delete Assignment test3
            //temp.SelectedAssignment = ass3;
            //temp.deleteAssignments();
        }

        ViewModels.UserViewModel user = new ViewModels.UserViewModel();
        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    //rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
