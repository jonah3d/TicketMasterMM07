using System;
using TM_Database.Repository;

namespace TM_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
           IEventRepository eventRepository = new EventRepository();
            Console.WriteLine("Music Events!");

            eventRepository.GetAllMusicEvent();
            for (int i = 0; i < eventRepository.GetAllMusicEvent().Count; i++)
            {
                Console.WriteLine(eventRepository.GetAllMusicEvent()[i].Nom);
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Theatre Events!");
            foreach (var ev in eventRepository.GetAllTheatreEvent())
            {
                Console.WriteLine(ev.Nom);
            }


            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Theatre Events!");
            foreach (var ev in eventRepository.GetAllSportsEvent())
            {
                Console.WriteLine(ev.Nom);
            }

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Arts Events!");
            foreach (var ev in eventRepository.GetAllArtsEvent())
            {
                Console.WriteLine(ev.Nom);
            }

        }
    }
}
