using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private Color currentColor = Colors.Gray;
      private ObservableCollection<Zona> zones = new ObservableCollection<Zona>();
        private Button[,] seatButtons;

        public CreacionSala()
        {
            this.InitializeComponent();
            InitializeControls();
        }
        private void InitializeControls()
        {
            // Initialize ComboBoxes with values 1-50
            for (int i = 1; i <= 50; i++)
            {
                Cmb_SalaRow.Items.Add(i);
                Cmb_SalaCol.Items.Add(i);
            }

            // Set default selections
            Cmb_SalaRow.SelectedIndex = 0;
            Cmb_SalaCol.SelectedIndex = 0;

            // Add event handlers
            Cmb_SalaRow.SelectionChanged += UpdateSeatingGrid;
            Cmb_SalaCol.SelectionChanged += UpdateSeatingGrid;
            Btn_PaintZone.Click += Btn_PaintZone_Click;
            Btn_EraseZone.Click += Btn_EraseZone_Click;

            // Initial grid setup
            UpdateSeatingGrid(null, null);
        }
        private void UpdateSeatingGrid(object sender, SelectionChangedEventArgs e)
        {
            if (Cmb_SalaRow.SelectedItem == null || Cmb_SalaCol.SelectedItem == null) return;

            int rows = (int)Cmb_SalaRow.SelectedItem;
            int cols = (int)Cmb_SalaCol.SelectedItem;

            // Clear existing grid
            GV_Zones.Items.Clear();

            // Create uniform grid for seats
            var uniformGrid = new Grid();
            for (int i = 0; i < rows; i++)
                uniformGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            for (int i = 0; i < cols; i++)
                uniformGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

            seatButtons = new Button[rows, cols];

            // Create seat buttons
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var seatButton = new Button
                    {
                        Width = 25,
                        Height = 25,
                        Margin = new Thickness(2),
                        Background = new SolidColorBrush(Colors.LightGray)
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

            // Update capacity
            UpdateTotalCapacity();
        }
        private void ZonaColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            currentColor = args.NewColor;
            Btn_ColorBrush.Color = currentColor;
        }

        private void Btn_PaintZone_Click(object sender, RoutedEventArgs e)
        {
            isPaintMode = !isPaintMode;
            Btn_PaintZone.Background = isPaintMode ?
                new SolidColorBrush(Colors.LightGreen) :
                new SolidColorBrush(Colors.Transparent);
        }

        private void Btn_EraseZone_Click(object sender, RoutedEventArgs e)
        {
            isPaintMode = false;
            currentColor = Colors.LightGray;
            Btn_PaintZone.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Btn_ClearColorSel_Click(object sender, RoutedEventArgs e)
        {
            currentColor = Colors.Gray;
            Btn_ColorBrush.Color = currentColor;
        }

        private void Btn_AddZona_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Tb_ZonaName.Text)) return;

            var zone = new Zona(Tb_ZonaName.Text, CountSeatsOfColor(currentColor),currentColor);
         

            zones.Add(zone);
            UpdateZonesList();
            UpdateTotalCapacity();
        }
        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button seatButton)
            {
                if (isPaintMode)
                {
                    seatButton.Background = new SolidColorBrush(currentColor);
                }
            }
        }

        private void Btn_DelZona_Click(object sender, RoutedEventArgs e)
        {
            if (Lv_ZonaList.SelectedItem is Zona selectedZone)
            {
                // Change all seats of this color back to default
                foreach (var button in seatButtons)
                {
                    if (((SolidColorBrush)button.Background).Color == selectedZone.color)
                    {
                        button.Background = new SolidColorBrush(Colors.LightGray);
                    }
                }

                zones.Remove(selectedZone);
                UpdateZonesList();
                UpdateTotalCapacity();
            }
        }
        private int CountSeatsOfColor(Color color)
        {
            int count = 0;
            foreach (var button in seatButtons)
            {
                if (((SolidColorBrush)button.Background).Color == color)
                    count++;
            }
            return count;
        }

        private void UpdateZonesList()
        {
            Lv_ZonaList.Items.Clear();
            foreach (var zone in zones)
            {
                Lv_ZonaList.Items.Add(zone);
            }
        }

        private void UpdateTotalCapacity()
        {
            if (seatButtons != null)
            {
                int totalCapacity = seatButtons.GetLength(0) * seatButtons.GetLength(1);
                Tb_SalaCapacity.Text = totalCapacity.ToString();
            }
        }
    }
}
