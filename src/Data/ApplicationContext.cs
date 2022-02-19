using EFCore.Tips.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Tips.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-B76722G\\SQLEXPRESS; Database=Tips; User ID=developer; Password=dev*10; Integrated Security=True;")
                          .EnableSensitiveDataLogging();
        }
    }
}
