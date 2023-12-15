using Estoque.App.Base;
using Estoque.Domain.Base;
using Estoque.Domain.Entities;
using Estoque.Service.Validators;

namespace Estoque.App.Cadastros
{
    public partial class CadastroCategoria : CadastroBase
    {
        private readonly IBaseService<Categoria> _categoriaService;

        private List<Categoria>? categoria;

        public CadastroCategoria(IBaseService<Categoria> categoriaService)
        {
            _categoriaService = categoriaService;
            InitializeComponent();
        }

        private void PreencheObjeto(Categoria Categoria)
        {
            Categoria.Nome = txtNome.Text;
        }

        protected override void Salvar()
        {
            try
            {
                if (IsAlteracao)
                {
                    if (int.TryParse(txtId.Text, out var id))
                    {
                        var categoria = _categoriaService.GetById<Categoria>(id);
                        PreencheObjeto(categoria);
                        categoria = _categoriaService.Update<Categoria, Categoria, CategoriaValidator>(categoria);
                    }
                }
                else
                {
                    var categoria = new Categoria();
                    PreencheObjeto(categoria);
                    _categoriaService.Add<Categoria, Categoria, CategoriaValidator>(categoria);

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
                _categoriaService.Delete(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Estoque", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        protected override void CarregaGrid()
        {
            categoria = _categoriaService.Get<Categoria>().ToList();
            dataGridViewConsulta.DataSource = categoria;
            dataGridViewConsulta.Columns["Nome"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        protected override void CarregaRegistro(DataGridViewRow? linha)
        {
            txtId.Text = linha?.Cells["Id"].Value.ToString();
            txtNome.Text = linha?.Cells["Nome"].Value.ToString();
        }

    }
}
