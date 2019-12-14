<%@ Page Title="Refunds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Refunds.aspx.cs" Inherits="eRaceWebApp.Sales.Refunds" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Refunds</h1>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
     <table class="table table-striped">
        <tr>
            <td class="col-4">
                <asp:Label runat="server" ID="EmployeeLabelLabel" AssociatedID="EmployeeLabel" Text="Employee Name: " />
                <asp:Literal runat="server" ID="EmployeeLabel" />
            </td>
            <td class="col-4">
                <asp:Label ID="OrderNumberLabel" runat="server" AssociatedControlID="OrderNumber" Text="Order Number: "/>
                <asp:TextBox ID="OrderNumber" runat="server"></asp:TextBox>
                <asp:LinkButton runat="server" ID="LookupBtn" CssClass="btn btn-primary" OnClick="LookupBtn_Click">Lookup Order</asp:LinkButton>
                <asp:LinkButton runat="server" ID="ClearBtn" CssClass="btn btn-danger" OnClick="ClearBtn_Click">Clear</asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="ReturnGV" runat="server" ItemType="eRaceSystem.DTOs.Sales.Refund" AutoGenerateColumns="false" GridLines="None" CssClass="table table-striped">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HiddenField ID="ProductID" runat="server" Value="<%#Item.ProductID%>"></asp:HiddenField>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Product">
                <ItemTemplate>
                    <asp:Label ID="ProductName" runat="server" Text="<%#Item.ItemName %>"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:Label ID="Quantity" runat="server" Text="<%#Item.Quantity %>"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>    
            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    <asp:Label ID="Price" runat="server" Text='<%# Item.Price.ToString("C") %>' Style="text-align: right"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Amount">
                <ItemTemplate>
                    <asp:Label ID="Amount" runat="server" Text='<%# Item.Amount.ToString("C") %>' Style="text-align: right"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Restock Chg">
                <ItemTemplate>
                    <asp:HiddenField ID="IsRefundable" runat="server" Value="<%#Item.IsRefundable%>"></asp:HiddenField>
                    <asp:CheckBox ID="RestockSelect" runat="server" AutoPostBack="true" />
                    <asp:Label ID="Restock" runat="server" Text='<%# Item.RestockCharge.ToString("C") %>'  Style="text-align: right"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Refund Reason">
                <ItemTemplate>
                    <asp:CheckBox ID="ReasonSelect" runat="server" AutoPostBack="true" />
                    <asp:TextBox ID="Reason" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <table class="table table-striped">
        <tr>
            <td class="col-4">
                <asp:Label ID="InvoiceLabel" runat="server" Text="Refund Invoice #: "/>
                <asp:TextBox ID="Invoice" runat="server" Enabled="false" ></asp:TextBox>
            </td>
            <td class="col-4">
                <asp:LinkButton ID="RefundBtn" runat="server" CssClass="btn btn-success btn-lg" OnClick="RefundBtn_Click" Text="Refund"></asp:LinkButton>
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
</asp:Content>
