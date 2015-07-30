using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication11.Models
{
    public class Jobs
    {
        [Key]
        public string client { get; set; }
        [Display(Name="Job Number")]
        public string jobNumber { get; set; }
        [Display(Name = "Job Name")]
        public string jobName { get; set; }
        [Display(Name = "Due")]
        public string due { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
    }
}