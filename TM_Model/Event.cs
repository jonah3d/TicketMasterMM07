﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TM_Model
{
    public class Event
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Protagonista { get; set; }
        public string ImatgePath { get; set; }


        public string Desc { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Time { get; set; }
        public List<Tarifa> Tarifes { get; set; }
        public Sala Sala { get; set; }
        public Estat Status { get; set; }
        public TipusEvent Tipus { get; set; }

        public Event() { }

        public Event(int id, string nom, string desc, string protagonista, string imatgePath,
                    DateTime data, TimeSpan time, TipusEvent tipusEvent, Sala sala, Estat estat)
        {
            Id = id;
            Nom = nom;
            Protagonista = protagonista;
            ImatgePath = imatgePath;
            Desc = desc;
            Data = data;
            Time = time;
            Tipus = tipusEvent;
            Sala = sala;
            Status = estat;
            Tarifes = new List<Tarifa>();
        }

        public Event(string name, string description, DateTime date, TimeSpan time, TipusEvent eventType, Estat status, string performer, Sala sala, string imagePath)
        {
            Nom = name;
            Desc = description;
            Data = date;
            Time = time;
            Tipus = eventType;
            Status = status;
            Protagonista = performer;
            Sala = sala;
            ImatgePath = imagePath;
      
        }

        public Event(int id,string name, string description, DateTime date, TimeSpan time, TipusEvent eventType, Estat status, string performer, Sala sala, string imagePath)
        {
            Id = id;
            Nom = name;
            Desc = description;
            Data = date;
            Time = time;
            Tipus = eventType;
            Status = status;
            Protagonista = performer;
            Sala = sala;
            ImatgePath = imagePath;

        }
    }
}