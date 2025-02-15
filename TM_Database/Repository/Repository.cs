using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TM_Model;
using static System.Net.Mime.MediaTypeNames;

namespace TM_Database.Repository
{
    public class Repository : IRepository
    {


        public Repository()
        {
            
        }

        public bool CreateChair(List<Cadira> c)
        {
            throw new NotImplementedException();
        }

        public Boolean CreateEvent(Event e)
        {
            string name = e.Nom;
            string description = e.Desc;
            string performer = e.Protagonista;
            string image = e.ImatgePath;
            DateTime date = e.Data;
            TimeSpan time = e.Time;
            string type = e.Tipus.ToString();

            int typeID = GetTipusId(type);

            Sala sala = e.Sala;
            int salaId = GetSalaId(sala.Nom);

            string estat = e.Status.ToString();

            int estatID = GetStatusId(estat);

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (var consulta = connection.CreateCommand())
                                {
                                    consulta.Transaction = transaction;
                                    consulta.CommandText = @"insert into event (Evt_Name, Evt_Description, Evt_Performer, Evt_Image, Evt_Date, Evt_Time, Evt_Type_Id, Evt_Sala_Id, Evt_Estat_Id) 
                                                        values (@name, @description, @performer, @image, @date, @time, @type, @sala, @estat)";
                                    consulta.Parameters.Add(new MySqlParameter("@name", name));
                                    consulta.Parameters.Add(new MySqlParameter("@description", description));
                                    consulta.Parameters.Add(new MySqlParameter("@performer", performer));
                                    consulta.Parameters.Add(new MySqlParameter("@image", image));
                                    consulta.Parameters.Add(new MySqlParameter("@date", date));
                                    consulta.Parameters.Add(new MySqlParameter("@time", time));
                                    consulta.Parameters.Add(new MySqlParameter("@type", typeID));
                                    consulta.Parameters.Add(new MySqlParameter("@sala", salaId));
                                    consulta.Parameters.Add(new MySqlParameter("@estat", estatID));
                                    consulta.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                return true;

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Can't Create Event {ex.Message}");
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't Create Event {ex.Message}");
            }
        }

        public bool CreateSala(Sala s)
        {
            if (s == null)
            {
                throw new Exception(" Can't Pass A Null Sala");
            }

            string name = s.Nom;
            string municipi = s.Municipi;
            string adreca = s.Adreca;
            bool teMapa = s.TeMapa;
            int totalSeats = s.Seats;
            int numFiles = s.NumFiles;
            int numColumnes = s.NumColumnes;

            try
            {

                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (var consulta = connection.CreateCommand())
                                {
                                    consulta.Transaction = transaction;
                                    consulta.CommandText = @"insert into sala (Sal_Name, Sal_Municipality, Sal_Address, Sal_MapAvail, Sal_Seats, Sal_Rows, Sal_Col) 
                                                        values (@name, @municipi, @adreca, @teMapa, @totalSeats, @numFiles, @numColumnes)";
                                    consulta.Parameters.Add(new MySqlParameter("@name", name));
                                    consulta.Parameters.Add(new MySqlParameter("@municipi", municipi));
                                    consulta.Parameters.Add(new MySqlParameter("@adreca", adreca));
                                    consulta.Parameters.Add(new MySqlParameter("@teMapa", teMapa));
                                    consulta.Parameters.Add(new MySqlParameter("@totalSeats", totalSeats));
                                    consulta.Parameters.Add(new MySqlParameter("@numFiles", numFiles));
                                    consulta.Parameters.Add(new MySqlParameter("@numColumnes", numColumnes));
                                    consulta.ExecuteNonQuery();
                                }
                                transaction.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Can't Create Sala {ex.Message}");
                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                throw new Exception($"Can't Create Sala {e.Message}");

            }
        }

