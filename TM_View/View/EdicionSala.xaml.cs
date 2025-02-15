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



namespace TM_View.View
{

    public sealed partial class EdicionSala : Page
    {
        private Repository repository;
        private Sala selectedSal;

        private bool isPaintMode = false;
        private Windows.UI.Color currentUIColor = Windows.UI.Colors.Gray;
        private ObservableCollection<Zona> zones = new ObservableCollection<Zona>();
        private Button[,] seatButtons;
        private static SolidColorBrush grey = new SolidColorBrush(Windows.UI.Colors.LightGray);
        private Zona selectedZone;

        public EdicionSala()
        {
            this.InitializeComponent();
            InitializeControls();
            repository = new Repository();

            this.DataContext = this;

        }

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is Sala selectedSala)
            {
                selectedSal = selectedSala;
                if (selectedSala.TeMapa)
                {
                    Tb_salaname.Text = selectedSala.Nom;
                    Tb_SalMunicipi.Text = selectedSala.Municipi;
                    Tb_SalAdreca.Text = selectedSala.Adreca;
                    Tg_Map.IsOn = selectedSala.TeMapa;
                    Tb_SalaCapacity.Text = selectedSala.Seats.ToString();
                    Cmb_SalaCol.SelectedIndex = selectedSala.NumColumnes;
                    Cmb_SalaRow.SelectedIndex = selectedSala.NumFiles;

                    zones.Clear();
                    zones = repository.GetAllZonesFromSala(selectedSala.Id);

                }
                else if(!selectedSala.TeMapa)
                {
                    Tb_salaname.Text = selectedSala.Nom;
                    Tb_SalMunicipi.Text = selectedSala.Municipi;
                    Tb_SalAdreca.Text = selectedSala.Adreca;
                    Tg_Map.IsOn = selectedSala.TeMapa;
                    Tb_SalaCapacity.Text = selectedSala.Seats.ToString();
                    Cmb_SalaCol.SelectedIndex = selectedSala.NumColumnes;
                    Cmb_SalaRow.SelectedIndex = selectedSala.NumFiles;
                }



                Tg_Map_Toggled(Tg_Map, null);
            }
        }

        private void InitializeControls()
        {
            Tg_Map.IsOn = true;

            for (int i = 1; i <= 100; i++)
            {
                Cmb_SalaRow.Items.Add(i);
                Cmb_SalaCol.Items.Add(i);
            }

            Cmb_SalaRow.SelectedIndex = 0;
            Cmb_SalaCol.SelectedIndex = 0;


            Cmb_SalaRow.SelectionChanged += UpdateSeatingGrid;
            Cmb_SalaCol.SelectionChanged += UpdateSeatingGrid;
            Lv_ZonaList.SelectionChanged += Lv_ZonaList_SelectionChanged;
            Btn_PaintZone.Click += Btn_PaintZone_Click;
            Btn_EraseZone.Click += Btn_EraseZone_Click;


            UpdateSeatingGrid(null, null);
        }
        private void UpdateSeatingGrid(object sender, SelectionChangedEventArgs e)
        {
            if (Cmb_SalaRow.SelectedItem == null || Cmb_SalaCol.SelectedItem == null) return;

            int rows = (int)Cmb_SalaRow.SelectedItem;
            int cols = (int)Cmb_SalaCol.SelectedItem;


            GV_Zones.Items.Clear();


            var uniformGrid = new Grid();
            for (int i = 0; i < rows; i++)
                uniformGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            for (int i = 0; i < cols; i++)
                uniformGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

            seatButtons = new Button[rows, cols];


            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var seatButton = new Button
                    {
                        Width = 25,
                        Height = 25,
                        Margin = new Thickness(2),
                        Background = grey
                    };

                    seatButton.Click += SeatButton_Click;
                    Grid.SetRow(seatButton, r);
                    Grid.SetColumn(seatButton, c);
                    uniformGrid.Children.Add(seatButton);
                    seatButtons[r, c] = seatButton;
                }
            }

            var gridViewItem = new GridViewItem();
            gridViewItem.Content = uniformGrid;
            GV_Zones.Items.Add(gridViewItem);


            UpdateTotalCapacity();
        }

        private async void Btn_EditSala_Click(object sender, RoutedEventArgs e)
        {
            var salaname = Tb_salaname.Text;
            var salamunicipi = Tb_SalMunicipi.Text;
            var salaadreca = Tb_SalAdreca.Text;
            var map = Tg_Map.IsOn;
            var totalsalacapacity = Int32.Parse(Tb_SalaCapacity.Text);
            var numfiles = Int32.Parse(Cmb_SalaRow.SelectedItem.ToString());
            var numcolumnes = Int32.Parse(Cmb_SalaCol.SelectedItem.ToString());

            if (map == false)
            {
                totalsalacapacity = 0;
                numfiles = 0;
                numcolumnes = 0;
            }

            

            try
            {
                Sala sala = new Sala
                {
                    Id = selectedSal.Id,
                    Nom = salaname,
                    Municipi = salamunicipi,
                    Adreca = salaadreca,
                    TeMapa = map,
                    Seats = totalsalacapacity,
                    NumFiles = numfiles,
                    NumColumnes = numcolumnes,

                };
                if (repository.EditSala(sala))
                {
                    ContentDialog successcontent = new ContentDialog
                    {
                        Title = "Success",
                        Content = $"Successfully Edited Sala {sala.Nom}",
                        CloseButtonText = "Ok"
                    };
                    await successcontent.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Error Creating Sala {ex.Message}",
                    CloseButtonText = "Ok"
                };
                await errorDialog.ShowAsync();
            }
        }

        private void Btn_EditAuditoriumSala_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Tg_Map_Toggled(object sender, RoutedEventArgs e)
        {
            if (Tg_Map.IsOn == true)
            {
                GV_Zones.Visibility = Visibility.Visible;
                Cmb_SalaCol.Visibility = Visibility.Visible;
                Cmb_SalaRow.Visibility = Visibility.Visible;
                Btn_PaintZone.Visibility = Visibility.Visible;
                Btn_EraseZone.Visibility = Visibility.Visible;
                TB_X.Visibility = Visibility.Visible;
                Sp_Zone.Visibility = Visibility.Visible;
                Btn_EditAuditoriumSala.Visibility = Visibility.Visible;
                Btn_EditSala.Visibility = Visibility.Collapsed;
            }
            else if (Tg_Map.IsOn == false)
            {



                GV_Zones.Visibility = Visibility.Collapsed;
                Cmb_SalaCol.Visibility = Visibility.Collapsed;
                Cmb_SalaRow.Visibility = Visibility.Collapsed;
                Btn_PaintZone.Visibility = Visibility.Collapsed;
                Btn_EraseZone.Visibility = Visibility.Collapsed;
                TB_X.Visibility = Visibility.Collapsed;
                Sp_Zone.Visibility = Visibility.Collapsed;
                Btn_EditSala.Visibility = Visibility.Visible;
                Btn_EditAuditoriumSala.Visibility = Visibility.Collapsed;
            }
        }

        private void Btn_PaintZone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_EraseZone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZonaColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {

        }

        private void Btn_ClearColorSel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_AddZona_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_DelZona_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Lv_ZonaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private int UpdateTotalCapacity()
        {
            int cap = 0;
            if (seatButtons != null)
            {
                int totalCapacity = seatButtons.GetLength(0) * seatButtons.GetLength(1);
                Tb_SalaCapacity.Text = totalCapacity.ToString();
                cap = totalCapacity;
            }

            return cap;
        }

        private System.Drawing.Color ConvertToDrawingColor(Windows.UI.Color uiColor)
        {
            return System.Drawing.Color.FromArgb(uiColor.A, uiColor.R, uiColor.G, uiColor.B);
        }

        private Windows.UI.Color ConvertToUIColor(System.Drawing.Color drawingColor)
        {
            return Windows.UI.Color.FromArgb(
                drawingColor.A,
                drawingColor.R,
                drawingColor.G,
                drawingColor.B
            );
        }

        private System.Drawing.Color ConvertFromHex(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 4), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 6), System.Globalization.NumberStyles.HexNumber);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        private string ConvertToHex(System.Drawing.Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        private int CountSeatsOfColor(Windows.UI.Color color)
        {
            int count = 0;
            foreach (var button in seatButtons)
            {
                if (((SolidColorBrush)button.Background).Color == color)
                    count++;
            }
            return count;
        }

        private async void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedZone == null)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Select An Existing Zone",
                    CloseButtonText = "Ok"
                };
                await errorDialog.ShowAsync();
                return;
            }

            if (sender is Button seatButton)
            {
                int row = -1, col = -1;

                // Find the position of the clicked button
                for (int i = 0; i < seatButtons.GetLength(0); i++)
                {
                    for (int j = 0; j < seatButtons.GetLength(1); j++)
                    {
                        if (seatButtons[i, j] == seatButton)
                        {
                            row = i;
                            col = j;
                            break;
                        }
                    }
                    if (row != -1) break;
                }

                if (isPaintMode)
                {

                    if (CountSeatsOfColor(ConvertToUIColor(selectedZone.Z_Color)) >= selectedZone.Capacitat)
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Alert!",
                            Content = "Zone Capacity Exceeded",
                            CloseButtonText = "Ok"
                        };
                        await errorDialog.ShowAsync();
                        return;
                    }


                    var existingSeat = selectedZone.Cadires.FirstOrDefault(c => c.X == col && c.Y == row);
                    if (existingSeat == null)
                    {

                        Cadira newSeat = new Cadira { X = col, Y = row };
                        selectedZone.Cadires.Add(newSeat);
                        seatButton.Background = new SolidColorBrush(ConvertToUIColor(selectedZone.Z_Color));
                    }
                }
                else
                {
                    var uiColor = ConvertToUIColor(selectedZone.Z_Color);
                    if (((SolidColorBrush)seatButton.Background).Color == uiColor)
                    {

                        var seatToRemove = selectedZone.Cadires.FirstOrDefault(c => c.X == col && c.Y == row);
                        if (seatToRemove != null)
                        {
                            selectedZone.Cadires.Remove(seatToRemove);
                            seatButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
                        }
                    }
                }
            }
        }
    }
}
