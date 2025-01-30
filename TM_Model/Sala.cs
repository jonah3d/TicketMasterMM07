using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM_Model
{
    public class Sala
    {
        private long id;
        private String nom;
        private string municipi;
        private string adreca;
        private List<Zona> zones;
        private bool teMapa;
        private int numFiles;
        private int numColumnes;

        public Sala(string nom)
        {
            this.nom = nom;
        }
    }

  
    }
