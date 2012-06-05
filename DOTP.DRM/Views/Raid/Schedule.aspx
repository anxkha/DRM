<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DOTP.DRM.Models.ScheduleRaidModel>" %>
<%@ Import Namespace="DOTP.RaidManager" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DRM - Raids - Schedule</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js" type="text/javascript"></script>
 <script src="http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
 <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.20/jquery-ui.min.js" type="text/javascript"></script>
 <script src="/Scripts/ScheduleRaid.js" type="text/javascript"></script>
 <link type="text/css" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.20/themes/humanity/jquery-ui.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="StatusContent" ContentPlaceHolderID="StatusContent" runat="server">
 <div class="errorPanel" id="errorPanel" style="display: none;">
  <h2>Unable to schedule the raid. Please correct the error and try again:</h2>
  <p id="errorContent"></p>
 </div>
 <div class="successPanel" id="successPanel" style="display: none">
  The raid was successfully scheduled. <%: Html.ActionLink("Go back to the raid list.", "Index", "Home") %>
 </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<h2>Schedule a Raid</h2>

<% using( Html.BeginForm("Schedule", "Raid", FormMethod.Post, new { id = "scheduleForm" }) ) { %>

<fieldset>
 <legend>Raid Details</legend>

 <div class="editor-label">
  <%: Html.LabelFor(m => m.Raid) %>
 </div>
 <div id="drmRaids" class="editor-field">
  <select id="Raids" name="Raids">
<%
    foreach(var r in Raid.Store.ReadAll())
    {
        Response.Write(string.Format(@"<option value=""{0}""{1}>{0}</option>", r.Name, (null != Model ? (r.Name == Model.Raid ? @"selected=""selected""" : "") : "")));
    }
%>
  </select>
 </div>

 <div class="editor-label">
  <%: Html.LabelFor(m => m.Name) %>
 </div>
 <div id="drmRaidInstanceName" class="editor-field">
  <input data-val="true" data-val-required="A raid instance name is required" id="Name" name="Name" type="text" value="<% if (null != Model) Response.Write(Model.Name); %>" maxlength="100" />
  <span class="field-validation-valid" data-valmsg-for="Name" data-valmsg-replace="true"></span>
 </div>

 <div class="editor-label">
  <%: Html.LabelFor(m => m.Description) %>
 </div>
 <div id="drmRaidInstanceDescription" class="editor-field">
  <textarea data-val="true" data-val-required="A raid instance description is required" id="Description" name="Description" maxlength="1000"><% if (null != Model) Response.Write(Model.Description); %></textarea>
  <span class="field-validation-valid" data-valmsg-for="Description" data-valmsg-replace="true"></span>
 </div>

 <div class="editor-label">
  <label for="Date">Raid Date</label>
 </div>
 <div id="drmRaidInstanceDate" class="editor-field">
  <input data-val="true" data-val-required="A raid date is required" id="Date" name="Date" type="text" value="<% if (null != Model) Response.Write(Model.InviteTime.ToShortDateString()); %>" maxlength="20" />
  <span class="field-validation-valid" data-valmsg-for="Date" data-valmsg-replace="true"></span>
 </div>

 <div class="editor-label">
  <%: Html.LabelFor(m => m.InviteTime) %>
 </div>
 <div id="drmRaidInstanceInviteTime" class="editor-field">
  <select id="InviteTimeHour" name="InviteTimeHour">
<%
       for (int i = 0; i < 24; i++)
       {
           Response.Write(string.Format(@"<option value=""{1}{0}""{2}>{1}{0}</option>", i, i < 10 ? "0" : "", (null != Model ? (i == Model.InviteTime.Hour ? @"selected=""selected""" : "") : "")));
       }
%>
  </select>
  <select id="InviteTimeMinute" name="InviteTimeMinute">
<%
       for (int i = 0; i < 60; i += 15)
       {
           Response.Write(string.Format(@"<option value=""{1}{0}""{2}>{1}{0}</option>", i, i < 10 ? "0" : "", (null != Model ? (i == Model.InviteTime.Minute ? @"selected=""selected""" : "") : "")));
       }
%>
  </select>
 </div>

 <div class="editor-label">
  <%: Html.LabelFor(m => m.StartTime) %>
 </div>
 <div id="drmRaidInstanceStartTime" class="editor-field">
  <select id="StartTimeHour" name="StartTimeHour">
<%
       for (int i = 0; i < 24; i++)
       {
           Response.Write(string.Format(@"<option value=""{1}{0}""{2}>{1}{0}</option>", i, i < 10 ? "0" : "", (null != Model ? (i == Model.StartTime.Hour ? @"selected=""selected""" : "") : "")));
       }
%>
  </select>
  <select id="StartTimeMinute" name="StartTimeMinute">
<%
       for (int i = 0; i < 60; i += 15)
       {
           Response.Write(string.Format(@"<option value=""{1}{0}""{2}>{1}{0}</option>", i, i < 10 ? "0" : "", (null != Model ? (i == Model.StartTime.Minute ? @"selected=""selected""" : "") : "")));
       }
%>
  </select>
 </div>

 <p>
  <input type="submit" value="Schedule" />
 </p>
</fieldset>

<% } %>

</asp:Content>
