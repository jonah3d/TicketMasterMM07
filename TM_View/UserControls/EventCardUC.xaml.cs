using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TM_View.UserControls
{
    public sealed partial class EventCardUC : UserControl
    {
        public EventCardUC()
        {
            this.InitializeComponent();
        }
        public string Title
        {
            get => EventTitle.Text;
            set => EventTitle.Text = value;
        }

        public string Status
        {
            get => EventStatus.Text;
            set => EventStatus.Text = value;
        }

        public string ImagePath
        {
            set => EventImage.Source = new BitmapImage(new System.Uri("ms-appx:///" + value));
        }
    }
}
