using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReservationCheckWorker
{
    public class WorkerService : BackgroundService
    {
        public IServiceProvider Services { get; }
        public WorkerService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var reservationChecker =
                    scope.ServiceProvider
                        .GetRequiredService<ReservationChecker>();

                await reservationChecker.DoWork(stoppingToken);
            }
        }
    }
}
