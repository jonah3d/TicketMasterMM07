using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TM_Database;
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


namespace TM_View.View
{
    public sealed partial class HomePage : Page
    {
        private IEventRepository eventRepository;
        private MySQLDBContext context;
        private DbConnection dBconnection;
        public ObservableCollection<Event> musicEvents { get; set; } = new ObservableCollection<Event>();

        public HomePage()
        {
            this.InitializeComponent();
            try
            {
                context = new MySQLDBContext();
                dBconnection = context.Database.GetDbConnection();

                dBconnection.Open();
                if (dBconnection.State == System.Data.ConnectionState.Open)
                {
                    eventRepository = new EventRepository(dBconnection);
                }
                else
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Connection error",
                        Content = "Failed to connect to the database",
                        CloseButtonText = "Ok"
                    };
                }


                LoadMusicEvents();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to initialize: {ex.Message}", ex);
            }
        }

        private void LoadMusicEvents()
        {
            try
            {
                musicEvents = eventRepository.GetAllMusicEvent();
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }
    }
}