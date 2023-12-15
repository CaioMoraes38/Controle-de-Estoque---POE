using System.Text.Json.Serialization;
using Estoque.Domain.Base;

namespace Estoque.Domain.Entities
{
    public class Venda : BaseEntity<int>
    {
        public Venda()
        {
            Items = new List<VendaItem>();
        }
        
        public Venda(int id, DateTime data, float valorTotal, Usuario? usuario, Fornecedor? fornecedor, List<VendaItem> items) : base(id)
        {
            Data = data;
            ValorTotal = valorTotal;
            Usuario = usuario;
            Fornecedor = fornecedor;
            Items = items;
        }

        public DateTime Data { get; set; }
        public float ValorTotal { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual Fornecedor? Fornecedor { get; set; }
        public virtual List<VendaItem> Items { get; set; }
    }

    public class VendaItem : BaseEntity<int>
    {
        public VendaItem()
        {
            
        }

        public VendaItem(int id, Produto? produto, int quantidade, float valorUnitario, float valorTotal, Venda? venda) : base(id)
        {
            Produto = produto;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
            Venda = venda;
        }

        public virtual Produto? Produto { get; set; }
        public int Quantidade { get; set; }
        public float ValorUnitario { get; set; }    
        public float ValorTotal { get; set; }
        [JsonIgnore]
        public virtual Venda? Venda { get; set; }
    }
}
