using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TM_Model;

namespace TM_Database.Repository
{
    public class EventRepository : IEventRepository
    {
    
        private MySQLDBContext context;
        private DbConnection connection;


        public EventRepository()
        {
            this.context = new MySQLDBContext();

            this.connection = context.Database.GetDbConnection();

            this.connection.Open();
        }

        public ObservableCollection<Event> GetAllArtsEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();



            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'ARTS'";

                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.IsDBNull(reader.GetOrdinal("Evt_Performer")) ? string.Empty : reader.GetString(reader.GetOrdinal("Evt_Performer"));
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }
            }
            
            return events;
        }

        public ObservableCollection<Event> GetAllCinemaEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();



            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'CINEMA'";
                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.IsDBNull(reader.GetOrdinal("Evt_Performer")) ? string.Empty : reader.GetString(reader.GetOrdinal("Evt_Performer"));
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }
            }
        
            return events;
        }

        public ObservableCollection<Event> GetAllEvents()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Event> GetAllFamilyEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();



            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'FAMILY'";

                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.IsDBNull(reader.GetOrdinal("Evt_Performer")) ? string.Empty : reader.GetString(reader.GetOrdinal("Evt_Performer"));
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }
            }
            return events;
        }

        public ObservableCollection<Event> GetAllMusicEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

           

                using (var consulta = connection.CreateCommand())
                {

                    consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'MUSIC'";

                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.IsDBNull(reader.GetOrdinal("Evt_Performer")) ? string.Empty : reader.GetString(reader.GetOrdinal("Evt_Performer"));
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }
                       

                    }
                


       
            return events;

        }

        public ObservableCollection<Event> GetAllOtherEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();



            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'OTHER'";

                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.IsDBNull(reader.GetOrdinal("Evt_Performer")) ? string.Empty : reader.GetString(reader.GetOrdinal("Evt_Performer"));
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }

            }
            return events;
        }

        public ObservableCollection<Event> GetAllSportsEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();



            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'SPORTS'";

                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.IsDBNull(reader.GetOrdinal("Evt_Performer")) ?  string.Empty : reader.GetString(reader.GetOrdinal("Evt_Performer"));
                      
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }

            }
                return events;
            
        }

        public ObservableCollection<Event> GetAllTheatreEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();


            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'THEATER'";

                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Evt_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Evt_Name"));
                        string desc = reader.GetString(reader.GetOrdinal("Evt_Description"));
                        string performer = reader.GetString(reader.GetOrdinal("Evt_Performer"));
                        string imatge = reader.GetString(reader.GetOrdinal("Evt_Image"));
                        DateTime data = reader.GetDateTime(reader.GetOrdinal("Evt_Date"));
                        TimeSpan time = ((MySqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("Evt_Time"));

                        string type = reader.GetString(reader.GetOrdinal("Type_Name"));
                        string sala = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        Sala sala1 = new Sala(sala);
                        string estat = reader.GetString(reader.GetOrdinal("St_Name"));

                        // Convert string to enum
                        TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                        Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                        Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                        events.Add(e);
                    }
                }
            }
            
            return events;

        }

        ObservableCollection<Sala> IEventRepository.GetAllSalas()
        {
           ObservableCollection<Sala> salas = new ObservableCollection<Sala>();




            using (var consulta = connection.CreateCommand())
            {
                consulta.CommandText = @"select Sal_Id, Sal_Name, Sal_Municipality, Sal_Address, Sal_MapAvail,
                                        Sal_Seats, Sal_Rows, Sal_Col from sala";
                using (var reader = consulta.ExecuteReader()) // Add using statement here
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(reader.GetOrdinal("Sal_Id"));
                        string nom = reader.GetString(reader.GetOrdinal("Sal_Name"));
                        string municipio = reader.GetString(reader.GetOrdinal("Sal_Municipality"));
                        string address = reader.GetString(reader.GetOrdinal("Sal_Address"));
                        bool mapAvail = reader.GetBoolean(reader.GetOrdinal("Sal_MapAvail"));
                        int seats = reader.GetInt32(reader.GetOrdinal("Sal_Seats"));
                        int rows = reader.GetInt32(reader.GetOrdinal("Sal_Rows"));
                        int col = reader.GetInt32(reader.GetOrdinal("Sal_Col"));
                        Sala s = new Sala(id, nom, municipio, address, mapAvail, seats, rows, col);
                        salas.Add(s);
                    }
                }

            }
                       
  
            return salas;
        }
    }
}

