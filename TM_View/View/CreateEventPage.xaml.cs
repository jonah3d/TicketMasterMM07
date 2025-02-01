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
using Microsoft.Extensions.Logging;
using Windows.UI.Core;


namespace TM_View.View
{
 
    public sealed partial class CreateEventPage : Page
    {
        private Estat estat;
        private IRepository eventRepository;
        private MySQLDBContext context;
        private DbConnection dBconnection;
        private ObservableCollection<Sala> salas { get; set; } = new ObservableCollection<Sala>();


        public CreateEventPage()
        {
            this.InitializeComponent();

          
              eventRepository = new Repository();
           

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
                Debug.WriteLine($"Retrieved {salas.Count} salas");
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

        private async void btn_EvtImgSelector_Click(object sender, RoutedEventArgs e)
        {
            TextBox inputTextBox = new TextBox();
            ContentDialog dialog = new ContentDialog
            {
                Title = "Enter Image Url",
                Content = inputTextBox,
                PrimaryButtonText = "Select",
                CloseButtonText = "Cancel"
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string userInput = inputTextBox.Text;
              
            }
        }

        private void Btn_CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            string name = Tb_EvtNom.Text;
            string description = Tb_EvtDescription.Text;
            DateTime date = CDP_EvtDate.Date.Value.DateTime;


            string eventTypeString =  cmb_type.SelectedItem.ToString();
            TipusEvent eventType = (TipusEvent)Enum.Parse(typeof(TipusEvent), eventTypeString);

            string statusString = cmb_status.SelectedItem.ToString();
            Estat status = (Estat)Enum.Parse(typeof(Estat), statusString);

            string performer = Tb_EvtPerformer.Text;

          cmb_sala.SelectedItem.ToString();
        }
    }
}
