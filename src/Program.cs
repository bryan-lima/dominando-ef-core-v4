using EFCore.Tips.Data;
using EFCore.Tips.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            //SingleOrDefaultVsFirstOrDefault();

            //SemChavePrimaria();

            //ToView();

            //NaoUnicode();

            //OperadoresDeAgregacao();

            OperadoresDeAgregacaoNoAgrupamento();
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

        static void SemChavePrimaria()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            UsuarioFuncao[] usuarioFuncoes = db.UsuarioFuncoes.Where(usuarioFuncao => usuarioFuncao.UsuarioId == Guid.NewGuid())
                                                              .ToArray();
        }

        static void ToView()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Database.ExecuteSqlRaw(
                @"CREATE VIEW vw_departamento_relatorio AS
                SELECT
                    d.Descricao, count(c.Id) AS Colaboradores
                FROM Departamentos d
                LEFT JOIN Colaboradores c ON c.DepartamentoId = d.Id
                GROUP BY d.Descricao");

            IEnumerable<Departamento> _departamentos = Enumerable.Range(1, 10)
                                                                 .Select(numeroDepartamento => new Departamento
                                                                 {
                                                                     Descricao = $"Departamento {numeroDepartamento}",
                                                                     Colaboradores = Enumerable.Range(1, numeroDepartamento)
                                                                                               .Select(numeroColaborador => new Colaborador 
                                                                                               {
                                                                                                   Nome = $"Colaborador {numeroDepartamento}-{numeroColaborador}"
                                                                                               }).ToList()
                                                                 });

            Departamento _departamento = new Departamento
            {
                Descricao = "Departamento Sem Colaborador"
            };

            db.Departamentos.Add(_departamento);
            db.Departamentos.AddRange(_departamentos);
            db.SaveChanges();

            List<DepartamentoRelatorio> relatorio = db.DepartamentoRelatorio.Where(departamentoRelatorio => departamentoRelatorio.Colaboradores < 20)
                                                                            .OrderBy(departamentoRelatorio => departamentoRelatorio.Departamento)
                                                                            .ToList();

            foreach (DepartamentoRelatorio departamento in relatorio)
            {
                Console.WriteLine($"{departamento.Departamento} [ Colaboradores: {departamento.Colaboradores}]");
            }
        }

        static void NaoUnicode()
        {
            using ApplicationContext db = new ApplicationContext();

            string sql = db.Database.GenerateCreateScript();

            Console.WriteLine(sql);
        }

        static void OperadoresDeAgregacao()
        {
            using ApplicationContext db = new ApplicationContext();

            var sql = db.Departamentos.GroupBy(departamento => departamento.Descricao)
                                      .Select(departamento => new 
            {
                Descricao = departamento.Key,
                Contador = departamento.Count(),
                Media = departamento.Average(departamento => departamento.Id),
                Maximo = departamento.Max(departamento => departamento.Id),
                Soma = departamento.Sum(departamento => departamento.Id)
            }).ToQueryString();

            Console.WriteLine(sql);
        }

        static void OperadoresDeAgregacaoNoAgrupamento()
        {
            using ApplicationContext db = new ApplicationContext();

            var sql = db.Departamentos.GroupBy(departamento => departamento.Descricao)
                                      .Where(departamento => departamento.Count() > 1)
                                      .Select(departamento => new
                                      {
                                          Descricao = departamento.Key,
                                          Contador = departamento.Count()
                                      }).ToQueryString();

            Console.WriteLine(sql);
        }
    }
}
