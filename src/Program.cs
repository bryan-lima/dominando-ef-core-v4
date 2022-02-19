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
            ToQueryString();
        }

        static void ToQueryString()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureCreated();

            IQueryable<Departamento> query = db.Departamentos.Where(departamento => departamento.Id > 2);

            string sql = query.ToQueryString();

            Console.WriteLine(sql);
        }
    }
}
