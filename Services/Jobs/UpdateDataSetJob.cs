using Business.Interface;
using Domain.Helper;
using Quartz;

namespace Services.Jobs
{
    public class UpdateDataSetJob : IJob
    {
        private readonly IJobBusiness _jobBusiness;
        public UpdateDataSetJob(IJobBusiness jobBusiness)
        {
            _jobBusiness = jobBusiness;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var success = await _jobBusiness.UpdateDataSet();
                if (success)
                    _jobBusiness.GenerateJobLog(true, SuccessMessages.UpdateJob);
                else
                    _jobBusiness.GenerateJobLog(false, ErrorMessages.UpdateJobError);

            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
