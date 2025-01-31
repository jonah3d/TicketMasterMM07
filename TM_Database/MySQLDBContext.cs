using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

using TM_Model;


namespace TM_Database
{
    public class MySQLDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "Server=10.2.227.118;Database=ticketmaster;User=jonah3d;Password=jonah3d;"

            );
        }

       
    }
}