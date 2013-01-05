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
                int specializationId = 1 == signup.RosteredSpecialization ? character.PrimarySpecialization : character.SecondarySpecialization;
                var specialization = Specialization.Store.ReadOneOrDefault(spec => spec.ID == specializationId);
                var checkboxClass = showRostered ? "drmRosteredCharacterCheckbox" : "drmQueuedCharacterCheckbox";
                string specializationMarkup = showRostered ? specialization.Name : DrawSpecializationDropDown(character, signup.RosteredSpecialization);

                Response.Write(string.Format("<tr class=\"{0}\">", rowClass));
                Response.Write("<td>" + character.Name + "</td>");
                Response.Write("<td>" + specialization.Role + "</td>");
                Response.Write("<td>" + character.Class + "</td>");
                Response.Write("<td>" + character.Race + "</td>");
                Response.Write("<td>" + specializationMarkup + "</td>");
                Response.Write("<td>" + signup.SignupDate.ToShortDateString() + " " + signup.SignupDate.ToShortTimeString() + "</td>");
                Response.Write(string.Format(@"<td><input type=""checkbox"" class=""{0}"" name=""{1}"" /></td>", checkboxClass, character.Name));
                Response.Write("</tr>");

                numberDrawn++;
            }

            return numberDrawn;
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

            if (35 != secondSpecializationId)
                markup += string.Format(@"<option value=""{1}"">{0}</option>", secondSpecialization.Name, (1 == rosteredSpecialization ? 2 : 1));

            markup += "</select>";

            return markup;
        }
    }
}
