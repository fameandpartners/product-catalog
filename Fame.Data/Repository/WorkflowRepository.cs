using Fame.Data.Extensions;
using Fame.Data.Models;
using System;

namespace Fame.Data.Repository
{
    public class WorkflowRepository : BaseRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(FameContext context) : base(context)
        {
        }

        public void DoWorkflowStep(WorkflowStep workflowStep)
        {
            var workflow = DbSet.Find(workflowStep);
            if (workflow == null)
            {
                DbSet.Add(new Workflow() { TriggeredDateTime = DateTime.UtcNow, WorkflowStep = workflowStep });
            } else
            {
                workflow.TriggeredDateTime = DateTime.UtcNow;
                DbSet.Update(workflow);
            }
        }
    }
}