using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MobTimer.Web.Domain;

namespace MobTimer.Web.Hubs
{
    public class TimerHub : Hub
    {        
        private IRoom room;
        public TimerHub(IRoom room)
        {
            this.room = room;
        }

        public async Task JoinMob(string displayName)
        {
            var member = new Member(displayName);
            room.JoinRoom(member, Context.ConnectionId);
            await Clients.All.SendAsync("MembersUpdated", room.GetMembers());
        }

        public async Task ShuffleMobbers()
        {
            room.ShuffleMembers();
            await Clients.All.SendAsync("MembersUpdated", room.GetMembers());   

        }

        public async Task StartDriving()
        {
            room.Start();
            await Clients.All.SendAsync("NextDriver", room.GetDriver());
        }

        public override async Task OnDisconnectedAsync(Exception exception) 
        {
            room.MemberDisconnected(Context.ConnectionId);
            await Clients.All.SendAsync("MembersUpdated", room.GetMembers());
        }
    }
}
