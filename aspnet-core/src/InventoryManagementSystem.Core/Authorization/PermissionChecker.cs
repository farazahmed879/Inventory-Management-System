using Abp.Authorization;
using InventoryManagementSystem.Authorization.Roles;
using InventoryManagementSystem.Authorization.Users;

namespace InventoryManagementSystem.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
