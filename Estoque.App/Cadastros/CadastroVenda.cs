using Estoque.App.Base;
using Estoque.App.Models;
using Estoque.Domain.Base;
using Estoque.Domain.Entities;
using Estoque.Service.Validators;
using System.Globalization;

namespace Estoque.App.Cadastros
{
    public partial class CadastroVenda : CadastroBase
    {
        private List<VendaItemModel> _vendaItems;
        private readonly IBaseService<Venda> _vendaService;
        private readonly IBaseService<Usuario> _usuarioService;
        private readonly IBaseService<Fornecedor> _fornecedorService;
        private readonly IBaseService<Produto> _produtoService;

        private List<VendaModel>? vendas;

        public CadastroVenda(IBaseService<Venda> vendaService,
                             IBaseService<Usuario> usuarioService,
                             IBaseService<Fornecedor> fornecedorService,
                             IBaseService<Produto> produtoService)

        {
            _vendaService = vendaService;
            _usuarioService = usuarioService;
            _fornecedorService = fornecedorService;
            _produtoService = produtoService;
            _vendaItems = new List<VendaItemModel>();
            InitializeComponent();
            CarregarCombo();
            CarregaGridItensVenda();
            Novo();
        }

        private void CarregarCombo()
        {
            cboUsuario.ValueMember = "Id";
            cboUsuario.DisplayMember = "Nome";
            cboUsuario.DataSource = _usuarioService.Get<Usuario>().ToList();

            cboFornecedor.ValueMember = "Id";
            cboFornecedor.DisplayMember = "Nome";
            cboFornecedor.DataSource = _fornecedorService.Get<Fornecedor>().ToList();

            cboProduto.ValueMember = "Id";
            cboProduto.DisplayMember = "Nome";
            cboProduto.DataSource = _produtoService.Get<Produto>().ToList();
        }

        private void PreencheObjeto(Venda venda)
        {
            if (DateTime.TryParse(txtDataVenda.Text, out var dataVenda))
            {
                venda.Data = dataVenda;
            }

            if (int.TryParse(cboUsuario.SelectedValue.ToString(), out var idUsuario))
            {
                var usuario = _usuarioService.GetById<Usuario>(idUsuario);
                venda.Usuario = usuario;
            }

            if (int.TryParse(cboFornecedor.SelectedValue.ToString(), out var idFornecedor))
            {
                var fornecedor = _fornecedorService.GetById<Fornecedor>(idFornecedor);
                venda.Fornecedor = fornecedor;
            }
            venda.ValorTotal = _vendaItems.Sum(x => x.ValorTotal);

            foreach (var item in _vendaItems)
            {
                var itemVenda = new VendaItem
                {
                    Venda = venda,
                    Produto = _produtoService.GetById<Produto>(item.IdProduto),
                    ValorUnitario = item.ValorUnitario,
                    Quantidade = item.Quantidade,
                    ValorTotal = item.ValorTotal
                };

                venda.Items.Add(itemVenda);
            }

        }

