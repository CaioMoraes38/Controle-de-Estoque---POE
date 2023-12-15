using Estoque.App.Base;
using Estoque.App.Models;
using Estoque.Domain.Base;
using Estoque.Domain.Entities;
using Estoque.Service.Validators;

namespace Estoque.App.Cadastros
{
    public partial class CadastroFornecedor : CadastroBase
    {
        private readonly IBaseService<Fornecedor> _fornecedorService;

        private List<FornecedorModel>? fornecedor;

        public CadastroFornecedor(IBaseService<Fornecedor> fornecedorService)
        {
            _fornecedorService = fornecedorService;

            InitializeComponent();
        }



        private void PreencheObjeto(Fornecedor fornecedor)
        {
            fornecedor.Nome = txtNome.Text;
            fornecedor.Endereco = txtEndereco.Text;
            fornecedor.Bairro = txtBairro.Text;
            fornecedor.Cidade = txtCidade.Text;



        }

        protected override void Salvar()
        {
            try
            {
                if (IsAlteracao)
                {
                    if (int.TryParse(txtId.Text, out var id))
                    {
                        var fornecedor = _fornecedorService.GetById<Fornecedor>(id);
                        PreencheObjeto(fornecedor);
                        _fornecedorService.Update<Fornecedor, Fornecedor, FornecedorValidator>(fornecedor);
                    }
                }
                else
                {
                    var fornecedor = new Fornecedor();
                    PreencheObjeto(fornecedor);
                    _fornecedorService.Add<Fornecedor, Fornecedor, FornecedorValidator>(fornecedor);

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
                _fornecedorService.Delete(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Estoque", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void CarregaGrid()
        {
            // fornecedor = _fornecedorService.Get<FornecedorModel>(new[] { "Cidade" }).ToList();
            fornecedor = _fornecedorService.Get<FornecedorModel>().ToList();
            dataGridViewConsulta.DataSource = fornecedor;
            dataGridViewConsulta.Columns["Nome"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
          
        }

        protected override void CarregaRegistro(DataGridViewRow? linha)
        {
            txtId.Text = linha?.Cells["Id"].Value.ToString();
            txtNome.Text = linha?.Cells["Nome"].Value.ToString();
            txtEndereco.Text = linha?.Cells["Endereco"].Value.ToString();
            txtBairro.Text = linha?.Cells["Bairro"].Value.ToString();
            txtCidade.Text = linha?.Cells["Cidade"].Value?.ToString();
        }

    }
}