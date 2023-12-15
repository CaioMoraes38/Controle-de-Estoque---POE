using Estoque.App.Base;
using Estoque.App.Models;
using Estoque.Domain.Base;
using Estoque.Domain.Entities;
using Estoque.Service.Validators;

namespace Estoque.App.Cadastros
{
    public partial class CadastroProduto : CadastroBase
    {
        private readonly IBaseService<Produto> _produtoService;
        private readonly IBaseService<Categoria> _categoriaService;

        private List<ProdutoModel>? produtos;

        public CadastroProduto(IBaseService<Produto> produtoService, IBaseService<Categoria> categoriaService)
        {
            _produtoService = produtoService;
            _categoriaService = categoriaService;
            InitializeComponent();
            CarregarCombo();
        }

        private void CarregarCombo()
        {
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Nome";
            cboCategoria.DataSource = _categoriaService.Get<Categoria>().ToList();
        }

        private void PreencheObjeto(Produto produto)
        {
            produto.Nome = txtNome.Text;
            if (float.TryParse(txtPreco.Text, out var preco))
            {
                produto.Preco = preco;
            }

            if (DateTime.TryParse(txtDataCompra.Text, out var dataCompra))
            {
                produto.DataCompra = dataCompra;
            }
            produto.UnidadeVenda = txtUnidadeVenda.Text;

            if (int.TryParse(cboCategoria.SelectedValue.ToString(), out var idCategoria))
            {
                var categoria = _categoriaService.GetById<Categoria>(idCategoria);
                produto.Categoria = categoria;
                _produtoService.AttachObject(categoria);


            }
        }

        protected override void Salvar()
        {
            try
            {
                if (IsAlteracao)
                {
                    if (int.TryParse(txtId.Text, out var id))
                    {
                        var produto = _produtoService.GetById<Produto>(id);
                        PreencheObjeto(produto);
                        produto = _produtoService.Update<Produto, Produto, ProdutoValidator>(produto);
                    }
                }
                else
                {
                    var produto = new Produto();
                    PreencheObjeto(produto);
                    _produtoService.Add<Produto, Produto, ProdutoValidator>(produto);

                }

                materialTabControl.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Estoque", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Deletar(int id)
        {
            try
            {
                _produtoService.Delete(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Estoque", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void CarregaGrid()
        {
            produtos = _produtoService.Get<ProdutoModel>(new[] { "Categoria" }).ToList();
            dataGridViewConsulta.DataSource = produtos;
            dataGridViewConsulta.Columns["IdCategoria"]!.Visible = false;
        }

        protected override void CarregaRegistro(DataGridViewRow? linha)
        {
            txtId.Text = linha?.Cells["Id"].Value.ToString();
            txtNome.Text = linha?.Cells["Nome"].Value.ToString();
            txtUnidadeVenda.Text = linha?.Cells["UnidadeVenda"].Value.ToString();
            txtPreco.Text = linha?.Cells["Preco"].Value.ToString();
            cboCategoria.SelectedValue = linha?.Cells["IdCategoria"].Value;
            txtDataCompra.Text = DateTime.TryParse(linha?.Cells["DataCompra"].Value.ToString(), out var dataC)
               ? dataC.ToString("g")
               : "";

        }

    }
}
