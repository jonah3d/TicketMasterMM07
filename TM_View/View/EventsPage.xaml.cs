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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

     
    }
}