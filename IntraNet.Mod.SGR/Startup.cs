using Hangfire;
using IntraNet.Data.Context;
using IntraNet.Mod.SGR.Controllers;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(IntraNet.Mod.SGR.Startup))]
namespace IntraNet.Mod.SGR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //var db = new HangfireContext();
            //GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=192.168.0.25,1433;Initial Catalog=data_HANG;User Id=sa;Password=sa");  // Copy connection string

            //RecurringJob.AddOrUpdate(() => NotificaDemandadoAsync(), "00 7 * * *");
            //RecurringJob.AddOrUpdate(() => NotificaDemandadoAsync(), "25 12 * * *");
            //RecurringJob.AddOrUpdate(() => NotificaDemandadoAsync(), "30 17 * * *");

           // app.UseHangfireDashboard("/hangfire");
            //app.UseHangfireServer();
        }

        //public async System.Threading.Tasks.Task NotificaDemandadoAsync()
        //{
            //DamandadoReuController demandado_notifica = new DamandadoReuController();
            //await demandado_notifica.NotificaDemandadosEmail();
        //}

        public void NoticficaSolicitanteDemanda()
        {

        }

        public void NotificaResponsavelItem()
        {

        }
    }
}
