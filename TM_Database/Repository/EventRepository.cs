using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TM_Model;

namespace TM_Database.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly MySQLDBContext context;

        public EventRepository(MySQLDBContext context)
        {
            this.context = context;

            
        }

        public ObservableCollection<Event> GetAllEvents()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Event> GetAllMusicEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

            var connection = context.Database.GetDbConnection();

           if(connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            var consulta = connection.CreateCommand();
            consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'MUSIC'";

            var reader = consulta.ExecuteReader();
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

            return events;
        }

    }
}
