using Business.Interface;
using Cross.Cutting.Helper;
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
                await _jobBusiness.UpdateDataSet();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
