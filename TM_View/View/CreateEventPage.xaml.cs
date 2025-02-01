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

  
        private void Btn_CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            if (inputValidation())
            {
                turnofvisibility();
                CreatenewEvent();
            }
        }

        private async void CreatenewEvent()
        {
           
            string name = Tb_EvtNom.Text;
            string description = Tb_EvtDescription.Text;
            DateTime date = CDP_EvtDate.Date.Value.DateTime;
            TimeSpan time = TP_EvtTime.SelectedTime.Value;

            if (cmb_type.SelectedItem == null || cmb_status.SelectedItem == null || cmb_sala.SelectedItem == null)
            {

                ContentDialog noSelectionDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please select a type, status, and sala.",
                    CloseButtonText = "Ok"
                };
              await   noSelectionDialog.ShowAsync();
                return;
            }

            string eventTypeString = cmb_type.SelectedItem.ToString();
            TipusEvent eventType = (TipusEvent)Enum.Parse(typeof(TipusEvent), eventTypeString);

            string statusString = cmb_status.SelectedItem.ToString();
            Estat status = (Estat)Enum.Parse(typeof(Estat), statusString);

            string performer = Tb_EvtPerformer.Text;
            Sala sala = (Sala)cmb_sala.SelectedItem;

            string imagePath = Tb_imgeurl.Text;
            Img_EvtImg.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(imagePath));

            try
            {
                Event newEvent = new Event(name, description, date, time, eventType, status, performer, sala, imagePath);
                if (eventRepository.CreateEvent(newEvent))
                {
                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Success",
                        Content = "Event created successfully",
                        CloseButtonText = "Ok"
                    };
                  await  successDialog.ShowAsync();
                }
                else
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Error creating event",
                        CloseButtonText = "Ok"
                    };
                 await   errorDialog.ShowAsync();
                }
            }catch(Exception ex)
            {
              ShowErrorDialogue(ex);
            }

            RTB_Error.Text = $"{name} {description} {date} {time} {eventType} {status} {performer} {sala.Nom} {imagePath}";
        }
        private static async void ShowErrorDialogue(Exception ex)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = ex.Message,
                CloseButtonText = "Ok"
            };
            await errorDialog.ShowAsync();
        }

        private Boolean inputValidation()
        {
            

            bool isValid = true;


            if (string.IsNullOrWhiteSpace(Tb_EvtNom.Text))
            {
                Err_Name.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Tb_EvtDescription.Text))
            {
                Err_Description.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Tb_EvtPerformer.Text))
            {
                Err_Performer.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (CDP_EvtDate.Date == null)
            {
                Err_Date.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (TP_EvtTime.SelectedTime == null)
            {
                Err_Time.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (cmb_type.SelectedItem == null)
            {
                Err_Type.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (cmb_status.SelectedItem == null)
            {
                Err_Status.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (cmb_sala.SelectedItem == null)
            {
                Err_Sala.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Tb_imgeurl.Text))
            {
                Err_Img.Visibility = Visibility.Visible;
                isValid = false;
            }

            return isValid;
        }

        private void turnofvisibility()
        {
            Err_Name.Visibility = Visibility.Collapsed;
            Err_Description.Visibility = Visibility.Collapsed;
            Err_Performer.Visibility = Visibility.Collapsed;
            Err_Date.Visibility = Visibility.Collapsed;
            Err_Time.Visibility = Visibility.Collapsed;
            Err_Type.Visibility = Visibility.Collapsed;
            Err_Status.Visibility = Visibility.Collapsed;
            Err_Sala.Visibility = Visibility.Collapsed;
            Err_Img.Visibility = Visibility.Collapsed;
            Err_Status.Visibility = Visibility.Collapsed;
        }
    }
}