        public bool CreateZone(ObservableCollection<Zona> z, int salaid)
        {
            if (z == null)
            {
                throw new Exception("Can't Pass A Null Zone");
            }

            using (MySQLDBContext context = new MySQLDBContext())
            {
                using (var connection = context.Database.GetDbConnection())
                {
                    connection.Open();
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        throw new Exception("Can't Open Connection");
                    }
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var consulta = connection.CreateCommand())
                            {
                                consulta.Transaction = transaction;
                                foreach (Zona zona in z)
                                {
                                    consulta.Parameters.Clear();
                                    consulta.CommandText = @"insert into zone (Zone_Name, Zone_Capacity, Zone_Color, Zone_Sal_Id) values (@Zname, @capacity, @color,@salid)";
                                    consulta.Parameters.Add(new MySqlParameter("@Zname", zona.Nom));
                                    consulta.Parameters.Add(new MySqlParameter("@capacity", zona.Capacitat));
                                    consulta.Parameters.Add(new MySqlParameter("@color", ConvertToHex(zona.Z_Color)));
                                    consulta.Parameters.Add(new MySqlParameter("@salid", salaid));
                                    consulta.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Can't Create Zone {ex.Message}");
                        }
                    }
                }

            }
        }
        public bool DeleteEvent(Event e)
        {
            using(MySQLDBContext context = new MySQLDBContext())
            {
                using(var connection = context.Database.GetDbConnection())
                {
                    connection.Open();
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        throw new Exception("Can't Open Connection");
                    }
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var consulta = connection.CreateCommand())
                            {
                                consulta.Transaction = transaction;
                                consulta.CommandText = @"delete from event where Evt_Id = @id";
                                consulta.Parameters.Add(new MySqlParameter("@id", e.Id));
                                consulta.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Can't Delete Event {ex.Message}");
                        }
                    }
                }
            }
        }

        public bool DeleteSala(Sala s)
        {
            using (MySQLDBContext context = new MySQLDBContext())
            {
                using (var connection = context.Database.GetDbConnection())
                {
                    connection.Open();
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        throw new Exception("Can't Open Connection");
                    }
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var consulta = connection.CreateCommand())
                            {
                                consulta.Transaction = transaction;
                                consulta.CommandText = @"delete from sala where Sal_Id = @id";
                                consulta.Parameters.Add(new MySqlParameter("@id", s.Id));
                                consulta.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Can't Delete Sala {ex.Message}");
                        }
                    }
                }
            }
        }

        public ObservableCollection<Event> GetAllArtsEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

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
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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
                    }
                }

                return events;
            }
            catch (Exception e)
            {
               throw new Exception($"Can't Get All Arts Events {e.Message}");
              
            }
        }

        public ObservableCollection<Event> GetAllCinemaEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();


            try
            {

                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             where et.Type_Name = 'CINEMA'";
                            using (var reader = consulta.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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
                    }
                }

                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Cinema Events {e.Message}");

            }
        }

        public ObservableCollection<Event> GetAllEvents()
        {
           ObservableCollection<Event> events = new ObservableCollection<Event>();

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, et.Type_Name, s.Sal_Name, es.St_Name 
                             from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id
                             order by e.Evt_Id";
                            using (var reader = consulta.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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


                                    TipusEvent tipusEvent = (TipusEvent)Enum.Parse(typeof(TipusEvent), type, true);
                                    Estat estat1 = (Estat)Enum.Parse(typeof(Estat), estat, true);


                                    Event e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                                    events.Add(e);
                                }
                            }
                        }
                    }
                  
                }
                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Events {e.Message}");

            }
        }

        public ObservableCollection<Event> GetAllFamilyEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

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
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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
                    }
                }
                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Family Events {e.Message}");

            }
        }

        public ObservableCollection<Event> GetAllMusicEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

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
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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

                    }
                }


                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Music Events {e.Message}");

            }
        }

        public ObservableCollection<Event> GetAllOtherEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

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
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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
                    }
                }
                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Arts Events {e.Message}");

            }
        }

        public ObservableCollection<Event> GetAllSportsEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();


            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

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
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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
                    }
                }
                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Sports Events {e.Message}");

            }
        }

        public ObservableCollection<Event> GetAllTheatreEvent()
        {
            ObservableCollection<Event> events = new ObservableCollection<Event>();

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

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
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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
                    }
                }
                return events;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get All Theatre Events {e.Message}");

            }

        }

        public Event GetEventByName(string name)
        {
            Event e = null;
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using(var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }
                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"select e.Evt_Id, e.Evt_Name, e.Evt_Description, e.Evt_Performer, e.Evt_Image, e.Evt_Date, e.Evt_Time, 
                                    et.Type_Name, s.Sal_Name, es.St_Name
                                     from event e
                             join event_type et on e.Evt_Type_Id = et.Type_Id
                             join event_state es on e.Evt_Estat_Id = es.St_Id
                             join sala s on e.Evt_Sala_Id = s.Sal_Id

where Evt_Name like @name";
                            consulta.Parameters.Add(new MySqlParameter("@name", "%" + name + "%"));
                            using (var reader = consulta.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(reader.GetOrdinal("Evt_Id"));
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


                                     e = new Event(id, nom, desc, performer, imatge, data, time, tipusEvent, sala1, estat1);
                                   
                                }

                                
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Can't Get Requested Sala By Name : {name}. {ex.Message}");
            }

            return e;
        }

        public int GetSalaId(string name)
        {
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"select Sal_Id from sala where Sal_Name = @name";
                            consulta.Parameters.Add(new MySqlParameter("@name", name));
                            using (var reader = consulta.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return reader.GetInt32(reader.GetOrdinal("Sal_Id"));
                                }
                                else
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get Sala Id {e.Message}");
            }
        }

        public int GetStatusId(string name)
        {
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"select St_Id from EVENT_STATE where St_Name = @name";
                            consulta.Parameters.Add(new MySqlParameter("@name", name));
                            using (var reader = consulta.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return reader.GetInt32(reader.GetOrdinal("St_Id"));
                                }
                                else
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get Status Id {e.Message}");
            }
        }

        public int GetTipusId(string name)
        {
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"select Type_Id from EVENT_TYPE where Type_Name = @name";
                            consulta.Parameters.Add(new MySqlParameter("@name", name));
                            using (var reader = consulta.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return reader.GetInt32(reader.GetOrdinal("Type_Id"));
                                }
                                else
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get Type Id {e.Message}");
            }
        }

        public bool UpdateEvent(Event e)
        {
            bool ans = false;
            int id = e.Id;
            string name = e.Nom;
            string description = e.Desc;
            string performer = e.Protagonista;
            string image = e.ImatgePath;
            DateTime date = e.Data;
            TimeSpan time = e.Time;
            string type = e.Tipus.ToString();
            int typeID = GetTipusId(type);
            Sala sala = e.Sala;
            int salaId = GetSalaId(sala.Nom);
            string estat = e.Status.ToString();
            int estatID = GetStatusId(estat);

            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (var consulta = connection.CreateCommand())
                                {
                                    consulta.Transaction = transaction;
                                    consulta.CommandText = @"UPDATE Event 
                                                     SET Evt_Name = @name, 
                                                         Evt_Description = @description, 
                                                         Evt_Performer = @performer, 
                                                         Evt_Image = @image, 
                                                         Evt_Date = @date, 
                                                         Evt_Time = @time, 
                                                         Evt_Type_Id = @type, 
                                                         Evt_Sala_Id = @sala, 
                                                         Evt_Estat_Id = @estat
                                                     WHERE Evt_Id = @Id";
                                    consulta.Parameters.Add(new MySqlParameter("@name", name));
                                    consulta.Parameters.Add(new MySqlParameter("@description", description));
                                    consulta.Parameters.Add(new MySqlParameter("@performer", performer));
                                    consulta.Parameters.Add(new MySqlParameter("@image", image));
                                    consulta.Parameters.Add(new MySqlParameter("@date", date));
                                    consulta.Parameters.Add(new MySqlParameter("@time", time));
                                    consulta.Parameters.Add(new MySqlParameter("@type", typeID));
                                    consulta.Parameters.Add(new MySqlParameter("@sala", salaId));
                                    consulta.Parameters.Add(new MySqlParameter("@estat", estatID));
                                    consulta.Parameters.Add(new MySqlParameter("@Id", id));

                                    int rowsAffected = consulta.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        ans = true;
                                    }
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Can't Update Event: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't Update Selected Event: {ex.Message}");
            }

            return ans;
        }


        ObservableCollection<Sala> IRepository.GetAllSalas()
        {
           ObservableCollection<Sala> salas = new ObservableCollection<Sala>();

                try
                {
                    using (MySQLDBContext context = new MySQLDBContext()) {
                        using(var connection = context.Database.GetDbConnection())
                        {
                            connection.Open();

                            if(connection.State != System.Data.ConnectionState.Open)
                            {
                                throw new Exception("Can't Open Connection");
                            }

                                using (var consulta = connection.CreateCommand())
                                {
                                    consulta.CommandText = @"select Sal_Id, Sal_Name, Sal_Municipality, Sal_Address, Sal_MapAvail,
                                                Sal_Seats, Sal_Rows, Sal_Col from sala";
                                        using (var reader = consulta.ExecuteReader())
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
                        }
                    }


                    return salas;
                }
                    catch (Exception e)
                    {
                        throw new Exception($"Can't Get All Salas {e.Message}");

                    }
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


        public Boolean checkSalaZones(int salaid)
        {
            Boolean ans = false;
            int count = 0;
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"SELECT COUNT(Zone_Id) AS ZoneCount FROM zone WHERE Zone_Sal_Id = @SalaId";
                            consulta.Parameters.Add(new MySqlParameter("@SalaId", salaid));
                            using (var reader = consulta.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    count = reader.GetInt32(reader.GetOrdinal("ZoneCount"));
                                    if(count > 0)
                                    {
                                        ans = true;
                                    }
                                    else
                                    {
                                        ans = false;
                                    }
                                }
                        
                            }
                        }
                    }
                }
                return ans;

            }
            catch (Exception e)
            {
                throw new Exception($"Can't Get Sala Zone Id {e.Message}");
            }

        }

        public bool DeleteAllSalaZones(int salaid)
        {
            try
            {
                if (checkSalaZones(salaid))
                {

                    List<int> zoneIds = ZoneIds(salaid);




                    if (DeleteAllChairZones(zoneIds)) { 

                        using (MySQLDBContext context = new MySQLDBContext())
                        {

                            using (var connection = context.Database.GetDbConnection())
                            {
                                connection.Open();
                                if (connection.State != System.Data.ConnectionState.Open)
                                {
                                    throw new Exception("Can't Open Connection");
                                }

                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        using (var consulta = connection.CreateCommand())
                                        {
                                            consulta.Transaction = transaction;

                                            consulta.Parameters.Clear();
                                            consulta.CommandText = @"delete from zone where Zone_Sal_Id = @salid";
                                            consulta.Parameters.Add(new MySqlParameter("@salid", salaid));
                                            consulta.ExecuteNonQuery();

                                        }

                                        transaction.Commit();
                                        return true;
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        throw new Exception($"Can't Delete Zones {ex.Message}");
                                    }

                                }

                            }

                        }
                    }
                    return true;
                }
                else
                {
                    throw new Exception("Can't Delete Zones, Sala has no Zones");
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Can't Delete Zones {e.Message}");
            }
        }
        
        public bool DeleteAllChairZones(List<int> zoneIds)
        {
            bool ans = false;
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (var consulta = connection.CreateCommand())
                                {
                                    
                                    foreach (int zoneid in zoneIds)
                                    {
                                        consulta.Transaction = transaction;
                                        consulta.Parameters.Clear();
                                        consulta.CommandText = @"delete from seat where seat_ZoneId = @zoneid";
                                        consulta.Parameters.Add(new MySqlParameter("@zoneid", zoneid));
                                        consulta.ExecuteNonQuery();
                                    }


                                }
                                transaction.Commit();
                                ans = true;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Can't Delete Chairs {ex.Message}");
                            }
                        }
                    }
                }
                return ans;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't Delete Chairs {e.Message}");
            }
        }

         public  List<int> ZoneIds(int salaid)
         {
            var list = new List<int>();
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }
                        using (var consulta = connection.CreateCommand())
                        {
                            consulta.CommandText = @"SELECT Zone_Id FROM zone WHERE Zone_Sal_Id = @SalaId";
                            consulta.Parameters.Add(new MySqlParameter("@SalaId", salaid));
                            using (var reader = consulta.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    list.Add(reader.GetInt32(reader.GetOrdinal("Zone_Id")));
                                }
                            }
                        }
                    }
                }
                return list;
            }
                catch (Exception e)
                {
                    throw new Exception($"Can't Get Zone Ids {e.Message}");
                }
         }

        public bool CreateSeats(ObservableCollection<Zona> zones)
        {
            try
            {
                using (MySQLDBContext context = new MySQLDBContext())
                {
                    using (var connection = context.Database.GetDbConnection())
                    {
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            throw new Exception("Can't Open Connection");
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                using (var command = connection.CreateCommand())
                                {
                                    command.Transaction = transaction;

                                    foreach (var zone in zones)
                                    {
                                        // Get the Zone_Id for this zone - assuming it was just created
                                        command.Parameters.Clear();
                                        command.CommandText = "SELECT Zone_Id FROM zone WHERE Zone_Name = @zoneName AND Zone_Color = @zoneColor";
                                        command.Parameters.Add(new MySqlParameter("@zoneName", zone.Nom));
                                        command.Parameters.Add(new MySqlParameter("@zoneColor", ConvertToHex(zone.Z_Color)));

                                        int zoneId;
                                        using (var reader = command.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                zoneId = reader.GetInt32(0);
                                            }
                                            else
                                            {
                                                throw new Exception($"Zone {zone.Nom} not found in database");
                                            }
                                        }

                                        // Insert each seat for this zone
                                        foreach (var seat in zone.Cadires)
                                        {
                                            command.Parameters.Clear();
                                            command.CommandText = @"
                                        INSERT INTO seat (Seat_Description, Seat_X, Seat_Y, Seat_ZoneId)
                                        VALUES (@description, @x, @y, @zoneId)";

                                            string seatDescription = $"{(char)('A' + seat.Y)}{seat.X + 1}"; // e.g., A1, B2, etc.

                                            command.Parameters.Add(new MySqlParameter("@description", seatDescription));
                                            command.Parameters.Add(new MySqlParameter("@x", seat.X));
                                            command.Parameters.Add(new MySqlParameter("@y", seat.Y));
                                            command.Parameters.Add(new MySqlParameter("@zoneId", zoneId));

                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }

                                transaction.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Error creating seats: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in CreateSeats: {ex.Message}");
            }
        }
    }
}

