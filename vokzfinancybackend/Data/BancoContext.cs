using Microsoft.EntityFrameworkCore;
using VokzFinancy.Models;

namespace VokzFinancy.Data {

    public class BancoContext : DbContext {

        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {}
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Despesa> Despesas {get; set;}
        public DbSet<Receita> Receitas {get; set;}
        public DbSet<Conta> Contas {get; set;}

    }

}