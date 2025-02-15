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

    public sealed partial class AuditoriumList : Page
    {
        private IRepository eventRepository;

        private ObservableCollection<Sala> salas { get; set; } = new ObservableCollection<Sala>();
        private Sala selectedSala;

        public AuditoriumList()
        {
            this.InitializeComponent();
            eventRepository = new Repository();

            loadSalas();

            Dg_Salas.ItemsSource = salas;

            this.DataContext = this;

            Btn_AddSala.IsEnabled = true;
            Btn_DelSala.IsEnabled = false;
            Btn_EditSala.IsEnabled = false;

            Dg_Salas.SelectionChanged += Dg_Salas_SelectionChanged;
        }

        private void Btn_AuditoriumSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_SearchClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dg_Salas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelection = Dg_Salas.SelectedItem != null;

            Btn_DelSala.IsEnabled = hasSelection;
            Btn_EditSala.IsEnabled = hasSelection;
            Btn_AddSala.IsEnabled = !hasSelection;
        }

        private void Btn_AddSala_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = Window.Current.Content as Frame;
            if (mainPage != null)
            {
                var navView = (mainPage.Content as MainPage)?.FindName("contentFrame") as Frame;
                if (navView != null)
                {
                    navView.Navigate(typeof(CreacionSala), null);
                }
            }
        }

        private async void Btn_DelSala_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedSala = Dg_Salas.SelectedItem as Sala;
                if (selectedSala != null)
                {
                    bool hasZones = eventRepository.checkSalaZones((int)selectedSala.Id);

                 
                    if (hasZones)
                    {
                        bool zonesDeleted = eventRepository.DeleteAllSalaZones((int)selectedSala.Id);
                        if (!zonesDeleted)
                        {
                            ContentDialog errorDialog = new ContentDialog
                            {
                                Title = "Error",
                                Content = $"Failed to delete zones for sala {selectedSala.Nom}",
                                CloseButtonText = "Ok"
                            };
                            await errorDialog.ShowAsync();
                            return;
                        }
                    }

               
                    if (eventRepository.DeleteSala(selectedSala))
                    {
                        ContentDialog successContent = new ContentDialog
                        {
                            Title = "Success",
                            Content = $"Successfully Deleted Sala {selectedSala.Nom}",
                            CloseButtonText = "Ok"
                        };
                        await successContent.ShowAsync();
                    }
                    loadSalas();
                }
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Error Deleting Sala: {ex.Message}",
                    CloseButtonText = "Ok"
                };
                await errorDialog.ShowAsync();
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

        private void Btn_EditSala_Click(object sender, RoutedEventArgs e)
        {
            selectedSala = Dg_Salas.SelectedItem as Sala;
            if (selectedSala != null)
            {
                var mainPage = Window.Current.Content as Frame;
                if (mainPage != null)
                {
                    var navView = (mainPage.Content as MainPage)?.FindName("contentFrame") as Frame;
                    if (navView != null)
                    {
                        navView.Navigate(typeof(EdicionSala), selectedSala);
                    }
                }
            }

        }

 
    }
}
