using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TM_Model;

namespace TM_Database.Repository
{
    public interface IEventRepository
    {

        ObservableCollection<Event> GetAllEvents();
        ObservableCollection<Event> GetAllMusicEvent();
    }
}
