using Azure.Core;
using Cliente_CRUD.Classes;
using Cliente_CRUD.Entityframework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
                lkbDelete.Visible = false;

                if (int.TryParse(Request.QueryString["ID"], out int ID))
                {
                    using (CLIENTEEntities db = new CLIENTEEntities())
                    {
                        CLIENTE cliente = db.CLIENTE.FirstOrDefault(x => x.CLIE_PK_ID == ID);

                        txtNome.Text = cliente.CLIE_NOME;
                        txtEmail.Text = cliente.CLIE_EMAIL;
                        txtCnpj.Text = cliente.CLIE_CNPJ;
                        txtCpf.Text = cliente.CLIE_CPF;
                        txtNascimento.Text = cliente.CLIE_DATA_NASCIMENTO.Value.ToString("yyyy-MM-dd");
                        txtTelefone.Text = cliente.CLIE_TELEFONE;
                        ddlTipo.SelectedValue = cliente.CLIE_TIPO;
                        ddlStatus.SelectedValue = cliente.CLIE_STATUS.ToString();

                        if (cliente.CLIE_IMAGEM != null)
                        {
                            Foto = cliente.CLIE_IMAGEM;
                            imgImagem.ImageUrl = string.Format("data:image/jpeg;base64, {0}", Convert.ToBase64String(cliente.CLIE_IMAGEM));
                        }
                    }

                    lkbDelete.Visible = true;

                    upCliente.Update();
                    upImagem.Update();
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
                    Utilitarios.Alerta(this, "Escolha uma imagem antes de tentar salvar!");
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
                    if (int.TryParse(Request.QueryString["ID"], out int ID))
                    {
                        if(db.CLIENTE.FirstOrDefault(x => x.CLIE_PK_ID == ID) is CLIENTE cliente)
                        {
                            cliente.CLIE_NOME = txtNome.Text;
                            cliente.CLIE_EMAIL = txtEmail.Text;
                            cliente.CLIE_CNPJ = txtCnpj.Text;
                            cliente.CLIE_CPF = txtCpf.Text;
                            cliente.CLIE_STATUS = Convert.ToInt32(ddlStatus.SelectedValue);
                            cliente.CLIE_TIPO = ddlTipo.SelectedValue;
                            cliente.CLIE_DATA_NASCIMENTO = Convert.ToDateTime(txtNascimento.Text);
                            cliente.CLIE_TELEFONE = txtTelefone.Text;
                            cliente.CLIE_IMAGEM = Foto;
                            cliente.CLIE_DATA_CADASTRO = DateTime.Now;

                            db.SaveChanges();

                            Utilitarios.Alerta(this, "Cadastro atualizado com sucesso!");
                        }
                    }
                    else
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

                        Response.Redirect($"/Edit.aspx?ID={cliente.CLIE_PK_ID}");
                    }

                }
                    
            }
            catch (Exception ex)
            {
                Utilitarios.Alerta(this, ex.Message);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if(validarDados())
                salvarCliente();

            Utilitarios.ScriptExecute(this, "habilitaCampo();");
        }

        protected void lkbDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (CLIENTEEntities db = new CLIENTEEntities())
                {
                    if (int.TryParse(Request.QueryString["ID"], out int ID))
                    {
                        if (db.CLIENTE.FirstOrDefault(x => x.CLIE_PK_ID == ID) is CLIENTE cliente)
                        {
                            db.CLIENTE.Remove(cliente);

                            db.SaveChanges();

                            Utilitarios.Alerta(this, "Registro excluído com sucesso.");

                            Response.Redirect($"/Default.aspx");
                        }
                    }
                }
            }catch(Exception ex)
            {
                Utilitarios.Alerta(this, ex.Message);
            }
        }

        #region VALIDADORES
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

                if (!string.IsNullOrWhiteSpace(txtCpf.Text) && !validaCPF(txtCpf.Text))
                    throw new Exception("O CPF informado é invalido");

                if (!string.IsNullOrWhiteSpace(txtCnpj.Text) && !VerificaCnpj(txtCnpj.Text))
                    throw new Exception("O CNPJ informado é invalido");

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

        public bool validaCPF(string cpf)
        {

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public Boolean VerificaCnpj(String cnpj)
        {
            if (Regex.IsMatch(cnpj, @"(^(\d{2}.\d{3}.\d{3}/\d{4}-\d{2})|(\d{14})$)"))
            {
                return validaCnpj(cnpj);
            }
            else
            {
                return false;
            }

        }

        private Boolean validaCnpj(String cnpj)
        {
            Int32[] digitos, soma, resultado;

            Int32 nrDig;

            String ftmt;

            Boolean[] cnpjOk;

            cnpj = cnpj.Replace("/", "");

            cnpj = cnpj.Replace(".", "");

            cnpj = cnpj.Replace("-", "");

            if (cnpj == "00000000000000")
            {
                return false;
            }

            ftmt = "6543298765432";

            digitos = new Int32[14];

            soma = new Int32[2];

            soma[0] = 0;

            soma[1] = 0;

            resultado = new Int32[2];

            resultado[0] = 0;

            resultado[1] = 0;

            cnpjOk = new Boolean[2];

            cnpjOk[0] = false;

            cnpjOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));

                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));

                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        cnpjOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        cnpjOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));

                }

                return (cnpjOk[0] && cnpjOk[1]);
            }
            catch
            {
                return false;
            }

        }
        #endregion

    }
}