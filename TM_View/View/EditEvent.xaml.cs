using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace TM_View.View
{

    public sealed partial class EditEvent : Page
    {
        private IRepository eventRepository;
        private ObservableCollection<Sala> salas { get; set; } = new ObservableCollection<Sala>();
        private int EventId;
        public EditEvent()
        {
            this.InitializeComponent();
            eventRepository = new Repository();
            cmb_status.ItemsSource = Enum.GetValues(typeof(Estat));
            cmb_type.ItemsSource = Enum.GetValues(typeof(TipusEvent));
            cmb_sala.ItemsSource = salas;
            loadSalas();

            this.DataContext = this;
        }

  

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Event selectedEvent)
            {
                EventId = selectedEvent.Id;
                Tb_EvtNom.Text = selectedEvent.Nom;
                Tb_EvtDescription.Text = selectedEvent.Desc;
                Tb_EvtPerformer.Text = selectedEvent.Protagonista;
                CDP_EvtDate.Date = selectedEvent.Data;
                TP_EvtTime.Time = selectedEvent.Time;
                Tb_imgeurl.Text = selectedEvent.ImatgePath;
                Img_EvtImg.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(selectedEvent.ImatgePath));

                cmb_sala.SelectedItem = salas.FirstOrDefault(s => s.Nom == selectedEvent.Sala.Nom);
                cmb_status.SelectedItem = selectedEvent.Status;
                cmb_type.SelectedItem = selectedEvent.Tipus;

            }
        }
        private async Task updateEvent()
        {
            string name = Tb_EvtNom.Text;
            string description = Tb_EvtDescription.Text;
            DateTime date = CDP_EvtDate.Date.Value.DateTime;

            TimeSpan? selectedTime = TP_EvtTime.SelectedTime;
            if (selectedTime == null)
            {
                await ShowErrorDialogue("Please select a time for the event.");
                return;
            }
            TimeSpan time = selectedTime.Value;

            if (cmb_type.SelectedItem == null || cmb_status.SelectedItem == null || cmb_sala.SelectedItem == null)
            {
                await ShowErrorDialogue("Please select a type, status, and sala.");
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
                Event newEvent = new Event(EventId, name, description, date, time, eventType, status, performer, sala, imagePath);
                if (eventRepository.UpdateEvent(newEvent))
                {
                    await ShowSuccessDialogue("Event Updated successfully");
                }
                else
                {
                    await ShowErrorDialogue("Error Updating event");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogue(ex.Message);
            }
        }

        private static async Task ShowErrorDialogue(string message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "Ok"
            };
            await errorDialog.ShowAsync();
        }

        private static async Task ShowSuccessDialogue(string message)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "Ok"
            };
            await successDialog.ShowAsync();
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

        private  async void Btn_EditEvent_Click(object sender, RoutedEventArgs e)
        {
          await  updateEvent();

        }
    }
}
