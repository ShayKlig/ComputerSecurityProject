<%@ Page Title="Computer Security Project" Language="C#" MasterPageFile="~/Site.Master" validateRequest="false" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ComputerSecurityProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Computer Security Project</h1>
        <p class="lead">Team members: Shay Kligerman, Omer Revach, Lee Benjamin</p>
        <asp:LoginView runat="server" ViewStateMode="Disabled">
        <LoggedInTemplate>
            <form>
          <div class="form-group">
            <label for="exampleInputEmail1">Email address</label>
            <input name="inputEmail" type="email" class="form-control" id="inputEmail" aria-describedby="emailHelp" placeholder="Enter email">
            <small id="emailHelp" class="form-text text-muted">We'll never share your email with anyone else.</small>
          </div>
          <div class="form-group">
            <label for="exampleInputPassword1">Customer Name</label>
            <input name="inputCustomerName" type="text" class="form-control" id="inputCustomerName" placeholder="Name">
          </div>
            <div class="form-group">
            <label for="exampleInputPassword1">Customer Id</label>
            <input type="number" class="form-control" id="inputCustomerId" name="inputCustomerId" placeholder="Customer Id">
          </div>
          <asp:Button runat="server" OnClick="AddCustomer" Text="Add customer" CssClass="btn btn-default" />
            <div class="form-group">
            <label for="exampleInputPassword1">Search customer by name</label>
            <input type="text" class="form-control" id="searchCustomerInput" name="searchCustomerInput">
          <asp:Button runat="server" OnClick="OnTextChanged" Text="Search Customer" CssClass="btn btn-default" />

          </div>

        </form>
        </LoggedInTemplate>
        </asp:LoginView>        
        <asp:Literal ID="TableLiteral" runat="server"></asp:Literal>        
    </div>
</asp:Content>
