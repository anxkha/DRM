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
        Response.Write(string.Format(@"<option value=""{0}"">{0}</option>", r.Name));
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
   <option value="00">00</option>
   <option value="01">01</option>
   <option value="02">02</option>
   <option value="03">03</option>
   <option value="04">04</option>
   <option value="05">05</option>
   <option value="06">06</option>
   <option value="07">07</option>
   <option value="08">08</option>
   <option value="09">09</option>
   <option value="10">10</option>
   <option value="11">11</option>
   <option value="12">12</option>
   <option value="13">13</option>
   <option value="14">14</option>
   <option value="15">15</option>
   <option value="16">16</option>
   <option value="17">17</option>
   <option value="18">18</option>
   <option value="19">19</option>
   <option value="20">20</option>
   <option value="21">21</option>
   <option value="22">22</option>
   <option value="23">23</option>
  </select>
  <select id="InviteTimeMinute" name="InviteTimeMinute">
   <option value="00">00</option>
   <option value="15">15</option>
   <option value="30">30</option>
   <option value="45">45</option>
  </select>
 </div>

 <div class="editor-label">
  <%: Html.LabelFor(m => m.StartTime) %>
 </div>
 <div id="drmRaidInstanceStartTime" class="editor-field">
  <select id="StartTimeHour" name="StartTimeHour">
   <option value="00">00</option>
   <option value="01">01</option>
   <option value="02">02</option>
   <option value="03">03</option>
   <option value="04">04</option>
   <option value="05">05</option>
   <option value="06">06</option>
   <option value="07">07</option>
   <option value="08">08</option>
   <option value="09">09</option>
   <option value="10">10</option>
   <option value="11">11</option>
   <option value="12">12</option>
   <option value="13">13</option>
   <option value="14">14</option>
   <option value="15">15</option>
   <option value="16">16</option>
   <option value="17">17</option>
   <option value="18">18</option>
   <option value="19">19</option>
   <option value="20">20</option>
   <option value="21">21</option>
   <option value="22">22</option>
   <option value="23">23</option>
  </select>
  <select id="StartTimeMinute" name="StartTimeMinute">
   <option value="00">00</option>
   <option value="15">15</option>
   <option value="30">30</option>
   <option value="45">45</option>
  </select>
 </div>

 <p>
  <input type="submit" value="Schedule" />
 </p>
</fieldset>

<% } %>

</asp:Content>
