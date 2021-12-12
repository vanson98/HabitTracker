using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}