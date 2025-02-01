using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TM_View.View;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TM_View
{
    
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainNavigation.ItemInvoked += MainNavigation_ItemInvoked;

            contentFrame.NavigationFailed += ContentFrame_NavigationFailed;
            contentFrame.Navigated += ContentFrame_Navigated;

            contentFrame.Navigate(typeof(HomePage));


            MainNavigation.SelectedItem = MainNavigation.MenuItems[0];

            MainNavigation.BackRequested += MainNavigation_BackRequested;
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Navigation failed",
                Content = "Failed to load page " + e.SourcePageType.Name,
                CloseButtonText = "Ok"
            };
            errorDialog.ShowAsync();
        }

        private void MainNavigation_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (contentFrame.CanGoBack)
            {
                contentFrame.GoBack();
            }
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
           
            MainNavigation.IsBackEnabled = contentFrame.CanGoBack;
        }

     
        private void MainNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var item = args.InvokedItemContainer as NavigationViewItem;
                if (item != null)
                {
                    switch (item.Tag)
                    {
                        case "HomePage":
                            contentFrame.Navigate(typeof(HomePage));
                            break;
                        case "CreateEventPage":
                            contentFrame.Navigate(typeof(CreateEventPage));
                            break;

                            case "EventsPage":
                            contentFrame.Navigate(typeof(EventsPage));
                            break;
                    }
                }
            }
        }
    }
}
