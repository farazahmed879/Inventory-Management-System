using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Controllers
{
    public abstract class InventoryManagementSystemControllerBase: AbpController
    {
        protected InventoryManagementSystemControllerBase()
        {
            LocalizationSourceName = InventoryManagementSystemConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
