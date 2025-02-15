using System;
using System.Collections.Generic;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM_Model    
{
    public class Zona
    {
       
        public long Id { get; set; }
        public string Nom { get; set; }
        public string Desc { get; set; }
        public List<Cadira> Cadires { get; set; }
        public int Capacitat { get; set; }
        public  Color Z_Color { get; set; }
     

        public Zona(string nom, int capacitat, Color color)
        {
            this.Nom = nom;
            this.Capacitat = capacitat;
            this.Z_Color = color;
            this.Cadires = new List<Cadira>();
        }

        public Zona (long id, string nom, int capacitat, Color color)
        {
            this.Id = id;
            this.Nom = nom;
          
            this.Capacitat = capacitat;
            this.Z_Color = color;
            this.Cadires = new List<Cadira>();
        }
    }
}
