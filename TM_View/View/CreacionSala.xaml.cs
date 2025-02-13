using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using TM_Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace TM_View.View
{

    public sealed partial class CreacionSala : Page
    {
        private bool isPaintMode = false;
        private Windows.UI.Color currentUIColor = Windows.UI.Colors.Gray;
        private ObservableCollection<Zona> zones = new ObservableCollection<Zona>();
        private Button[,] seatButtons;
        private static SolidColorBrush grey = new SolidColorBrush(Windows.UI.Colors.LightGray);
        private Zona selectedZone;

        public CreacionSala()
        {
            this.InitializeComponent();
            InitializeControls();

            Lv_ZonaList.ItemsSource = zones;

            this.DataContext = this;
        }
        private void InitializeControls()
        {
      
            for (int i = 1; i <= 50; i++)
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
        private void ZonaColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            currentUIColor = args.NewColor;
            Btn_ColorBrush.Color = currentUIColor;
        }
        private async void Btn_PaintZone_Click(object sender, RoutedEventArgs e)
        {
     
                isPaintMode = true;
                ZonaColorPicker.Color = currentUIColor;
            
        }
        private void Btn_EraseZone_Click(object sender, RoutedEventArgs e)
        {
            isPaintMode = false;
            currentUIColor = Windows.UI.Colors.LightGray;
            ZonaColorPicker.Color = currentUIColor;

        }

        private void Btn_ClearColorSel_Click(object sender, RoutedEventArgs e)
        {

            Tb_ZonaName.Text = "";
            Tb_ZonaCapacity.Text = "";
            currentUIColor = Windows.UI.Colors.Gray;
            Btn_ColorBrush.Color = currentUIColor;
            Btn_ColorPicker.Background = new SolidColorBrush(currentUIColor);
            Lv_ZonaList.SelectedItem = null;
            selectedZone = null;
        }
        private void Btn_AddZona_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Tb_ZonaName.Text)) return;

            int capacity = Int32.Parse(Tb_ZonaCapacity.Text);
            var drawingColor = ConvertToDrawingColor(currentUIColor);

            var zone = new Zona(
                Tb_ZonaName.Text,
                capacity,
                drawingColor
            );

            zones.Add(zone);
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
            }else if( selectedZone != null)
            {
                if (sender is Button seatButton)
                {

                    var ZoneCapacity = selectedZone.Capacitat;

                    for(int i = 0; i < seatButtons.GetLength(0); i++)
                    {
                        for (int j = 0; j < seatButtons.GetLength(1); j++)
                        {
                            if (seatButtons[i, j] == seatButton)
                            {
                                if (isPaintMode)
                                {
                                    if (CountSeatsOfColor(ConvertToUIColor(selectedZone.Z_Color)) >= ZoneCapacity)
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
                                }
                                else
                                {
                                    seatButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
                                    return;
                                }
                            }
                        }
                    }

                    seatButton.Background = new SolidColorBrush(ConvertToUIColor(selectedZone.Z_Color));

                    Cadira newSeat = new Cadira();
                    selectedZone.Cadires.Add(newSeat);
                }
            }

        }

        private void Btn_DelZona_Click(object sender, RoutedEventArgs e)
        {
            if (Lv_ZonaList.SelectedItem is Zona selectedZone)
            {
                var uiColor = ConvertToUIColor(selectedZone.Z_Color);

           
                foreach (var button in seatButtons)
                {
                    if (((SolidColorBrush)button.Background).Color == uiColor)
                    {
                        button.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
                    }
                }

                zones.Remove(selectedZone);
       
                UpdateTotalCapacity();
            }
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

        private void GV_Zones_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
        }

        private void Lv_ZonaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lv_ZonaList.SelectedItem is Zona zona)
            {
                selectedZone = zona;
            }
          
                Tb_ZonaName.Text = selectedZone.Nom;
                Tb_ZonaCapacity.Text = selectedZone.Capacitat.ToString();
                ZonaColorPicker.Color = ConvertToUIColor(selectedZone.Z_Color);
                Btn_ColorPicker.Background = new SolidColorBrush(ConvertToUIColor(selectedZone.Z_Color));
            
        
        }
    }
}
