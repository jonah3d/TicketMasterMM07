using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TM_Model;

namespace TM_Database.Repository
{
    public interface IEventRepository
    {

        List<Event> GetAllEventsAsync();
        List<Event> GetAllMusicEventAsync();
    }
}
