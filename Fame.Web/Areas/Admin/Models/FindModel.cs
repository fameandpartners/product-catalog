using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Web.Areas.Admin.Models
{
    public class FindModel
    {
        [Required(ErrorMessage = "Please Add a PID")]
        public string PID { get; set; }
    }
}
