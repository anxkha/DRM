using System.Collections.Generic;
using System.Web;

namespace DOTP.RaidManager.Drawing
{
    public class Roster
    {
        public HttpResponse Response { get; set; }
        public List<RaidSignup> TankSignups { get; set; }
        public List<RaidSignup> HealerSignups { get; set; }
        public List<RaidSignup> RangedSignups { get; set; }
        public List<RaidSignup> MeleeSignups { get; set; }

        public int DrawTankRows(bool showRostered = false)
        {
            return DrawRow("tankRow", TankSignups, showRostered);
        }

        public int DrawHealerRows(bool showRostered = false)
        {
            return DrawRow("healerRow", HealerSignups, showRostered);
        }

        public int DrawRangedRows(bool showRostered = false)
        {
            return DrawRow("rangedRow", RangedSignups, showRostered);
        }

        public int DrawMeleeRows(bool showRostered = false)
        {
            return DrawRow("meleeRow", MeleeSignups, showRostered);
        }

        private int DrawRow(string rowClass, List<RaidSignup> signups, bool showRostered)
        {
            var numberDrawn = 0;

            foreach (var signup in signups)
            {
                if (signup.IsCancelled)
                    continue;
                if (signup.IsRostered && !showRostered)
                    continue;
                if (!signup.IsRostered && showRostered)
                    continue;

                var character = Character.Store.ReadOneOrDefault(c => c.Name == signup.Character);
                var specializationId = 1 == signup.RosteredSpecialization ? character.PrimarySpecialization : character.SecondarySpecialization;
                var specialization = Specialization.Store.ReadOneOrDefault(spec => spec.ID == specializationId);

                Response.Write(string.Format("<tr class=\"{0}\">", rowClass));
                Response.Write("<td>" + character.Name + "</td>");
                Response.Write("<td>" + specialization.Role + "</td>");
                Response.Write("<td>" + character.Class + "</td>");
                Response.Write("<td>" + character.Race + "</td>");
                Response.Write("<td>" + specialization.Name + "</td>");
                Response.Write("<td>" + signup.SignupDate.ToShortDateString() + " " + signup.SignupDate.ToShortTimeString() + "</td>");
                Response.Write(string.Format(@"<td><input type=""checkbox"" class=""characterCheckbox"" name=""{0}"" /></td>", character.Name));
                Response.Write("</tr>");

                numberDrawn++;
            }

            return numberDrawn;
        }
    }
}
