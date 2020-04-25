using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobTimer.Web.Domain
{
    public interface IMobMessenger 
    {
        Task NextDriver(Member driver);
        Task MembersUpdated(IList<Member> members);
        Task Tock(Tick tick);
    }
}