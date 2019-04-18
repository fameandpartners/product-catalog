using Fame.Data.Models;

namespace Fame.Data.Repository
{
    public interface IWorkflowRepository : IBaseRepository<Workflow>
    {
        void DoWorkflowStep(WorkflowStep workflowStep);
    }
}