<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DOTP.RaidManager" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DRM - Characters - Delete Character</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="/Scripts/DeleteCharacter.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="StatusContent" ContentPlaceHolderID="StatusContent" runat="server">
 <div class="errorPanel" id="errorPanel" style="display: none;">
  <h2>Unable to delete the character. Please correct the error and try again:</h2>
  <p id="errorContent"></p>
 </div>
 <div class="successPanel" id="successPanel" style="display: none">
  The character was successfully deleted. <%: Html.ActionLink("Go back to your character list.", "Index", "Characters") %>
 </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<p><%: Html.ActionLink("Go back to your character list.", "Index", "Characters") %></p>

<h2>Delete a Character</h2>

<% using( Html.BeginForm("Delete", "Characters", FormMethod.Post, new { id = "deleteForm" }) ) { %>

<%
    var character = (Character)ViewBag.Character;
    
    if(null != character)
    {
%>

 <p>Are you sure you want to delete this character?</p>

 <input type="hidden" value="<%: character.Name %>" id="Name" />
 <input type="hidden" value="<%: character.AccountID %>" id="AccountID" />

 <table>
  <thead>
   <tr>
    <td><b>Name</b></td>
    <td><b>Level</b></td>
    <td><b>Class</b></td>
    <td><b>Race</b></td>
    <td><b>Primary Specialization</b></td>
    <td><b>Secondary Specialization</b></td>
   </tr>
  </thead>
  <tbody>
   <tr>
    <td><%: character.Name %></td>
    <td><%: character.Level %></td>
    <td><%: character.Class %></td>
    <td><%: character.Race %></td>
    <td><%: Specialization.Store.ReadOneOrDefault(s => s.ID == character.PrimarySpecialization).Name %></td>
    <td><%: Specialization.Store.ReadOneOrDefault(s => s.ID == character.SecondarySpecialization).Name %></td>
   </tr>
  </tbody>
 </table>

 <input id="deleteButton" type="button" value="Delete" />
 <input id="noButton" type="button" value="No!" />

<%
    }
}
%>

</asp:Content>




