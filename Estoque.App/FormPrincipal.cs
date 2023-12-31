using Estoque.App.Cadastros;
using Estoque.App.Infra;
using Estoque.App.Outros;
using Estoque.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using System.Security.Policy;

namespace Estoque.App
{
    public partial class FormPrincipal : MaterialForm
    {

        public static Usuario? Usuario { get; set; }
        public FormPrincipal()
        {
            InitializeComponent();
            CarregaLogin();
        }

        private void CarregaLogin()
        {
            var login = ConfigureDI.ServicesProvider!.GetService<Login>();
            if (login != null && !login.IsDisposed)
            {
                if (login.ShowDialog() != DialogResult.OK)
                {
                    Environment.Exit(0);
                }
                else
                {
                    lblUsuario.Text = $"Usu�rio: {Usuario.Nome}";
                }
            }
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
            }
        }

        private void usu�riosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exibeformulario<CadastroUsuario>();
        }

        private void categoriaDeProdutosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exibeformulario<CadastroCategoria>();
        }

        private void produtosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exibeformulario<CadastroProduto>();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exibeformulario<CadastroFornecedor>();
        }

        private void vendaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exibeformulario<CadastroVenda>();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void Exibeformulario<TFormlario>() where TFormlario : Form
        {
            var cad = ConfigureDI.ServicesProvider!.GetService<TFormlario>();
            if (cad != null && !cad.IsDisposed)
            {
                cad.MdiParent = this;
                cad.Show();
            }
        }
    }
}