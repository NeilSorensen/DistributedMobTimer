using Microsoft.VisualBasic.CompilerServices;

namespace MobTimer.Web.Domain
{
    public class Member
    {
        public string DisplayName { get; }

        public Member(string displayName)
        {
            DisplayName = displayName;
        }

        public override bool Equals(object other)
        {
            var otherMember = other as Member;
            return otherMember != null && otherMember.DisplayName == DisplayName;
        }

        public override int GetHashCode()
        {
            return DisplayName.GetHashCode();
        }
    }
}
