using Microsoft.EntityFrameworkCore;
using ProjetoEscala.Models;

namespace ProjetoEscala.Context
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {        }
        
        public DbSet<Escala> Escala {get; set;}
        public DbSet<Evento> Evento {get;set;}
        public DbSet<Local> Local {get;set;}
        public DbSet<Pessoa> Pessoa {get; set;}
        public DbSet<PessoaQuadro> PessoaQuadro {get;set;}
        public DbSet<Quadro> Quadro {get;set;}

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(@"Data Source=E:\SqLite\SQLiteDatabaseBrowserPortable\Data\Developer.db;");
        }*/
    }
}