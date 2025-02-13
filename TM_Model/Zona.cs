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
        private long id;
        private string nom;
        private string desc;
        private List<Cadira> cadires;
        private int capacitat;
        public  Color color { get; set; }

        public Zona(string nom, int capacitat, Color color)
        {
            this.nom = nom;
            this.capacitat = capacitat;
            this.color = color;
        }
    }


}
