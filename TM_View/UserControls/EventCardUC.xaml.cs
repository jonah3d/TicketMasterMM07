using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TM_View.UserControls
{
    public sealed partial class EventCardUC : UserControl
    {
        public EventCardUC()
        {
            this.InitializeComponent();
            //this.DataContext = this; 
        }






        public BitmapImage TheImage
        {
            get { return (BitmapImage)GetValue(TheImageProperty); }
            set { SetValue(TheImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TheImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TheImageProperty =
            DependencyProperty.Register("TheImage", typeof(BitmapImage), typeof(EventCardUC), new PropertyMetadata(null));



        public Event TheEvent
        {
            get { return (Event)GetValue(TheEventProperty); }
            set { SetValue(TheEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TheEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TheEventProperty =
            DependencyProperty.Register("TheEvent", typeof(Event), typeof(EventCardUC), new PropertyMetadata(null, OnEventChangedfStatic));

        private static void OnEventChangedfStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EventCardUC)d).OnEventChanged(e);
        }
        private void OnEventChanged(  DependencyPropertyChangedEventArgs e)
        {
            TheImage = new BitmapImage(new Uri(TheEvent.ImatgePath));
        }



        //public string Title
        //{
        //    get => EventTitle.Text;
        //    set => EventTitle.Text = value;
        //}

        //public string Status
        //{
        //    get => EventStatus.Text;
        //    set => EventStatus.Text = value;
        //}

        //public string ImagePath
        //{
        //    set
        //    {

        //        if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
        //        {
        //            EventImage.Source = new BitmapImage(new Uri(value)); 
        //        }
        //        else
        //        {

        //            EventImage.Source = null;
        //        }
        //    }
        //}
    }
}
