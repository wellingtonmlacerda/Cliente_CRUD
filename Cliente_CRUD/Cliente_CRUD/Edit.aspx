<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Cliente_CRUD.Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="ContentScripts" ContentPlaceHolderID="Js" runat="server">
    <script type="text/javascript" defer="defer">
        window.onload = function () {
            habilitaCampo();
        };
            function habilitaCampo() {
                var cpf = $('#groupCpf')
                var cnpj = $('#groupCnpj')

                cpf.addClass('hide')
                cnpj.addClass('hide')

                var ddlTipo = $('#<%= ddlTipo.ClientID%> option:selected').val();

                if (ddlTipo == 'PF' || ddlTipo == '')
                    cpf.removeClass('hide')
                else
                    cnpj.removeClass('hide')
        }
        function confirma() {
            if (confirm('Realmente deseja excluír esse registro?') == true)
                return true

            return false
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h1>Clientes - Cadastro</h1>
        <asp:HiddenField ID="hfError" runat="server" />
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label for="imgImagem" class="col-sm-2 control-label">Foto</label>
                <div class="col-sm-10">
                    <asp:UpdatePanel ID="upImagem" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Image ID="imgImagem" runat="server" ImageUrl="~/Imagens/sem-foto.jpg" CssClass="img-thumbnail foto" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:FileUpload ID="flpImagem" runat="server" /><br />
                    <asp:Button ID="btnUpload" OnClick="btnUpload_Click" CssClass="btn btn-primary" runat="server" Text="Salvar imagem" />
                </div>
            </div>
            <asp:UpdatePanel ID="upCliente" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="form-group">
                        <label for="txtNome" class="col-sm-2 control-label">Nome</label>
                        <div class="col-sm-10">
                            <asp:RequiredFieldValidator ID="rfvNome" runat="server" ValidationGroup="cliente" ErrorMessage="*Campo obrigatório" CssClass="label label-danger" ControlToValidate="txtNome" SetFocusOnError="true" />
                            <asp:TextBox ID="txtNome" CssClass="form-control" runat="server" ValidationGroup="cliente"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txtNascimento" class="col-sm-2 control-label">Data de Nascimento</label>
                        <div class="col-sm-10">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="cliente" ErrorMessage="*Campo obrigatório" CssClass="label label-danger" ControlToValidate="txtNascimento" SetFocusOnError="true" />
                            <asp:TextBox ID="txtNascimento" Type="date" CssClass="form-control" runat="server" ValidationGroup="cliente"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txtTelefone" class="col-sm-2 control-label">Telefone</label>
                        <div class="col-sm-10">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="cliente" ErrorMessage="*Campo obrigatório" CssClass="label label-danger" ControlToValidate="txtTelefone" SetFocusOnError="true" />
                            <asp:TextBox ID="txtTelefone" CssClass="form-control telefone" runat="server" ValidationGroup="cliente"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ddlTipo" class="col-sm-2 control-label">Tipo</label>
                        <div class="col-sm-10">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="cliente" ErrorMessage="*Campo obrigatório" CssClass="label label-danger" ControlToValidate="ddlTipo" SetFocusOnError="true" />
                            <asp:DropDownList ID="ddlTipo" CssClass="form-control" runat="server" OnClick="habilitaCampo()" ValidationGroup="cliente">
                                <asp:ListItem Text="Selecione..." Value="" />
                                <asp:ListItem Text="Pessoa Física" Value="PF" />
                                <asp:ListItem Text="Pessoa Jurídica" Value="PJ" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txtEmail" class="col-sm-2 control-label">Email</label>
                        <div class="col-sm-10">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="cliente" ErrorMessage="*Campo obrigatório" CssClass="label label-danger" ControlToValidate="txtEmail" SetFocusOnError="true" />
                            <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" Type="email" ValidationGroup="cliente" />
                        </div>
                    </div>
                        <div class="form-group groupCpf" id="groupCpf">
                            <label for="txtCpf" class="col-sm-2 control-label">CPF</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtCpf" CssClass="form-control cpf" runat="server" />
                            </div>
                        </div>
                        <div class="form-group groupCnpj" id="groupCnpj">
                            <label for="txtCnpj" class="col-sm-2 control-label">CNPJ</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtCnpj" CssClass="form-control cnpj" runat="server" />
                            </div>
                        </div>
                    <div class="form-group">
                        <label for="ddlStatus" class="col-sm-2 control-label">Status</label>
                        <div class="col-sm-10">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="cliente" ErrorMessage="*Campo obrigatório" CssClass="label label-danger" ControlToValidate="ddlStatus" SetFocusOnError="true" />
                            <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" ValidationGroup="cliente">
                                <asp:ListItem Text="Selecione..." Value="" />
                                <asp:ListItem Text="Ativo" Value="1" />
                                <asp:ListItem Text="Inativo" Value="0" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-10">
                            <asp:Button ID="btnSalvar" CssClass="btn btn-success" runat="server" OnClick="btnSalvar_Click" Text="Salvar" ValidationGroup="cliente" />
                            <asp:LinkButton ID="lkbVoltar" CssClass="btn btn-primary" PostBackUrl="~/Default.aspx" runat="server" Text="Voltar" />
                            <asp:LinkButton ID="lkbDelete" CssClass="btn btn-danger" runat="server" Text="Excluir" OnClick="lkbDelete_Click" OnClientClick="return confirma()" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
