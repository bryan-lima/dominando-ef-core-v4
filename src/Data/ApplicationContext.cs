using EFCore.Tips.Domain;
using EFCore.Tips.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
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
        public DbSet<UsuarioFuncao> UsuarioFuncoes { get; set; }
        public DbSet<DepartamentoRelatorio> DepartamentoRelatorio { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-B76722G\\SQLEXPRESS; Database=Tips; User ID=developer; Password=dev*10; Integrated Security=True;")
                          .LogTo(Console.WriteLine, LogLevel.Information)
                          .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UsuarioFuncao>()
            //            .HasNoKey();

            modelBuilder.Entity<DepartamentoRelatorio>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_departamento_relatorio");
                entity.Property(departamentoRelatorio => departamentoRelatorio.Departamento)
                      .HasColumnName("Descricao");
            });

            IEnumerable<IMutableProperty> properties = modelBuilder.Model.GetEntityTypes()
                                                                         .SelectMany(type => type.GetProperties())
                                                                         .Where(property => property.ClrType == typeof(string) && property.GetColumnType() == null);

            foreach (var property in properties)
            {
                property.SetIsUnicode(false);
            }

            modelBuilder.ToSnakeCaseNames();
        }
    }
}
