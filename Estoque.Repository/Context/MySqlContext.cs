using Estoque.Domain.Entities;
using Estoque.Repository.Mapping;
using Microsoft.EntityFrameworkCore;


namespace Estoque.Repository.Context
{
    public sealed class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
            Database.EnsureCreated();
            ChangeTracker.LazyLoadingEnabled = false;
            
        }

        public DbSet<Usuario>? Usuario { get; set; }
        public DbSet<Fornecedor>? Fornecedor { get; set; }
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Produto>? Produto { get; set; }
        public DbSet<Venda>? Venda { get; set; }
        public DbSet<VendaItem>? VendaItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);   
            modelBuilder.Entity<Fornecedor>(new FornecedorMap().Configure);
            modelBuilder.Entity<Categoria>(new CategoriaMap().Configure);
            modelBuilder.Entity<Produto>(new ProdutoMap().Configure);
            modelBuilder.Entity<Venda>(new VendaMap().Configure);
            modelBuilder.Entity<VendaItem>(new VendaItemMap().Configure);
        }
    }
}