        protected override void Novo()
        {
            base.Novo();
            _vendaItems.Clear();
            CarregaGridItensVenda();
            txtDataVenda.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected override void Salvar()
        {
            try
            {
                if (IsAlteracao)
                {
                    if (int.TryParse(txtId.Text, out var id))
                    {
                        var venda = _vendaService.GetById<Venda>(id);
                        PreencheObjeto(venda);
                        venda = _vendaService.Update<Venda, Venda, VendaValidator>(venda);
                    }
                }
                else
                {
                    var venda = new Venda();
                    PreencheObjeto(venda);
                    venda = _vendaService.Add<Venda, Venda, VendaValidator>(venda);
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
                _vendaService.Delete(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Estoque", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        protected override void CarregaGrid()
        {
            var includes = new List<string>() { "Fornecedor", "Usuario" };
            vendas = _vendaService.Get<VendaModel>(includes).ToList();
            dataGridViewConsulta.DataSource = vendas;
            dataGridViewConsulta.Columns["IdUsuario"]!.Visible = false;
            dataGridViewConsulta.Columns["IdFornecedor"]!.Visible = false;
            dataGridViewConsulta.Columns["ValorTotal"].DefaultCellStyle.Format = "C2";
            dataGridViewConsulta.Columns["ValorTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        protected override void CarregaRegistro(DataGridViewRow? linha)
        {
            int.TryParse(linha?.Cells["Id"].Value.ToString(), out var id);
            txtId.Text = linha?.Cells["Id"].Value.ToString();
            cboUsuario.SelectedValue = linha?.Cells["IdUsuario"].Value;
            cboFornecedor.SelectedValue = linha?.Cells["IdFornecedor"].Value;
            txtDataVenda.Text = DateTime.TryParse(linha?.Cells["Data"].Value.ToString(), out var dataC)
               ? dataC.ToString("g")
               : "";

            var includes = new List<string>() { "Fornecedor", "Usuario", "Items", "Items.Produto" };
            var venda = _vendaService.GetById<Venda>(id, includes);
            _vendaItems = new List<VendaItemModel>();
            foreach (var item in venda.Items)
            {
                var vendaItem = new VendaItemModel
                {
                    Id = item.Id,
                    IdProduto = item.Produto!.Id,
                    Produto = item.Produto!.Nome,
                    ValorTotal = item.ValorTotal,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario
                };
                _vendaItems.Add(vendaItem);
            }
            CarregaGridItensVenda();

        }

        private void CarregaGridItensVenda()
        {
            var source = new BindingSource();
            source.DataSource = _vendaItems.ToArray();
            dataGridViewItens.DataSource = source;
            dataGridViewItens.Columns["Id"]!.Visible = false;
            dataGridViewItens.Columns["IdProduto"]!.HeaderText = @"Id.Produto";
            dataGridViewItens.Columns["ValorUnitario"]!.DefaultCellStyle.Format = "C2";
            dataGridViewItens.Columns["ValorUnitario"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewItens.Columns["ValorTotal"]!.DefaultCellStyle.Format = "C2";
            dataGridViewItens.Columns["ValorTotal"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewItens.Columns["Quantidade"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (ValidaItem())
            {
                var vendaItem = new VendaItemModel();
                if (int.TryParse(cboProduto.SelectedValue.ToString(), out var idProduto))
                {
                    var produto = _produtoService.GetById<Produto>(idProduto);
                    vendaItem.IdProduto = produto.Id;
                    vendaItem.Produto = produto.Nome;
                }

                if (float.TryParse(txtVlUnitario.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out var vlUnitario))
                {
                    vendaItem.ValorUnitario = vlUnitario;
                }
                if (int.TryParse(txtQuantidade.Text, out var qtd))
                {
                    vendaItem.Quantidade = qtd;
                }

                vendaItem.ValorTotal = vendaItem.Quantidade * vendaItem.ValorUnitario;

                _vendaItems.Add(vendaItem);
                CalculaTotalVenda();
                CarregaGridItensVenda();
            }
        }

        private bool ValidaItem()
        {
            return true;
        }

        private void txtVlUnitario_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(txtVlUnitario.Text, out double value))
                txtVlUnitario.Text = string.Format(CultureInfo.CurrentCulture, @"{0:C2}", value);
            else
                txtVlUnitario.Text = string.Empty;

            CalculaTotalItem();
        }

        private void CalculaTotalItem()
        {
            var convVlr = float.TryParse(txtVlUnitario.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out float vlUnitario);
            var convQtd = int.TryParse(txtQuantidade.Text, out int quantidade);
            if (convVlr && convQtd)
            {
                var valorTotal = quantidade * vlUnitario;
                txtVlTotal.Text = string.Format(CultureInfo.CurrentCulture, "{0:C2}", valorTotal);
            }
        }

        private void CalculaTotalVenda()
        {
            lblValor.Text = $@"Valor Total: {string.Format(CultureInfo.CurrentCulture, "{0:C2}", _vendaItems.Sum(x => x.ValorTotal))}";
            lblQtdItens.Text = $@"Qtd. Produtos: {_vendaItems.Sum(x => x.Quantidade)}";
        }

        private void txtQuantidade_Leave(object sender, EventArgs e)
        {
            CalculaTotalItem();
        }
    }
}
