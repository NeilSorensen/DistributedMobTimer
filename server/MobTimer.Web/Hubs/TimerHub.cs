using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MobTimer.Web.Domain;

namespace MobTimer.Web.Hubs
{
    public class TimerHub : Hub
    {
        private IMob mob;
        private Room room;
        private ITimer timer;

        public TimerHub()
        {
            timer = new Timer();
            mob = new Mob();
            room = new Room(mob, timer);
            room.NextTurn += NotifyRoomNextTurn;
        }

        private void NotifyRoomNextTurn(Member driver)
        {
            Clients.All.SendAsync("NextDriver", driver.DisplayName);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinMob(string displayName)
        {
            var member = new Member(displayName);
            mob.Join(member);
            await Clients.All.SendAsync("MembersUpdated", mob.GetMembers());
        }

        public async Task StartDriving()
        {
            timer.Start();
            await Clients.All.SendAsync("StartTimer", timer.LengthInMilliseconds);
        }
    }
}
