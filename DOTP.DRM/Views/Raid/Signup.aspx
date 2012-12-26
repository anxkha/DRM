<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DOTP.RaidManager" %>
<%@ Import Namespace="DOTP.RaidManager.Drawing" %>
<%@ Import Namespace="DOTP.Users" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DRM - Raid - Signup</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="/Scripts/SignupRaid.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="StatusContent" ContentPlaceHolderID="StatusContent" runat="server">
 <div class="errorPanel" id="errorPanel" style="display: none;">
  <h2>Please correct any errors and try again:</h2>
  <p id="errorContent"></p>
 </div>
 <div class="successPanel" id="successPanel" style="display: none">
  <span id="successContent">You are successfully signed up for the raid.</span> <a href="Signup?ID=<%: ViewBag.RaidDetails.ID %>">Go back to the raid details.</a>
 </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<h2><%: ViewBag.RaidDetails.RaidInstance.Name %> - Signup Details</h2>

<table style="width: 100%" class="listTable">
 <thead>
  <tr>
   <td style="text-align: center; font-weight: bold">Raid Description</td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td style="vertical-align: top; text-align: left;">
    <%: ViewBag.RaidDetails.RaidInstance.Description %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<!-- Raid Details -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr>
   <td style="text-align: center; font-weight: bold; width: 50%">Signup Details</td>
   <td style="text-align: center; font-weight: bold">Raid Details</td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td style="vertical-align: top">
    <table style="width: 100%" class="listTable">
     <tbody>
      <tr>
       <td style="width: 50%"><b>Date</b></td>
       <td><%: ViewBag.RaidDetails.RaidInstance.StartTime.ToShortDateString()%></td>
      </tr>
      <tr>
       <td><b>Invite Time</b></td>
       <td><%: ViewBag.RaidDetails.RaidInstance.InviteTime.ToShortTimeString()%></td>
      </tr>
      <tr>
       <td><b>Start Time</b></td>
       <td><%: ViewBag.RaidDetails.RaidInstance.StartTime.ToShortTimeString() %></td>
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
       <td><%: ViewBag.RaidDetails.Raid.Name%></td>
      </tr>
      <tr>
       <td><b>Expansion</b></td>
       <td><%: ViewBag.RaidDetails.Raid.Expansion %></td>
      </tr>
      <tr>
       <td style="width: 50%"><b>Minimum Level</b></td>
       <td><%: ViewBag.RaidDetails.Raid.MinimumLevel %></td>
      </tr>
      <tr>
       <td><b>Raid Maximum</b></td>
       <td><%: ViewBag.RaidDetails.Raid.MaxPlayers %></td>
      </tr>
      <tr>
       <td><b>Number of Bosses</b></td>
       <td><%: ViewBag.RaidDetails.Raid.NumberOfBosses %></td>
      </tr>
     </tbody>
    </table>
   </td>
  </tr>
 </tbody>
</table>

<br />

<%
 Signup signupHelper = new Signup
 {
  Response = Response
 };
%>

<!-- Rostered Tanks -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td><b>Rostered Tanks</b></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>
<%
    List<RaidSignup> signups = ViewBag.RaidDetails.Signups;

    var tanks = null == signups ? null : ViewBag.RaidDetails.GetTankSignups();

    if ((null == tanks) || (0 == tanks.Count))
    {
        Response.Write("None");
    }
    else
    {
%>
    <table style="width: 100%" class="listTable">
     <thead>
      <% signupHelper.DrawHeader(); %>
     </thead>
     <tbody>
<%
        foreach (var signup in tanks)
        {
            signupHelper.DrawRow(signup, true, true);
        }
%>
     </tbody>
    </table>
<% } %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<!-- Rostered Healers -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td><b>Rostered Healers</b></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>
<%
    var healers = null == signups ? null : ViewBag.RaidDetails.GetHealerSignups();
    
    if ((null == healers) || (0 == healers.Count))
    {
        Response.Write("None");
    }
    else
    {
%>
    <table style="width: 100%" class="listTable">
     <thead>
      <% signupHelper.DrawHeader(); %>
     </thead>
     <tbody>
<%
        foreach (var signup in healers)
        {
            signupHelper.DrawRow(signup, true, true);
        }
%>
     </tbody>
    </table>
<% } %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<!-- Rostered Melee -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td><b>Rostered Melee</b></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>
<%
    var melee = null == signups ? null : ViewBag.RaidDetails.GetMeleeSignups();

    if ((null == melee) || (0 == melee.Count))
    {
        Response.Write("None");
    }
    else
    {
%>
    <table style="width: 100%" class="listTable">
     <thead>
      <% signupHelper.DrawHeader(); %>
     </thead>
     <tbody>
<%
        foreach (var signup in melee)
        {
         signupHelper.DrawRow(signup, true, true);
        }
%>
     </tbody>
    </table>
<% } %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<!-- Rostered Ranged -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td><b>Rostered Ranged</b></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>
<%
    var ranged = null == signups ? null : ViewBag.RaidDetails.GetRangedSignups();
    
    if ((null == ranged) || (0 == ranged.Count))
    {
        Response.Write("None");
    }
    else
    {
%>
    <table style="width: 100%" class="listTable">
     <thead>
      <% signupHelper.DrawHeader(); %>
     </thead>
     <tbody>
<%
        foreach (var signup in ranged)
        {
            signupHelper.DrawRow(signup, true, true);
        }
%>
     </tbody>
    </table>
<% } %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<!-- Queued Signups -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td><b>Queued Signups</b></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>
<%
    if (0 == ViewBag.NumQueued)
        Response.Write("None");
    else
    {
%>
    <table style="width: 100%" class="listTable">
     <thead>
      <% signupHelper.DrawHeader(); %>
     </thead>
     <tbody>
<%
        List<RaidSignup> queued = ViewBag.RaidDetails.Signups;

        foreach (var signup in queued.FindAll(rs => !rs.IsCancelled && !rs.IsRostered))
        {
         signupHelper.DrawRow(signup, true, true);
        }
%>
     </tbody>
    </table>
<% } %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<!-- Cancelled Signups -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td><b>Cancelled Signups</b></td>
  </tr>
 </thead>
 <tbody>
  <tr>
   <td>
