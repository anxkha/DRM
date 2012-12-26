using System.Collections.Generic;
using System.Web;

namespace DOTP.RaidManager.Drawing
{
    public class Signup
    {
        public HttpResponse Response { get; set; }

        public void DrawHeader()
        {
            Response.Write("<tr>");
            Response.Write("<td><b>Name</b></td>");
            Response.Write("<td><b>Comment</b></td>");
            Response.Write("<td><b>Level</b></td>");
            Response.Write("<td><b>Race</b></td>");
            Response.Write("<td><b>Class</b></td>");
            Response.Write("<td><b>Signup Date/Time</b></td>");
            Response.Write("<td><b>Role</b></td>");
            Response.Write("<td><b>Roster As</b></td>");
            Response.Write("<td></td>");
            Response.Write("</tr>");
        }

        public void DrawRow(RaidSignup signup, bool canCancel, bool canDelete)
        {
            var character = Character.Store.ReadOneOrDefault(c => c.Name == signup.Character);
            var primarySpec = Specialization.Store.ReadOneOrDefault(s => s.ID == character.PrimarySpecialization);
            var secondarySpec = Specialization.Store.ReadOneOrDefault(s => s.ID == character.SecondarySpecialization);

            Response.Write("<tr>");
            Response.Write(string.Format("<td>{0}</td>", character.Name));
            Response.Write(string.Format("<td>{0}</td>", signup.Comment));
            Response.Write(string.Format("<td>{0}</td>", character.Level.ToString()));
            Response.Write(string.Format("<td>{0}</td>", character.Race));
            Response.Write(string.Format("<td>{0}</td>", character.Class));
            Response.Write(string.Format("<td>{0}</td>", signup.SignupDate.ToShortDateString() + " " + signup.SignupDate.ToShortTimeString()));
            Response.Write(string.Format("<td>{0}</td>", primarySpec.Role));
            
            Response.Write("<td>");
            if (null != secondarySpec)
            {
                Response.Write(string.Format("<select name=\"{0}RosterRole\" class=\"SpecSwitcher\" id=\"{0}RosterRole\">", character.Name));
                Response.Write(string.Format("<option value=\"{0}\">{1}</option>", primarySpec.ID.ToString(), primarySpec.Name));
                Response.Write(string.Format("<option value=\"{0}\">{1}</option>", secondarySpec.ID.ToString(), secondarySpec.Name));
            }
            Response.Write("</td>");

            Response.Write("<td>");
            if (canCancel)
            {
                Response.Write(string.Format("<a href=\"#\" id=\"Cancel{0}\" class=\"drmCancelSignupButton\" title=\"Cancel this signup\" onclick=\"return false;\">", character.Name));
                Response.Write("<img src=\"/Content/images/cancel-icon.png\" alt=\"\" />");
                Response.Write("</a>");
            }
            if (canDelete)
            {
                Response.Write(string.Format("<a href=\"#\" id=\"Delete{0}\" class=\"drmDeleteSignupButton\" title=\"Delete this signup\" onclick=\"return false;\">", character.Name));
                Response.Write("<img src=\"/Content/images/delete-icon.png\" alt=\"\" />");
                Response.Write("</a>");
            }
            Response.Write("</td>");

            Response.Write("</tr>");
        }
    }
}
