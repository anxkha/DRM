<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DOTP.DRM.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">DRM - Account - Register</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js" type="text/javascript"></script>
 <script src="http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Create a New Account</h2>
    <p>Use the form below to create a new account.</p>
    <p>Passwords are required to be a minimum of 8 characters in length.</p>

    

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Account creation was unsuccessful. Please correct the errors and try again.") %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                
                <div class="editor-label">
                 <%: Html.LabelFor(m => m.FirstName) %>
                </div>
                <div class="editor-field">
                 <%: Html.TextBoxFor(m => m.FirstName) %>
                 <%: Html.ValidationMessageFor(m => m.FirstName) %>
                </div>
                
                <div class="editor-label">
                 <%: Html.LabelFor(m => m.Email) %>
                </div>
                <div class="editor-field">
                 <%: Html.TextBoxFor(m => m.Email) %>
                 <%: Html.ValidationMessageFor(m => m.Email) %>
                </div>
                
                <div class="editor-label">
                 <%: Html.LabelFor(m => m.Password) %>
                </div>
                <div class="editor-field">
                 <%: Html.PasswordFor(m => m.Password) %>
                 <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>

                <div class="editor-label">
                 <%: Html.LabelFor(m => m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                 <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                 <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </div>
                <p>
                 <input type="submit" value="Register" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
