using System;
using System.Collections.Generic;
using System.Text;

namespace Fame.Data.Models
{
    public class Workflow
    {
        public WorkflowStep WorkflowStep { get;set; }

        public DateTimeOffset? TriggeredDateTime { get;set; }
    }

    public enum WorkflowStep { 
        ProductImport = 10,
        SearchIndex = 20,
        CurationsImport = 30,
        SilhouetteData = 40,
        FileSync = 50,
        Layering = 60,
        SpreeExport = 70
    }
}
