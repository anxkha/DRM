using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace DOTP.DRM.Models
{
    public class ScheduleRaidModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Raid")]
        public string Raid { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Descriptive Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Invite Time")]
        public DateTime InviteTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
    }
}
