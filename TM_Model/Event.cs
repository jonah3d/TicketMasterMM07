using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM_Model
{
    public class Event
    {
        private TipusEvent tipus;
        private long id;
        private String nom;
        private String protagonista;
        private string imatgePath;
        private string desc;
        private DateTime data;
        private DateTime time;
        private List<Tarifa> tarifes;
        private Sala sala;
        private Estat estat;
        private string location;
    }
}
