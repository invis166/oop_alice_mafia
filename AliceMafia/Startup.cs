using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliceMafia.Action;
using AliceMafia.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Ninject;

namespace AliceMafia
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            var container = new StandardKernel();
            container.Bind<RoleActionBase>().To<MafiaAction>().WhenInjectedInto<Mafia>();
            container.Bind<RoleActionBase>().To<ManiacAction>().WhenInjectedInto<Maniac>();
            container.Bind<RoleActionBase>().To<SheriffAction>().WhenInjectedInto<Sheriff>();
            container.Bind<RoleActionBase>().To<DoctorAction>().WhenInjectedInto<Doctor>();
            container.Bind<RoleActionBase>().To<CourtesanAction>().WhenInjectedInto<Courtesan>();
            container.Bind<IGame>().To<Game>();
        }
        
    }
}