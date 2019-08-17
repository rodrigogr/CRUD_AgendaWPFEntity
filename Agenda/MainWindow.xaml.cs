using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agenda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        databaseEntities context = new databaseEntities();
        private agendum contato = null;
        private string operacao = "";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListarContatos();
            AlterarBotoes(1);
        }
        private void DtGridResultado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtGridResultado.SelectedIndex >= 0)
            {
                AlterarBotoes(3);
                contato = (agendum)dtGridResultado.SelectedItem;
                txtId.Text = contato.id.ToString();
                txtNome.Text = contato.nome;
                txtEmail.Text = contato.email;
                txtTelefone.Text = contato.telefone;
            }
        }
        private void ListarContatos()
        {            
                int quantidade = context.agenda.Count();
                labelQuantidade.Content = quantidade.ToString() + " registro(s) encontrado(s)";
                var consulta = context.agenda;
                dtGridResultado.ItemsSource = consulta.ToList();
        }
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            //Gravar no Banco de Dados
            if (operacao == "inserir")
            {
                contato = new agendum();
                contato.nome = txtNome.Text;
                contato.email = txtEmail.Text;
                contato.telefone = txtTelefone.Text;

                context.agenda.Add(contato);
                context.SaveChanges();
            }
            if (operacao == "alterar")
            {
                contato = context.agenda.Find(Convert.ToInt32(txtId.Text));
                if (context != null)
                {
                    contato.nome = txtNome.Text;
                    contato.email = txtEmail.Text;
                    contato.telefone = txtTelefone.Text;
                    context.SaveChanges();
                }
            }
            AlterarBotoes(1);
            ListarContatos();
            LimparCampos();
        }
        private void BtnInserir_Click(object sender, RoutedEventArgs e)
        {
            operacao = "inserir";
            AlterarBotoes(2);
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
            AlterarBotoes(1);
        }
        private void BtnLocalizar_Click(object sender, RoutedEventArgs e)
        {
            //buscar pelo nome
            if (txtNome.Text.Trim().Count() > 0)
            {
                try
                {
                    var consulta = from c in context.agenda
                    where c.nome.Contains(txtNome.Text)
                    select c;
                    dtGridResultado.ItemsSource = consulta.ToList();                    
                }
                catch { }
            }
        }
        private void BtnAlterar_Click(object sender, RoutedEventArgs e)
        {
            operacao = "alterar";
            AlterarBotoes(2);
        }
        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
           contato = context.agenda.Find(Convert.ToInt32(txtId.Text));
            if (context != null)
            {
                context.agenda.Remove(contato);
                context.SaveChanges();
                AlterarBotoes(1);
                ListarContatos();
                LimparCampos();
            }
        }
        private void AlterarBotoes(int opcao)
        {
            btnAlterar.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnExcluir.IsEnabled = false;
            btnInserir.IsEnabled = false;
            btnLocalizar.IsEnabled = false;
            btnSalvar.IsEnabled = false;

            if (opcao == 1)
            {
                //ativar opções iniciais
                btnInserir.IsEnabled = true;
                btnLocalizar.IsEnabled = true;
            }
            if (opcao == 2)
            {
                //Inserir um valor
                btnCancelar.IsEnabled = true;
                btnSalvar.IsEnabled = true;
            }
            if (opcao == 3)
            {
                btnAlterar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
        }
        private void LimparCampos()
        {
            txtId.Clear();
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
        }
    }
}
