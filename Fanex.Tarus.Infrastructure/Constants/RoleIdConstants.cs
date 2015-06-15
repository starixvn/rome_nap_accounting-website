namespace Fanex.Tarus.Infrastructure.Constants
{
    using Fanex.Tarus.Infrastructure.Resource;

    public class RoleIdConstants
    {
        public const int Super = 4;
        public const int Master = 3;
        public const int Agent = 2;
        public const int Member = 1;

        public static string RoleName(int roleId)
        {
            switch (roleId)
            {
                case Super:
                    return Labels.Super;
                case Master:
                    return Labels.Master;
                case Agent:
                    return Labels.Agent;
                case Member:
                    return Labels.Member;
                default:
                    return string.Empty;
            }
        }
    }
}
