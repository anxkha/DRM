<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DOTP.RaidManager" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DOTP - Raid - Archive</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="/Scripts/ArchiveRaidInstance.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="StatusContent" ContentPlaceHolderID="StatusContent" runat="server">
 <div class="errorPanel" id="errorPanel" style="display: none;">
  <h2>Unable to archive the raid instance. Please correct the error and try again:</h2>
  <p id="errorContent"></p>
 </div>
 <div class="successPanel" id="successPanel" style="display: none">
  The raid instance was successfully archived. <%: Html.ActionLink("Go back to the raid list.", "Index", "Home") %>
 </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<h2>Archive a Raid Instance</h2>

<p>Are you sure you want to archive this raid instance? This action <em>can</em> be undone.</p>

<%
using( Html.BeginForm("Delete", "Characters", FormMethod.Post, new { id = "deleteForm" }) )
{
    var raidInstance = (RaidInstance)ViewBag.RaidInstance;
    
%>

<p>All times are in server time (EST).</p>

<input type="hidden" id="ID" name="ID" value="<%: raidInstance.ID %>" />
<input type="hidden" id="Name" name="Name" value="<%: raidInstance.Name %>" />

<table style="width: 100%" class="listTable">
 <thead>
  <tr>
   <td><b>Raid ID</b></td>
   <td><b>Name</b></td>
   <td><b>Raid</b></td>
   <td><b>Invite Time</b></td>
   <td><b>Start Time</b></td>
   <td></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td><%: raidInstance.ID %></td>
   <td><%: raidInstance.Name %></td>
   <td><%: raidInstance.Raid %></td>
   <td><%: raidInstance.InviteTime.ToShortDateString() + " " + raidInstance.InviteTime.ToShortTimeString() %></td>
   <td><%: raidInstance.StartTime.ToShortDateString() + " " + raidInstance.StartTime.ToShortTimeString() %></td>
  </tr>
 </tbody>
</table>

<br />

<input type="button" name="Archive" id="Archive" value="Archive" />
<input type="button" name="Cancel" id="Cancel" value="Cancel" />

<% } %>

</asp:Content>


