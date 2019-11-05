using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Web.Areas.Admin.Models
{
    public class FindModel
    {
        public FindModel(string _message=null)
        {
            Message = _message;
        }

        [Required(ErrorMessage = "Please Add a PID")]
        public string PID { get; set; }

        public string Message { get; set; }
    }
}
