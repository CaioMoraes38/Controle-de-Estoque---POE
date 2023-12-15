using Estoque.Domain.Base;

namespace Estoque.Domain.Entities
{
    public class Categoria : BaseEntity<int>
    {
        public Categoria()
        {
            
        }

        public Categoria(int id, string? nome) : base(id)
        {
            Nome = nome;
        }

        public string? Nome { get; set; } 
    }
}
