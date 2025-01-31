using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TM_Model
{
    public class Event
    {
        public long Id { get; set; }
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

        public Event(long id, string nom, string desc, string protagonista, string imatgePath,
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
    }
}