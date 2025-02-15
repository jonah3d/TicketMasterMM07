using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using TM_Model;

namespace TM_Database.Repository
{
    public interface IRepository
    {

        ObservableCollection<Event> GetAllEvents();
        ObservableCollection<Event> GetAllMusicEvent();
        ObservableCollection<Event> GetAllTheatreEvent();
        ObservableCollection<Event> GetAllSportsEvent();
        ObservableCollection<Event> GetAllCinemaEvent();
        ObservableCollection<Event> GetAllFamilyEvent();
        ObservableCollection<Event> GetAllArtsEvent();
        ObservableCollection<Event> GetAllOtherEvent();
        ObservableCollection<Sala> GetAllSalas();
        
        Event GetEventByName(string name);

        Boolean CreateEvent(Event e);
        Boolean DeleteEvent(Event e);
        Boolean UpdateEvent(Event e);
        Boolean CreateSala(Sala s);
        Boolean DeleteSala(Sala s);

        Boolean CreateZone(ObservableCollection<Zona>z, int salaid);
        Boolean DeleteAllSalaZones(int salaid);
        Boolean checkSalaZones(int salaid);
        Boolean CreateChair(List<Cadira> c);
        Boolean DeleteAllChairZones(List<int> zoneIds);
        List<int>ZoneIds(int salaid);
        Boolean CreateSeats(ObservableCollection<Zona> zones);

        int GetSalaId(String name);
        int GetStatusId(String name);
        int GetTipusId(String name);
       

    }
}
