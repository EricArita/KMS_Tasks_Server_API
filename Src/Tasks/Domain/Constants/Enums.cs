using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Constants
{
    public class Enums
    {
        public enum UserRoles
        {
            Administrator = 0,
            Moderator = 1,
            User = 2
        }

        public enum ProjectRoles : byte
        {
            Owner = 0,
            PM = 1,
            Leader = 2,
            QA = 3,
            Dev = 4,
            BA = 5,
            Member = 6
        }

        public enum MenuSidebarOptions : byte
        {
            Dashboard = 0,
            Today = 1,
            Upcoming = 2
        }

        public enum LoginProvider : byte
        {
            System = 0,
            Facebook = 1,
            Google = 2,
        }
    }
}
