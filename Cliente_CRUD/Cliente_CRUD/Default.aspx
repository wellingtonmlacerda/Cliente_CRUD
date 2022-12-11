<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Cliente_CRUD._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h1>Clientes</h1>
        <asp:HiddenField ID="hfError" runat="server" />
        <div class="col-md-12">
            <div class="form-group">
                <a class="btn btn-primary" href="Edit.aspx">Novo Cliente</a>
            </div>
            <div class="form-group">
                <asp:Panel ID="pnPesquisa" CssClass="panel" runat="server" DefaultButton="btnBuscar">
                    <label for="imgImagem" class="col-md-2 control-label">Buscar</label>
                    <asp:TextBox ID="txtPesquisa" CssClass="col-md-9 form-control" runat="server" />
                    <asp:Button runat="server" ID="btnBuscar" OnClick="btnBuscar_Click" CssClass="col-md-1 btn btn-primary" Text="Buscar" />
                </asp:Panel>
            </div>
        </div>
    </div>
            <div class="row">
                <div class="col-md-12">
                <asp:UpdatePanel ID="upClientes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:ListView ID="lvClientes" runat="server">
                        <LayoutTemplate>
                            <table class="table table-striped">
                                <tr>
                                    <th scope="col">#</th>
                                    <th scope="col">Codigo</th>
                                    <th scope="col">Nome</th>
                                    <th scope="col">Email</th>
                                    <th scope="col">Tipo</th>
                                    <th scope="col">Data Cadastro</th>
                                    <th scope="col">Status</th>
                                </tr>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </table>
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="5">
                                <Fields>
                                    <asp:NextPreviousPagerField
                                        ButtonType="Link"
                                        ShowFirstPageButton="false"
                                        ShowLastPageButton="false" />
                                </Fields>
                            </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <a href="Edit.aspx?ID=<%# Eval("CLIE_PK_ID") %>" title="Editar"><i class="glyphicon glyphicon-check"></i></a>
                                </td>
                                <td>
                                    <asp:Label ID="lblClienteID" runat="server" Text='<%# Eval("CLIE_PK_ID") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblClienteNome" runat="server" Text='<%# Eval("CLIE_NOME") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("CLIE_EMAIL") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("CLIE_TIPO") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblDataCadastro" runat="server" Text='<%# Eval("CLIE_DATA_CADASTRO") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# (int)Eval("CLIE_STATUS") == 1 ? "Ativo" : "Inativo" %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>
                </div>
            </div>
</asp:Content>
