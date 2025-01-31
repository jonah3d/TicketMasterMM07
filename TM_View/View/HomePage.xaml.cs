using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TM_Database;
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
    public sealed partial class HomePage : Page
    {
        private IRepository eventRepository;
        
        public ObservableCollection<Event> musicEvents { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Event> theatreEvents { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Event> sportsEvents { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Event> cinemaEvents { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Event> familyEvents { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Event> artsEvents { get; set; } = new ObservableCollection<Event>();
        public ObservableCollection<Event> otherEvents { get; set; } = new ObservableCollection<Event>();


        public HomePage()
        {
            this.InitializeComponent();
            try
            {
                    eventRepository = new Repository();


                LoadSportsEvent();
                LoadMusicEvents();
                LoadTheatreEvent();
                LoadCinemaEvent();
                LoadFamilyEvent();
                LoadArtsEvent();
                LoadOtherEvents();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to initialize: {ex.Message}", ex);
            }
        }

        private void LoadMusicEvents()
        {
            try
            {
                var events = eventRepository.GetAllMusicEvent();
                Debug.WriteLine($"Retrieved {events.Count} music events");
                musicEvents.Clear();
                foreach (var ev in events)
                {
                    musicEvents.Add(ev);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }

        private void LoadSportsEvent()
        {
            try
            {
                var events = eventRepository.GetAllSportsEvent();
                sportsEvents.Clear();
                foreach (var ev in events)
                {
                    sportsEvents.Add(ev);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }


      

        private void LoadTheatreEvent()
        {
            try
            {
                var events = eventRepository.GetAllTheatreEvent();
                theatreEvents.Clear();
                foreach (var ev in events)
                {
                    theatreEvents.Add(ev);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }

        private void LoadCinemaEvent()
        {
            try
            {
                var events = eventRepository.GetAllCinemaEvent();
                cinemaEvents.Clear();
                foreach (var ev in events)
                {
                    cinemaEvents.Add(ev);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }

        private void LoadFamilyEvent()
        {
            try
            {
                var events = eventRepository.GetAllFamilyEvent();
                familyEvents.Clear();
                foreach (var ev in events)
                {
                    familyEvents.Add(ev);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }

        private void LoadArtsEvent()
        {
            try
            {
                var events = eventRepository.GetAllArtsEvent();
                artsEvents.Clear();
                foreach (var ev in events)
                {
                    artsEvents.Add(ev);
                    Debug.WriteLine(ev.Nom);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }

        private void LoadOtherEvents()
        {
            try
            {
                var events = eventRepository.GetAllOtherEvent();
                otherEvents.Clear();
                foreach (var ev in events)
                {
                    otherEvents.Add(ev);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading events error: {ex.Message}");
            }
        }
    }
}