<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="DOTP.RaidManager" %>
<%@ Import Namespace="DOTP.Users" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DRM - Home</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<h2>Welcome to the Defenders of the Pass raid management site!</h2>

<p>All times are in server time (EST).</p>
<p>
 Scheduled raids
<% if((null != Manager.GetCurrentUser()) && Manager.GetCurrentUser().IsAdmin){ %>
 (<a href="/Raid/Schedule" title="Schedule a raid"><img src="/Content/images/new-icon.png" alt="New" /></a>)
<% } %>
 :
</p>

<table style="width: 100%" class="listTable">
 <thead>
  <tr>
   <td><b>Raid ID</b></td>
   <td><b>Name</b></td>
   <td><b>Raid</b></td>
   <td><b>Invite Time</b></td>
   <td><b>Start Time</b></td>
   <td></td>
<% if((null != Manager.GetCurrentUser()) && (Manager.GetCurrentUser().IsAdmin || Manager.GetCurrentUser().IsRaidTeam)){ %>
   <td></td>
   <td></td>
<% } %>
  </tr>
 </thead>
 <tbody>

<%
var instances = RaidInstance.Store.ReadAll(ri => !ri.Archived);
    
if( null != instances )
{
    foreach(var raid in instances)
    {
%>   

  <tr>
   <td><%: raid.ID %></td>
   <td><%: raid.Name %></td>
   <td><%: raid.Raid %></td>
   <td><%: raid.InviteTime.ToShortDateString() + " " + raid.InviteTime.ToLongTimeString() %></td>
   <td><%: raid.StartTime.ToShortDateString() + " " + raid.StartTime.ToLongTimeString() %></td>
   <td><a href="/Raid/Signup?ID=<%: raid.ID %>" title="View signup details for this raid"><img src="/Content/images/calendar-icon.png" alt="Sign Up" /></a></td>
<% if((null != Manager.GetCurrentUser()) && Manager.GetCurrentUser().IsAdmin){ %>
   <td><a href="/Raid/Edit?ID=<%: raid.ID %>" title="Edit this raid"><img src="/Content/images/edit-icon.png" alt="Edit" /></a></td>
   <td><a href="/Raid/Archive?ID=<%: raid.ID %>" title="Archive this raid"><img src="/Content/images/archive-icon.png" alt="Archive" /></a></td>
<% } %>
  </tr>

<%
    }
}
else
{
%>

  <tr>
   <td colspan="5">There are no raids scheduled at this time.</td>
  </tr>

<%
}    
%>

 </tbody>
</table>

<p>Archived raids:</p>

<table style="width: 100%" class="listTable">
 <thead>
  <tr>
   <td><b>Raid ID</b></td>
   <td><b>Name</b></td>
   <td><b>Raid</b></td>
   <td><b>Invite Time</b></td>
   <td><b>Start Time</b></td>
   <td></td>
<% if((null != Manager.GetCurrentUser()) && Manager.GetCurrentUser().IsAdmin){ %>
   <td></td>
<% } %>
  </tr>
 </thead>
 <tbody>

<%
instances = RaidInstance.Store.ReadAll(ri => ri.Archived);
    
if( null != instances )
{
    foreach(var raid in instances)
    {
%>   

  <tr>
   <td><%: raid.ID %></td>
   <td><%: raid.Name %></td>
   <td><%: raid.Raid %></td>
   <td><%: raid.InviteTime.ToShortDateString() + " " + raid.InviteTime.ToLongTimeString() %></td>
   <td><%: raid.StartTime.ToShortDateString() + " " + raid.StartTime.ToLongTimeString() %></td>
   <td><a href="/Raid/Signup?ID=<%: raid.ID %>" title="View signup details for this raid"><img src="/Content/images/calendar-icon.png" alt="Sign Up" /></a></td>
<% if((null != Manager.GetCurrentUser()) && Manager.GetCurrentUser().IsAdmin){ %>
   <td><a href="/Raid/UnArchive?ID=<%: raid.ID %>" title="Un-Archive this raid"><img src="/Content/images/archive-icon.png" alt="Un-Archive" /></a></td>
<% } %>
  </tr>

<%
    }
}
else
{
%>

  <tr>
   <td colspan="5">There are no archived raids at this time.</td>
  </tr>

<%
}    
%>

 </tbody>
</table>

</asp:Content>
