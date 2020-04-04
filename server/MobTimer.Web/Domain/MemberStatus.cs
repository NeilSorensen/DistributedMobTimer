namespace MobTimer.Web.Domain
{
    public class MemberStatus
    {
        public MemberStatus(Member member, Status status)
        {
            Member = member;
            Status = status;
        }

        public Member Member { get; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Mobbing,
        Afk
    }
}
