using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TM_Database.Repository;
using TM_Database;
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
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TM_View.View
{
 
    public sealed partial class CreateEventPage : Page
    {
        private Estat estat;
        private IEventRepository eventRepository;
        private MySQLDBContext context;
        private DbConnection dBconnection;
        private ObservableCollection<Sala> salas { get; set; } = new ObservableCollection<Sala>();
        public CreateEventPage()
        {
            this.InitializeComponent();

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

            cmb_status.ItemsSource = Enum.GetValues(typeof(Estat));
            cmb_type.ItemsSource = Enum.GetValues(typeof(TipusEvent));
           cmb_sala.ItemsSource = salas;
            loadSalas();
            this.DataContext = this;
        }

        public void loadSalas()
        {
            try
            {
                var retrievedSalas = eventRepository.GetAllSalas();
                salas.Clear(); 
                foreach (var sala in retrievedSalas)
                {
                    salas.Add(sala); 
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }

    }
}
