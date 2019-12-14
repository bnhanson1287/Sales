<%@ Page Title="Sales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="eRaceWebApp.Sales.Sales" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>In-Store Sales</h1>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    
    <table class="table table-striped">
        <tr>
            <td class="col-4">
                <asp:Label runat="server" ID="EmployeeLabelLabel" AssociatedID="EmployeeLabel" Text="Employee Name: " />
                <asp:Literal runat="server" ID="EmployeeLabel" />
            </td>
            <td class="col-4">
                <asp:Label ID="CategoryLabel" runat="server" AssociatedControlID="CategoryDropDown" Text="Category: "/>
                <asp:DropDownList runat="server" ID="CategoryDropDown" DataSourceID="CategoryDataSource" DataTextField="Description" DataValueField="CategoryID" AppendDataBoundItems="true" AutoPostBack="true">
                    <asp:ListItem Value="0">Select a Category </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="col-4">
                <asp:Label ID="ProductLabel" runat="server" AssociatedControlID="ProductDropDown" Text="Product: "/>
                <asp:DropDownList runat="server" ID="ProductDropDown" DataSourceID="ProductDataSource" DataTextField="ItemName" DataValueField="ProductID">
                </asp:DropDownList>
            </td>
            <td class="col-4">
                <asp:Label ID="QuantityLabel" runat="server" Text="Quantity: " AssociatedControlID="Quantity"/>
                <asp:TextBox runat="server" ID="Quantity" Width="50px" Text="1" Style="text-align: right; padding-right: 5px" TextMode="Number"/>
                <asp:LinkButton ID="AddItem" runat="server" OnClick="AddItem_Click" CssClass="btn btn-success"><i class="glyphicon glyphicon-plus"></i> Add</asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="Cart" ItemType="eRaceSystem.DTOs.Sales.CartItems" AutoGenerateColumns="false" GridLines="None" CssClass="table table-striped" HeaderStyle-HorizontalAlign="Center">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="ProductIDHF" runat="server" Value='<%# Item.ProductID %>' />
                </ItemTemplate>
            </asp:TemplateField >
            <asp:TemplateField HeaderText="Product">
                <ItemTemplate>
                    <asp:Label ID="Product" runat="server" Text='<%# Item.ItemName %>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="Quantity" runat="server" Text='<%# Item.Quantity %>' Style="padding-left:5px" TextMode="Number"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="Refresh" CssClass="btn btn-success" Width="45px" CommandName="Refresh" OnCommand="Refresh_Command"><i class="glyphicon glyphicon-refresh"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ItemPrice">
                <ItemTemplate>
                    <asp:Label ID="ItemPrice" runat="server" Text='<%# Item.ItemPrice.ToString("C") %>' Width="150px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Amount">
                <ItemTemplate>
                    <asp:Label ID="Amount" runat="server" Text='<%# Item.Amount.ToString("C") %>' Width="150px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CssClass="btn btn-danger" CommandName="Delete" CausesValidation="False" ID="DeleteBtn" OnCommand="DeleteBtn_Command" CommandArgument="<%#Item.ProductID %>"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <table class="table table-striped">
        <tr>
            <td class="col-4">
                <asp:LinkButton ID="SaleBtn" runat="server" CssClass="btn btn-success btn-lg" CommandName="Sale" OnCommand="SaleBtn_Command">Process Sale</asp:LinkButton>
                <asp:LinkButton ID="Clear" runat="server" CssClass="btn btn-danger" CommandName="Clear" OnCommand="Clear_Command">Clear Cart</asp:LinkButton>
            </td>
            <td class="col-4">
                <asp:Label ID="InvoiceLabel" runat="server" Text="Invoice #: "/>
                <asp:TextBox ID="Invoice" runat="server" Enabled="false"></asp:TextBox>
            </td>
             <td class="col-4" style="text-align:right">
                <div style="height:26px">
                    <asp:Label ID="SubtotalLabel" runat="server" Text="Subtotal: "/>
                </div><br />
                <div style="height:26px">
                    <asp:Label ID="GSTLabel" runat="server" Text="GST: "/>
                </div><br />
                <div style="height:26px">
                    <asp:Label ID="TotalLabel" runat="server" Text="Refund Total: "/>
                </div>
            </td>
            <td class="col-6" style="text-align:right">                
                <div style="height:26px">
                    <asp:Label ID="Subtotal" runat="server" Style="text-align: right"/>
                </div><br />
                <div style="height:26px">
                    <asp:Label ID="GST" runat="server"  Style="text-align: right"/>
                </div><br />
                <div style="height:26px">
                    <asp:Label ID="Total" runat="server"  Style="text-align: right"/>
                </div><br />
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource runat="server" ID="CategoryDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="ListCategory" TypeName="eRaceSystem.BLL.SalesController"></asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="ProductDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="ListProducts_ByCategory" TypeName="eRaceSystem.BLL.SalesController">
        <SelectParameters>
            <asp:ControlParameter ControlID="CategoryDropDown" PropertyName="SelectedValue" DefaultValue="0" Name="CategoryID" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
