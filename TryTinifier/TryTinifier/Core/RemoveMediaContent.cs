using Quartz;
using Quartz.Impl;

namespace TryTinifier.Core
{
    public class RemoveMediaContent
    {
        public static void Start()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            IJobDetail job = JobBuilder.Create<SimpleJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                //.StartAt(DateTimeOffset.Parse("01:00:00 AM"))
                //.WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
                .Build();

            sched.ScheduleJob(job, trigger);
        }
    }

    public class SimpleJob : IJob
    {

        void IJob.Execute(IJobExecutionContext context)
        {
            DeleteContentMediaFolder();
        }

        private void DeleteContentMediaFolder()
        {

        }
    }
}