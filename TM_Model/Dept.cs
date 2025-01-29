using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM_Model
{
    public class Dept : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        public Dept(int deptNo, string dName, string loc)
        {
            DeptNo = deptNo;
            DName = dName;
            Loc = loc;
        }

        public int DeptNo { get; set; }
        public string DName { get; set; }
        public string Loc { get; set; }

    }
}
