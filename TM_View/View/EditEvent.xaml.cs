using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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


namespace TM_View.View
{

    public sealed partial class EditEvent : Page
    {
        private IRepository eventRepository;
        private ObservableCollection<Sala> salas { get; set; } = new ObservableCollection<Sala>();
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

        private void Btn_CreateEvent_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Event selectedEvent)
            {
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

    }
}
