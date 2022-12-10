using Azure.Core;
using Cliente_CRUD.Classes;
using Cliente_CRUD.Entityframework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Cliente_CRUD
{
    public partial class Edit : Page
    {
        public byte[] Foto
        {
            get
            {
                object o = ViewState["Foto"];
                return (o == null) ? null : (byte[])o;
            }

            set
            {
                ViewState["Foto"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["ID"], out int ID))
                {
                    using (CLIENTEEntities db = new CLIENTEEntities())
                    {
                        CLIENTE cliente = db.CLIENTE.FirstOrDefault(x => x.CLIE_PK_ID == ID);

                        txtNome.Text = cliente.CLIE_NOME;
                        txtEmail.Text = cliente.CLIE_EMAIL;
                        txtCnpj.Text = cliente.CLIE_CNPJ;
                        txtCpf.Text = cliente.CLIE_CPF;
                        txtNascimento.Text = cliente.CLIE_DATA_NASCIMENTO.Value.ToString("dd/MM/yyyy");
                        txtTelefone.Text = cliente.CLIE_TELEFONE;
                        ddlTipo.SelectedValue = cliente.CLIE_TIPO;
                        ddlStatus.SelectedValue = cliente.CLIE_STATUS.ToString();
                        imgImagem.ImageUrl = string.Format("data:image/jpeg;base64, {0}", Convert.ToBase64String(cliente.CLIE_IMAGEM));

                        ddlTipo_SelectedIndexChanged(sender, e);
                    }
                    upCliente.Update();
                    upImagem.Update();
                }
                else
                {
                    phCnpj.Visible = false;
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                String savePath = @"/Uploads/";

                if (flpImagem.HasFile)
                {
                    if (!Directory.Exists(Server.MapPath(savePath)))
                        Directory.CreateDirectory(savePath);

                    String fileName = flpImagem.FileName;

                    savePath += fileName;

                    if (File.Exists(Server.MapPath(savePath)))
                        File.Delete(Server.MapPath(savePath));

                    flpImagem.SaveAs(Server.MapPath(savePath));

                    Foto = File.ReadAllBytes(Server.MapPath(savePath));

                    imgImagem.ImageUrl = savePath;
                    upCliente.Update();
                }
                else
                {
                    Utilitarios.Alerta(this, "Escolha uma imagem antes de salvar!");
                }
            }catch(Exception ex)
            {
                hfError.Value = ex.Message;
                Utilitarios.Alerta(this, "Elgo deu errado ao carregar a imagem!");
            }

        }
        public void salvarCliente()
        {
            try
            {

                using (CLIENTEEntities db = new CLIENTEEntities())
                {
                    CLIENTE cliente = new CLIENTE()
                    {
                        CLIE_NOME = txtNome.Text,
                        CLIE_EMAIL = txtEmail.Text,
                        CLIE_CNPJ = txtCnpj.Text,
                        CLIE_CPF = txtCpf.Text,
                        CLIE_STATUS = Convert.ToInt32(ddlStatus.SelectedValue),
                        CLIE_TIPO = ddlTipo.SelectedValue,
                        CLIE_DATA_NASCIMENTO = Convert.ToDateTime(txtNascimento.Text),
                        CLIE_TELEFONE = txtTelefone.Text,
                        CLIE_IMAGEM = Foto,
                        CLIE_DATA_CADASTRO = DateTime.Now
                    };
                    
                    db.CLIENTE.Add(cliente);

                    db.SaveChanges();

                    txtNome.Text = txtEmail.Text = txtCnpj.Text = txtCpf.Text =
                    txtNascimento.Text = txtTelefone.Text = string.Empty;

                    Utilitarios.Alerta(this, "Cadastro salvo com sucesso!");

                }
                    Response.Redirect("/Default.aspx");
            }
            catch (Exception ex)
            {
                Utilitarios.Alerta(this, ex.Message);
            }
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            phCnpj.Visible = phCpf.Visible = false;

            if (ddlTipo.SelectedValue == "PF")
                phCpf.Visible = true;
            else
                phCnpj.Visible = true;

            upCliente.Update();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if(validarDados())
                salvarCliente();
        }

        public bool validarDados()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                    throw new Exception("Nome  não pode ser vazio");

                string email = txtEmail.Text;

                Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                if (string.IsNullOrWhiteSpace(txtEmail.Text))// ||)
                    throw new Exception("Email não pode ser vazio");

                if(!rg.IsMatch(email))
                    throw new Exception("Email invalido");

                if (ddlTipo.SelectedIndex == 0)
                    throw new Exception("É necessário escolher o tipo de cliente.");

                if (string.IsNullOrWhiteSpace(txtCpf.Text) && string.IsNullOrWhiteSpace(txtCnpj.Text))
                    throw new Exception("CPF e CNPJ estão vazios, pelo menos um deles deve ser preenchido");

                if (string.IsNullOrWhiteSpace(txtNascimento.Text))
                    throw new Exception("Nascimento não pode ser vazio");

                if (string.IsNullOrWhiteSpace(txtTelefone.Text))
                    throw new Exception("Telefone não pode ser vazio");

                if (ddlStatus.SelectedIndex == 0)
                    throw new Exception("É necessário escolher um status para o cliente.");

                return true;
            }catch(Exception ex)
            {
                Utilitarios.Alerta(this, ex.Message);
                return false;
            }
        }
    }
}