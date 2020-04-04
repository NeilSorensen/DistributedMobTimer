using Microsoft.VisualBasic.CompilerServices;

namespace MobTimer.Web.Domain
{
    public class Member
    {
        public string DisplayName { get; set; }

        public override bool Equals(object? other)
        {
            var otherMember = other as Member;
            return otherMember != null && otherMember.DisplayName == DisplayName;
        }
    }
}
