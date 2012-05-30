using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DOTP.DRM.Models
{
    public class AddCharacterModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [MaxLength(12)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Level")]
        [MaxLength(2)]
        public string Level { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Race")]
        public string Race { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Class")]
        public string Class { get; set; }

        [Display(Name = "Primary Specialization")]
        public int PrimarySpecialization { get; set; }

        [Display(Name = "Secondary Specialization")]
        public int SecondarySpecialization { get; set; }
    }

    public class EditCharacterModel
    {
        public string OldName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [MaxLength(12)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Level")]
        [MaxLength(2)]
        public string Level { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Race")]
        public string Race { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Class")]
        public string Class { get; set; }

        [Display(Name = "Primary Specialization")]
        public int PrimarySpecialization { get; set; }

        [Display(Name = "Secondary Specialization")]
        public int SecondarySpecialization { get; set; }
    }
}