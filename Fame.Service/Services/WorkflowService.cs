using System;
using System.Collections.Generic;
using System.Linq;
using Fame.Data.Extensions;
using Fame.Data.Models;
using Fame.Data.Repository;

namespace Fame.Service.Services
{

    public class WorkflowService : BaseService<Workflow>, IWorkflowService
    {
        private readonly IRepositories _repositories;
        private readonly IWorkflowRepository _workflowRepo;

        public WorkflowService(IRepositories repositories)
            : base(repositories.Workflow.Value)
        {
            _repositories = repositories;
            _workflowRepo = repositories.Workflow.Value;
        }

        public Dictionary<WorkflowStep, Workflow> GetWorkflow()
        {
            var workflows = new Dictionary<WorkflowStep, Workflow>();
            var workflowData = _workflowRepo.Get().ToList().ToDictionary(w => w.WorkflowStep, w => w);

            foreach (var workflowStep in (WorkflowStep[])Enum.GetValues(typeof(WorkflowStep)))
            {
                if (workflowData.ContainsKey(workflowStep))
                {
                    workflows.Add(workflowStep, workflowData[workflowStep]);
                } else
                {
                    workflows.Add(workflowStep, new Workflow { WorkflowStep = workflowStep, TriggeredDateTime = null });
                }
            }

            return workflows;
        }
        
        public void TriggerWorkflowStep(WorkflowStep workflowStep)
        {
            _workflowRepo.DoWorkflowStep(workflowStep);
        }
    }
}