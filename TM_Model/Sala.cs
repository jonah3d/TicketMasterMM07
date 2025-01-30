using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM_Model
{
    public class Sala
    {
        public long Id { get; set; }
        public string Nom { get; set; }
        public string Municipi { get; set; }
        public string Adreca { get; set; }
        public List<Zona> Zones { get; set; }
        public int Seats { get; set; }
        public bool TeMapa { get; set; }
        public int NumFiles { get; set; }
        public int NumColumnes { get; set; }

        public Sala(string nom)
        {
            this.Nom = nom;
        }

        public Sala() { }

        public Sala(long id, string nom, string municipi, string adreca, List<Zona> zones, bool teMapa, int numFiles, int numColumnes)
        {
            this.Id = id;
            this.Nom = nom;
            this.Municipi = municipi;
            this.Adreca = adreca;
            this.Zones = zones;
            this.TeMapa = teMapa;
            this.NumFiles = numFiles;
            this.NumColumnes = numColumnes;
        }

        public Sala(long id, string nom, string municipi, string adreca, bool teMapa,int seats, int numFiles, int numColumnes)
        {
            this.Id = id;
            this.Nom = nom;
            this.Municipi = municipi;
            this.Adreca = adreca;
            this.TeMapa = teMapa;
            this.NumFiles = numFiles;
            this.NumColumnes = numColumnes;
            this.Seats = seats;
        }
    }

  
    }
