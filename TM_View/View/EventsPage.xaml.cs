using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TM_Database.Repository;
using TM_Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TM_View.View
{
   
    public sealed partial class EventsPage : Page
    {

        private IRepository repository;
        private ObservableCollection<Event> events = new ObservableCollection<Event>();

        public EventsPage()
        {
            this.InitializeComponent();
            this.repository = new Repository();
            loadAllEvents();
         


            Dg_Events.ItemsSource = events;

            this.DataContext = this;

            Btn_EditEvent.IsEnabled = false;
            Btn_DeleteEvent.IsEnabled = false;
            Btn_AddEvent.IsEnabled = true;

            Dg_Events.SelectionChanged += Dg_Events_SelectionChanged;

        }


        void loadAllEvents()
        {
            try
            {
                var allEvents = repository.GetAllEvents();
                events.Clear();
                foreach (var item in allEvents)
                {
                    events.Add(item);
                }
            }
            catch (Exception e)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = e.Message,
                    CloseButtonText = "Ok"
                };

            }
        }

        private void Btn_AddEvent_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = Window.Current.Content as Frame;
            if (mainPage != null)
            {
                var navView = (mainPage.Content as MainPage)?.FindName("contentFrame") as Frame;
                if (navView != null)
                {
                    navView.Navigate(typeof(CreateEventPage),null);
                }
            }
        }

        private void Btn_EditEvent_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = Window.Current.Content as Frame;
            var selectedEvent = Dg_Events.SelectedItem as Event;
            if (mainPage != null)
            {
                var navView = (mainPage.Content as MainPage)?.FindName("contentFrame") as Frame;
                if (navView != null)
                {
                    navView.Navigate(typeof(EditEvent), selectedEvent);
                }
            }
        }

        private async void Btn_DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            Event selectedEvent = Dg_Events.SelectedItem as Event;

            if (selectedEvent != null)
            {
                try
                {
                    if (repository.DeleteEvent(selectedEvent))
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Success!",
                            Content = "Event Deleted Successfully",
                            CloseButtonText = "Ok"
                        };
                        await errorDialog.ShowAsync();
                       loadAllEvents();
                    }
                    else
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "Error deleting event",
                            CloseButtonText = "Ok"
                        };
                        await errorDialog.ShowAsync();

                    }
                      
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = ex.Message,
                        CloseButtonText = "Ok"
                    };
                   await errorDialog.ShowAsync();
                }
            }

        }

        private void Dg_Events_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelection = Dg_Events.SelectedItem != null;

           
            Btn_EditEvent.IsEnabled = hasSelection;
            Btn_DeleteEvent.IsEnabled = hasSelection;

           
            Btn_AddEvent.IsEnabled = !hasSelection;
        }

        private async void Btn_EventSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             var searchedEvent = repository.GetEventByName(SearchBox.Text);
                if (searchedEvent != null)
                {
                    events.Clear();
                    events.Add(searchedEvent);
                }
                else
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "No Event With The Specified Name In The Database",
                        CloseButtonText = "Ok"
                    };
                    await errorDialog.ShowAsync();
                }

            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok"
                };
                await errorDialog.ShowAsync();
            }

            
        }

        private void FI_clearsearch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            events.Clear();
            SearchBox.ClearValue(TextBox.TextProperty);
            loadAllEvents();

        }
    }
}