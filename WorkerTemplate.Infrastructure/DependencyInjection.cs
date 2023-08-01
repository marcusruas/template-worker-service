using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WorkerTemplate.Infrastructure.Repositories.ExampleContext;

namespace WorkerTemplate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IExampleContextRepository, ExampleContextRepository>();
            return services;
        }
    }
}