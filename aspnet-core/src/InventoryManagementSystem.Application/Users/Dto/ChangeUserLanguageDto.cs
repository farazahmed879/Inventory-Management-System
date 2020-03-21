using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}