using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Web;

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
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                .StartAt(DateTimeOffset.Parse("02:00 AM"))
                //.StartNow()
                //.WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
                .Build();

            sched.ScheduleJob(job, trigger);
        }
    }

    public class SimpleJob : IJob
    {

        void IJob.Execute(IJobExecutionContext context)
        {
            //DeleteContentMediaFolder();
        }

        private void DeleteContentMediaFolder()
        {
            string pathToMediaFolder = Path.Combine(HttpRuntime.AppDomainAppPath, "media");
            var di = new DirectoryInfo(pathToMediaFolder);

            foreach (var dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}