<%
    if (0 == ViewBag.NumCancelled)
    {
        Response.Write("None");
    }
    else
    {
%>
    <table style="width: 100%" class="listTable">
     <thead>
      <tr>
       <td><b>Name</b></td>
       <td><b>Comment</b></td>
       <td><b>Level</b></td>
       <td><b>Race</b></td>
       <td><b>Class</b></td>
       <td><b>Signup Date/Time</b></td>
       <td><b>Role</b></td>
       <td></td>
      </tr>
     </thead>
     <tbody>
<%
        List<RaidSignup> queued = ViewBag.RaidDetails.Signups;

        foreach (var signup in queued.FindAll(rs => rs.IsCancelled))
        {
            var character = Character.Store.ReadOneOrDefault(c => c.Name == signup.Character);
            var primarySpec = Specialization.Store.ReadOneOrDefault(s => s.ID == character.PrimarySpecialization);
%>
      <tr>
       <td><%: character.Name%></td>
       <td><%: signup.Comment %></td>
       <td><%: character.Level %></td>
       <td><%: character.Race %></td>
       <td><%: character.Class %></td>
       <td><%: signup.SignupDate.ToShortDateString() + " " + signup.SignupDate.ToShortTimeString() %></td>
       <td><%: primarySpec.Role %></td>
       <td>
        <a href="#" id="Restore<%: character.Name %>" class="drmRestoreSignupButton" title="Restore this signup" onclick="return false;"><img src="/Content/images/revert-icon.png" alt="" /></a>
        <a href="#" id="Delete<%: character.Name %>" class="drmDeleteSignupButton" title="Delete this signup" onclick="return false;"><img src="/Content/images/delete-icon.png" alt="" /></a>
       </td>
      </tr>
<% } %>
     </tbody>
    </table>
<% } %>
   </td>
  </tr>
 </tbody>
</table>

<br />

<%
    if (Manager.IsReallyAuthenticated(Request))
    {
        using (Html.BeginForm("NewSignup", "Raid", FormMethod.Post, new { id = "signupForm" }))
        { %>

<input type="hidden" id="RaidInstanceID" name="RaidInstanceID" value="<%: ViewBag.RaidDetails.ID %>" />

<!-- Signup Form -->
<table style="width: 100%" class="listTable">
 <thead>
  <tr style="text-align: center">
   <td colspan="2"><b>Sign up for this Raid</b></td>
  </tr>
 </thead>
 <tbody>
<%
            var characters = Character.Store.ReadAll(c => (c.AccountID == Manager.GetCurrentUser().ID) && (c.Level >= ViewBag.RaidDetails.Raid.MinimumLevel));

            if (null != characters)
            {
%>
  <tr>
   <td style="width: 25%"><b>Display Name</b></td>
   <td><%: Manager.GetCurrentUser().FirstName%></td>
  </tr>
  <tr>
   <td><b>Character</b></td>
   <td>
    <select id="Character" name="Character">
<%
                foreach (var character in characters)
                {
                    Response.Write(string.Format(@"<option value=""{0}"">{0}</option>", character.Name));
                }
%>
    </select>
   </td>
  </tr>
  <tr>
   <td><b>Comment</b></td>
   <td><input type="text" id="Comment" name="Comment" maxlength="200" value="" /></td>
  </tr>
  <tr>
   <td></td>
   <td><input type="button" id="Signup" name="Signup" value="Sign Up" onclick="$('#signupForm').submit();" /></td>
  </tr>
<%
            }
            else
            {
%>
    <tr>
     <td>You either do not have any characters that meet the minimum level requirement, or you have to characters. Visit the characters menu option to remedy this.</td>
    </tr>
<% } %>
 </tbody>
</table>

<% 
        }
    }
%>

</asp:Content>
