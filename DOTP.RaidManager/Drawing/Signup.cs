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

        public void DrawRow(RaidSignup signup, bool canCancel, bool canDelete, bool canRestore, bool canChangeSpec)
        {
            var character = Character.Store.ReadOneOrDefault(c => c.Name == signup.Character);
            int specializationId = 1 == signup.RosteredSpecialization ? character.PrimarySpecialization : character.SecondarySpecialization;
            var specialization = Specialization.Store.ReadOneOrDefault(spec => spec.ID == specializationId);
            string specializationMarkup = canChangeSpec ? DrawSpecializationDropDown(character, signup.RosteredSpecialization) : specialization.Name;

            Response.Write("<tr>");
            Response.Write(string.Format("<td>{0}</td>", character.Name));
            Response.Write(string.Format("<td>{0}</td>", signup.Comment));
            Response.Write(string.Format("<td>{0}</td>", character.Level.ToString()));
            Response.Write(string.Format("<td>{0}</td>", character.Race));
            Response.Write(string.Format("<td>{0}</td>", character.Class));
            Response.Write(string.Format("<td>{0}</td>", signup.SignupDate.ToShortDateString() + " " + signup.SignupDate.ToShortTimeString()));
            Response.Write(string.Format("<td>{0}</td>", specialization.Role));
            Response.Write(string.Format("<td>{0}</td>", specializationMarkup));

            Response.Write("<td>");
            if (canCancel)
            {
                Response.Write(string.Format("<a href=\"#\" id=\"Cancel{0}\" class=\"drmCancelSignupButton\" title=\"Cancel this signup\" onclick=\"return false;\">", character.Name));
                Response.Write("<img src=\"/Content/images/cancel-icon.png\" alt=\"\" /></a>");
            }
            if (canRestore)
            {
                Response.Write(string.Format("<a href=\"#\" id=\"Restore{0}\" class=\"drmRestoreSignupButton\" title=\"Restore this signup\" onclick=\"return false;\">", character.Name));
                Response.Write("<img src=\"/Content/images/revert-icon.png\" alt=\"\" /></a>");
            }
            if (canDelete)
            {
                Response.Write(string.Format("<a href=\"#\" id=\"Delete{0}\" class=\"drmDeleteSignupButton\" title=\"Delete this signup\" onclick=\"return false;\">", character.Name));
                Response.Write("<img src=\"/Content/images/delete-icon.png\" alt=\"\" /></a>");
            }
            Response.Write("</td>");

            Response.Write("</tr>");
        }

        private string DrawSpecializationDropDown(Character character, int rosteredSpecialization)
        {
            string markup;

            int firstSpecializationId = 1 == rosteredSpecialization ? character.PrimarySpecialization : character.SecondarySpecialization;
            var firstSpecialization = Specialization.Store.ReadOneOrDefault(spec => spec.ID == firstSpecializationId);
            int secondSpecializationId = 1 == rosteredSpecialization ? character.SecondarySpecialization : character.PrimarySpecialization;
            var secondSpecialization = Specialization.Store.ReadOneOrDefault(spec => spec.ID == secondSpecializationId);

            markup = string.Format(@"<select id=""{0}Specialization"" name=""{0}Specialization"" class=""drmSpecializationDropDown"">", character.Name);
            markup += string.Format(@"<option value=""{1}"" selected=""selected"">{0}</option>", firstSpecialization.Name, rosteredSpecialization);

            if(35 != secondSpecializationId)
                markup += string.Format(@"<option value=""{1}"">{0}</option>", secondSpecialization.Name, (1 == rosteredSpecialization ? 2 : 1));

            markup += "</select>";

            return markup;
        }
    }
}
