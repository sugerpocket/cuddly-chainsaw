using cuddly_chainsaw.Models;
using cuddly_chainsaw.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace cuddly_chainsaw
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        PaneThemeTransition PaneAnim = new PaneThemeTransition { Edge = EdgeTransitionLocation.Left };
        AssignmentViewModel AssignmentModel;
        UserViewModel UserModel;
        public LoginPage()
        {
            ManipulationCompleted += AppleAnimationPage_ManipulationCompleted;
            Transitions = new TransitionCollection();
            Transitions.Add(PaneAnim);
            ManipulationMode = ManipulationModes.TranslateX;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PaneAnim.Edge = e.NavigationMode == NavigationMode.Back ? EdgeTransitionLocation.Left : EdgeTransitionLocation.Right;
            base.OnNavigatedTo(e);
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            PaneAnim.Edge = e.NavigationMode != NavigationMode.Back ? EdgeTransitionLocation.Left : EdgeTransitionLocation.Right;
        }

        private void AppleAnimationPage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var trans = e.Cumulative.Translation;
            double DeltaX = Math.Abs(trans.X);
            if (Math.Abs(trans.Y) * 3 < DeltaX && DeltaX > ActualWidth / 2)
            {
                if (trans.X > 0)
                {
                    if (Frame.CanGoBack)
                        Frame.GoBack();
                }
                else
                {
                    if (Frame.CanGoForward)
                        Frame.GoForward();
                }
            }
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            UserModel = new UserViewModel();
            string user = username.Text;
            string psw = password.Password;
            Boolean isLogin = await UserModel.logIn(user, psw);
            if (isLogin)
            {
                AssignmentModel = new AssignmentViewModel();
                await AssignmentModel.getAllAssignments();
                var updator = TileUpdateManager.CreateTileUpdaterForApplication();
                updator.Clear();
                updator.EnableNotificationQueue(true);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(File.ReadAllText("Tile.xml"));
                XmlNodeList texts = xmlDoc.GetElementsByTagName("text");
                XmlNodeList images = xmlDoc.GetElementsByTagName("image");
                foreach (Assignment ass in AssignmentModel.DoingAssignments)
                {
                    if (!ass.isEnded())
                    {
                        string pic = "Assets/header.jpeg";
                        if (ass.Type > 0 && ass.Type < 5)
                        {
                            pic = "Assets/" + ass.Type + ".jpg";
                        }
                        int count = 0;
                        int countImage = 0;
                        // Small
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        ((XmlElement)images[countImage]).SetAttribute("src", pic);
                        count++;
                        countImage++;

                        // Medium
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;
                        ((XmlElement)texts[count]).InnerText = ass.DDL.ToString();
                        ((XmlElement)images[countImage]).SetAttribute("src", pic);
                        count++;
                        countImage++;

                        // Wide
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;
                        ((XmlElement)texts[count]).InnerText = ass.DDL.ToString();
                        ((XmlElement)images[countImage]).SetAttribute("src", pic);
                        count++;
                        countImage++;

                        //Large
                        ((XmlElement)texts[count]).InnerText = ass.Title;
                        count++;
                        ((XmlElement)texts[count]).InnerText = ass.DDL.ToString();
                        ((XmlElement)images[countImage]).SetAttribute("src", pic);


                        TileNotification notification = new TileNotification(xmlDoc);
                        updator.Update(notification);
                    }
                }
                
                this.Frame.Navigate(typeof(MainPage), UserModel);
            }
        }

        private void sign_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpPage));
        }
    }
}
