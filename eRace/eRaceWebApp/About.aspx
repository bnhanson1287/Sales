<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="eRaceWebApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    

    <h4>Employee Username & Passwords</h4>
    <p>*** Note: You must login as a 'Clerk' to access sales system.</p>
    <asp:Label runat="server">Admin User Name: Webmaster</asp:Label><br />
    <asp:Label runat="server">Admin Password: Pa$$w0rd</asp:Label><br />   
    <asp:GridView runat="server" ID="EmployeeGrid" AutoGenerateColumns="False" DataSourceID="EmployeePositions">

        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID"></asp:BoundField>
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName"></asp:BoundField>
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"></asp:BoundField>
            <asp:TemplateField HeaderText="Password">
                <ItemTemplate>
                    <asp:Label runat="server" ID="Password">Pa$$word1</asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:ObjectDataSource runat="server" ID="EmployeePositions" SelectMethod="ListEmployeeAndPosition" TypeName="eRaceSystem.BLL.eRaceController">
        <SelectParameters>
            <asp:Parameter DefaultValue="erace.ca" Name="emailDomain" Type="String"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>


    <h2>Connection String Information</h2>
    <h3>Connection String Names:</h3> <p>eRaceDb</p>
    <p>DefaultConnection</p>
</asp:Content>
