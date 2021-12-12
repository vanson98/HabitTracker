using Abp.Authorization;
using HabitTracker.Authorization.Roles;
using HabitTracker.Authorization.Users;

namespace HabitTracker.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
