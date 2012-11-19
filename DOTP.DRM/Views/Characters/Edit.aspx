<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DOTP.DRM.Models.EditCharacterModel>" %>
<%@ Import Namespace="DOTP.RaidManager" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">DRM - Characters - Edit</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js" type="text/javascript"></script>
 <script src="http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
 <script src="/Scripts/EditCharacter.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="StatusContent" ContentPlaceHolderID="StatusContent" runat="server">
 <div class="errorPanel" id="errorPanel" style="display: none;">
  <h2>Unable to modify your character. Please correct the error and try again:</h2>
  <p id="errorContent"></p>
 </div>
 <div class="successPanel" id="successPanel" style="display: none">
  Your character was successfully modified. <%: Html.ActionLink("Go back to your character list.", "Index", "Characters") %>
 </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<p><%: Html.ActionLink("Go back to your character list.", "Index", "Characters") %></p>

<h2>Edit a Character</h2>

<% using( Html.BeginForm("Edit", "Characters", FormMethod.Post, new { id = "editForm" }) ) { %>

<div>
 <fieldset>
  <legend>Character Details</legend>

  <input type="hidden" id="OldName" name="OldName" value="<% if (null != Model) { if (Model.OldName != Model.Name) Response.Write(Model.OldName); else Response.Write(Model.Name); } %>" />

  <div class="editor-label">
   <%: Html.LabelFor(m => m.Name) %>
  </div>
  <div class="editor-field">
   <input data-val="true" data-val-required="A character name is required" id="Name" name="Name" type="text" value="<% if (null != Model) Response.Write(Model.Name); %>" maxlength="12" />
   <span class="field-validation-valid" data-valmsg-for="Name" data-valmsg-replace="true"></span>
  </div>

  <div class="editor-label">
   <%: Html.LabelFor(m => m.Level) %>
  </div>
  <div class="editor-field">
   <input data-val="true" data-val-required="A character level is required" data-val-number="The character level must be a number" data-val-range="The character level must be between 1 and 90" data-val-range-max="90" data-val-range-min="1" id="Level" name="Level" type="text" value="<% if (null != Model) Response.Write(Model.Level); %>" maxlength="2" />
   <span class="field-validation-valid" data-valmsg-for="Level" data-valmsg-replace="true"></span>
  </div>

  <div class="editor-label">
   <%: Html.LabelFor(m => m.Race) %>
  </div>
  <div id="drmRaces" class="editor-field">
    <select id="Race" name="Race">

<%
    foreach (var r in Race.Store.ReadAll())
    {
        if (null == RaceClasses.Store.ReadOneOrDefault(r.Name))
            continue;
%>

     <option value="<%: r.Name %>" <% if(r.Name == (Model == null ? "" : Model.Race)) Response.Write("selected=\"selected\""); %>><%: r.Name %></option>
         
<%                
    }
%>

    </select>
  </div>

  <div class="editor-label">
   <%: Html.LabelFor(m => m.Class) %>
  </div>
  <div id="drmClasses" class="editor-field">
<%
    foreach (var rc in RaceClasses.Store.ReadAll())
    {
        Response.Write(string.Format(@"<select id=""{0}Classes"" name=""{0}Classes"" style=""display: none;"">", rc.Race.Replace(" ", "")));
        
        var classes = rc.Classes;
        
        classes.Sort();
        
        foreach (var c in classes)
        {
            var m = null == Model ? "" : Model.Class;

            Response.Write(string.Format(@"<option value=""{0}""{1}>{0}</option>", c, m == c ? " selected=\"selected\"" : ""));
        }

        Response.Write("</select>");
    }
%>
  </div>

  <div class="editor-label">
   <%: Html.LabelFor(m => m.PrimarySpecialization) %>
  </div>
  <div class="editor-field drmSpecializations">
<%
    foreach (var c in Class.Store.ReadAll())
    {
        Response.Write(string.Format(@"<select id=""{0}PriSpec"" name=""{0}PriSpec"" style=""display: none;"">", c.Name.Replace(" ", "")));
        
        var specs = Specialization.Store.ReadAll(s => s.Class == c.Name);

        foreach (var spec in specs)
        {
            if (spec.Name == "None")
                continue;
            
            var m = null == Model ? 0 : Model.PrimarySpecialization;

            Response.Write(string.Format(@"<option value=""{0}""{1}>{2}</option>", spec.ID, (m == spec.ID ? " selected=\"selected\"" : ""), spec.Name));
        }

        Response.Write("</select>");
    }
%>
  </div>

  <div class="editor-label">
   <%: Html.LabelFor(m => m.SecondarySpecialization) %>
  </div>
  <div class="editor-field drmSpecializations">
<%
    foreach (var c in Class.Store.ReadAll())
    {
        Response.Write(string.Format(@"<select id=""{0}SecSpec"" name=""{0}SecSpec"" style=""display: none;"">", c.Name.Replace(" ", "")));

        var none = Specialization.Store.ReadOneOrDefault(s => s.Name == "None");
        
        Response.Write(string.Format(@"<option value=""{0}"">{1}</option>", none.ID, none.Name));
        
        var specs = Specialization.Store.ReadAll(s => s.Class == c.Name);

        foreach (var spec in specs)
        {
            if (spec.Name == "None")
                continue;
            
            var m = null == Model ? 0 : Model.SecondarySpecialization;

            Response.Write(string.Format(@"<option value=""{0}""{1}>{2}</option>", spec.ID, (m == spec.ID ? " selected=\"selected\"" : ""), spec.Name));
        }

        Response.Write("</select>");
    }
%>
  </div>

  <p>
   <input type="submit" value="Edit" />
  </p>
 </fieldset>
</div>
<% } %>

</asp:Content>


