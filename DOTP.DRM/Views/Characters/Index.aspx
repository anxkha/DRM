<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="DOTP.RaidManager" %>
<%@ Import Namespace="DOTP.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 DRM - Account Management - Characters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h2>Characters</h2>
 <p><%: Html.ActionLink("Add Character", "Add", "Characters") %></p>
 <table class="listTable" style="width: 75em;">
  <thead>
   <tr>
    <td><b>Name</b></td>
    <td><b>Level</b></td>
    <td><b>Class</b></td>
    <td><b>Race</b></td>
    <td><b>Primary Specialization</b></td>
    <td><b>Secondary Specialization</b></td>
    <td></td>
    <td></td>
   </tr>
  </thead>
  <tbody>

<%
    
    var characters = (List<Character>)ViewBag.Characters;
    
    if(null != characters)
    {
        foreach(var character in characters)
        {
    
%>   

   <tr>
    <td><%: character.Name %></td>
    <td><%: character.Level %></td>
    <td><%: character.Class %></td>
    <td><%: character.Race %></td>
    <td><%: Specialization.Store.ReadOneOrDefault(s => s.ID == character.PrimarySpecialization).Name %></td>
    <td><%: Specialization.Store.ReadOneOrDefault(s => s.ID == character.SecondarySpecialization).Name %></td>
    <td><a href="/Characters/Edit?Name=<%: character.Name %>" title="Edit this character"><img src="/Content/images/edit-icon.png" alt="Edit" /></a></td>
    <td><a href="/Characters/Delete?Name=<%: character.Name %>" title="Delete this character"><img src="/Content/images/delete-icon.png" alt="Edit" /></a></td>
   </tr>

<%

        }
    }
    
%>

  </tbody>
 </table>
</asp:Content>
