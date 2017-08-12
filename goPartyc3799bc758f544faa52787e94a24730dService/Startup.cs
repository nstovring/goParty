using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(goPartyc3799bc758f544faa52787e94a24730dService.Startup))]

namespace goPartyc3799bc758f544faa52787e94a24730dService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}