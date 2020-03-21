using Abp.AutoMapper;
using InventoryManagementSystem.Authentication.External;

namespace InventoryManagementSystem.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
