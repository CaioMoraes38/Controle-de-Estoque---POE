using Estoque.Domain.Base;

namespace Estoque.Domain.Entities
{
    public class Fornecedor : BaseEntity<int>
    {
        public Fornecedor()
        {
            
        }

        public Fornecedor(int id, string? nome, string? endereco, string? bairro,string? cidade) : base(id)
        {
            Nome = nome;
            Endereco = endereco;
            Bairro = bairro;
            Cidade = cidade;
        }

        public string? Nome { get; set; }
        public string? Endereco { get; set; }
        public string? Bairro { get; set; }
        public string?Cidade { get; set; }
    }
}
