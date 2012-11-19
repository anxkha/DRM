<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DOTP.RaidManager" %>
<%@ Import Namespace="DOTP.RaidManager.Drawing" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DRM - Raid - Roster</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="/Scripts/RosterRaid.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="StatusContent" ContentPlaceHolderID="StatusContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<h2><%: ViewBag.RaidDetails.RaidInstance.Name %> - Rostering</h2>

 <%
  
  var details = ViewBag.RaidDetails as RaidDetails;
  var tankSignups = ViewBag.TankSignups as List<RaidSignup>;
  var healerSignups = ViewBag.HealerSignups as List<RaidSignup>;
  var rangedSignups = ViewBag.RangedSignups as List<RaidSignup>;
  var meleeSignups = ViewBag.MeleeSignups as List<RaidSignup>;

  var rosterHelper = new Roster
  {
   Response = Response,
   TankSignups = tankSignups,
   HealerSignups = healerSignups,
   RangedSignups = rangedSignups,
   MeleeSignups = meleeSignups
  };

  int numberDrawn;
  
 %>

<table style="width: 100%" class="listTable">
 <thead>
  <tr>
   <td style="text-align: center; width: 50%">Signup Details</td>
   <td style="text-align: center; ">Raid Details</td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td style="vertical-align: top">
    <table style="width: 100%" class="listTable">
     <tbody>
      <tr>
       <td style="width: 50%"><b>Date</b></td>
       <td><%: details.RaidInstance.StartTime.ToShortDateString()%></td>
      </tr>
      <tr>
       <td><b>Invite Time</b></td>
       <td><%: details.RaidInstance.InviteTime.ToShortTimeString()%></td>
      </tr>
      <tr>
       <td><b>Start Time</b></td>
       <td><%: details.RaidInstance.StartTime.ToShortTimeString() %></td>
      </tr>
      <tr>
       <td><b>Rostered Members</b></td>
       <td><%: ViewBag.NumRostered %> (<%: ViewBag.PercentageRostered %>%)</td>
      </tr>
      <tr>
       <td><b>Queued Members</b></td>
       <td><%: ViewBag.NumQueued %> (<%: ViewBag.PercentageQueued %>%)</td>
      </tr>
      <tr>
       <td><b>Cancelled Signups</b></td>
       <td><%: ViewBag.NumCancelled %> (<%: ViewBag.PercentageCancelled %>%)</td>
      </tr>
      <tr>
       <td><b>Total Signups</b></td>
       <td><%: ViewBag.NumTotal %></td>
      </tr>
     </tbody>
    </table>
   </td>
   <td style="vertical-align: top">
    <table style="width: 100%" class="listTable">
     <tbody>
      <tr>
       <td style="width: 50%"><b>Raid</b></td>
       <td><%: details.Raid.Name%></td>
      </tr>
      <tr>
       <td><b>Expansion</b></td>
       <td><%: details.Raid.Expansion %></td>
      </tr>
      <tr>
       <td style="width: 50%"><b>Minimum Level</b></td>
       <td><%: details.Raid.MinimumLevel %></td>
      </tr>
      <tr>
       <td><b>Raid Maximum</b></td>
       <td><%: details.Raid.MaxPlayers %></td>
      </tr>
      <tr>
       <td><b>Number of Bosses</b></td>
       <td><%: details.Raid.NumberOfBosses %></td>
      </tr>
     </tbody>
    </table>
   </td>
  </tr>
 </tbody>
</table>
<br />

<form action="#" method="post">
 <h2>Rostered Signups</h2>
 <table class="rosterTable listTable">
  <thead>
   <tr>
    <td>Character</td>
    <td>Role</td>
    <td>Class</td>
    <td>Race</td>
    <td>Rostered Specialization</td>
    <td>Signup Time</td>
    <td>Unroster</td>
   </tr>
  </thead>
  <%

   numberDrawn = 0;

   numberDrawn = rosterHelper.DrawTankRows(true);
   numberDrawn += rosterHelper.DrawHealerRows(true);
   numberDrawn += rosterHelper.DrawRangedRows(true);
   numberDrawn += rosterHelper.DrawMeleeRows(true);
  
   if(numberDrawn > 0)
   {
  %>
  <tr>
   <td colspan="7">
    <input type="button" class="updateButton" id="updateRostered" name="update" value="Update" />
    <input type="button" class="cancelButton" id="cancelRostered" name="cancel" value="Cancel" />
   </td>
  </tr>
  <% } else { %>
  <tr>
   <td colspan="7">
    None rostered.
   </td>
  </tr>
  <% } %>
 </table>
 <h2>Queued Signups</h2>
 <table class="rosterTable listTable">
  <thead>
   <tr>
    <td>Character</td>
    <td>Role</td>
    <td>Class</td>
    <td>Race</td>
    <td>Rostered Specialization</td>
    <td>Signup Time</td>
    <td>Roster</td>
   </tr>
  </thead>
  <%

   numberDrawn = 0;

   numberDrawn = rosterHelper.DrawTankRows();
   numberDrawn += rosterHelper.DrawHealerRows();
   numberDrawn += rosterHelper.DrawRangedRows();
   numberDrawn += rosterHelper.DrawMeleeRows();
  
   if(numberDrawn > 0)
   {
  %>
  <tr>
   <td colspan="7">
    <input type="button" id="updateQueued" class="updateButton" name="update" value="Update" />
    <input type="button" id="cancelQueued" class="cancelButton" name="cancel" value="Cancel" />
   </td>
  </tr>
  <% } else { %>
  <tr>
   <td colspan="7">
    None rostered.
   </td>
  </tr>
  <% } %>
 </table>
</form>

</asp:Content>
