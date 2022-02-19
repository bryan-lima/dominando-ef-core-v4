using EFCore.Tips.Data;
using EFCore.Tips.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCore.Tips
{
    class Program
    {
        static void Main(string[] args)
        {
            //ToQueryString();

            //DebugView();

            //Clear();

            //ConsultaFiltrada();

            SingleOrDefaultVsFirstOrDefault();
        }

        static void ToQueryString()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureCreated();

            IQueryable<Departamento> query = db.Departamentos.Where(departamento => departamento.Id > 2);

            string sql = query.ToQueryString();

            Console.WriteLine(sql);
        }

        static void DebugView()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

            IQueryable<Departamento> query = db.Departamentos.Where(departamento => departamento.Id > 2);
        }

        static void Clear()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

            db.ChangeTracker.Clear();
        }

        static void ConsultaFiltrada()
        {
            using ApplicationContext db = new ApplicationContext();

            string sql = db.Departamentos.Include(departamento => departamento.Colaboradores.Where(colaborador => colaborador.Nome.Contains("Teste")))
                                         .ToQueryString();

            Console.WriteLine(sql);
        }

        static void SingleOrDefaultVsFirstOrDefault()
        {
            using ApplicationContext db = new ApplicationContext();

            Console.WriteLine("SingleOrDefault:");
            _ = db.Departamentos.SingleOrDefault(departamento => departamento.Id > 2);

            Console.WriteLine("FirstOrDefault:");
            _ = db.Departamentos.FirstOrDefault(departamento => departamento.Id > 2);
        }
    }
}
