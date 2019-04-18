using Fame.Data.Models;
using System.Collections.Generic;

namespace Fame.Service.Services
{
    public interface IWorkflowService : IBaseService<Workflow>
    {
        void TriggerWorkflowStep(WorkflowStep workflowStep);
        Dictionary<WorkflowStep, Workflow> GetWorkflow();
    }
}