using Cliente_CRUD.Classes;
using Cliente_CRUD.Entityframework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cliente_CRUD
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (CLIENTEEntities db = new CLIENTEEntities())
                {
                    var clinetes = db.CLIENTE.ToList();

                    lvClientes.DataSource = clinetes;
                    lvClientes.DataBind();
                }
            }catch(Exception ex)
            {
                hfError.Value = ex.Message;
                Utilitarios.Alerta(this, "Um erro ocorreu ao carregar a página!");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtPesquisa.Text))
                {
                    using (CLIENTEEntities db = new CLIENTEEntities())
                    {
                        var clinetes = db.CLIENTE.Where(x => x.CLIE_NOME.Contains(txtPesquisa.Text.Trim())).ToList();

                        lvClientes.DataSource = clinetes;
                        lvClientes.DataBind();

                        upClientes.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                hfError.Value = ex.Message;
                Utilitarios.Alerta(this, "Um erro ocorreu ao tentar buscar os registros!");
            }
        }
    }
}