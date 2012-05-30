<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DRM - Account Management</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Account Management</h2>

<p><%: Html.ActionLink("Change your password", "ChangePassword", "Account") %></p>
<p><%: Html.ActionLink("Manage your characters", "Index", "Characters") %></p>

</asp:Content>
