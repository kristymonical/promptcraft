using System;
using SVT.Core;
using SVT.Extensions.Tasks;

namespace SVT.Platform.Services
{
    public class SchedulingService : TimedSoftBot
    {
        public SchedulingService() : base()
        {
            IterationWaitTime = TimeSpan.FromSeconds(60);
        }
        protected override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override TaskResult Initialize()
        {
            return base.Initialize();
        }
    }
}