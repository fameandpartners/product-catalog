using Fame.Data.Models;
using System.Collections.Generic;

namespace Fame.Web.Areas.Admin.Models
{
    public class WorkflowViewModel
    {
        public List<string> LayeringDropNames { get; set; }

        public Dictionary<WorkflowStep, Workflow> Workflow { get; set; }

        public List<string> SpreeDropNames { get; set; }
    }
}
