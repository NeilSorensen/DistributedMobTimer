using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MobTimer.Web.Domain;

namespace MobTimer.Web.Hubs
{
    public class MobMessenger : IMobMessenger
    {
        private readonly IHubContext<TimerHub> hubContext;

        public MobMessenger(IHubContext<TimerHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task NextDriver(Member driver)
        {
            await hubContext.Clients.All.SendAsync("NextDriver", driver);
        }
        public async Task MembersUpdated(IList<Member> members)
        {
            await hubContext.Clients.All.SendAsync("MembersUpdated", members);
        }
        public async Task Tock(Tick tick)
        {
            await hubContext.Clients.All.SendAsync("Tock", tick);
        }
    }
}