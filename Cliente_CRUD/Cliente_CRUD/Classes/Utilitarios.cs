using AjaxControlToolkit.Bundling;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Cliente_CRUD.Classes
{
    public class Utilitarios
    {
        public static void Alerta (Page control, string mensagem)
        {
            ScriptManager.RegisterClientScriptBlock(control,control.GetType(), mensagem,"<script> alert('"+mensagem+"');</script>",false);
        }
        public static void ScriptExecute(Page control, string script)
        {
            ScriptManager.RegisterClientScriptBlock(control, control.GetType(), "", "<script> " + script + "</script>", false);
        }
    }
}