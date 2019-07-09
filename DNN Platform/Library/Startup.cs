using System;
using DotNetNuke.DependencyInjection;
using DotNetNuke.Entities.Portals;
using DotNetNuke.UI.Modules;
using DotNetNuke.UI.Modules.Html5;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetNuke
{
    public class Startup : IDnnStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<WebFormsModuleControlFactory>();
            services.AddSingleton<Html5ModuleControlFactory>();
            services.AddSingleton<ReflectedModuleControlFactory>();

            ConfigureEntityServices(services);
        }

        private void ConfigureEntityServices(IServiceCollection services)
        {
            services.AddSingleton<IPortalStylesController, PortalStylesController>();
        }
    }
}
