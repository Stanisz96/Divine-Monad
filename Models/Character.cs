using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DivineMonad.Models
{
    public class Character
    {
        public int ID { get; set; }

        [RegularExpression(@"[a-zA-Z0-9\s]*$"),
            Required,
            StringLength(13, MinimumLength = 4)]
        [Remote(
            action: "IsNameUnique",
            controller: "Characters",
            AdditionalFields = nameof(ID))]
        public string Name { get; set; }

        public string UserId { get; set; }

        public int CBStatsId { get; set; }
        public CharacterBaseStats CBStats { get; set; }

        public int GStatsId { get; set; }
        public GameStats GStats { get; set; }

        public string AvatarUrl { get; set; }

        [NotMapped]
        public IFormFile AvatarImage { get; set; }

        public Character()
        {
        }
    }
}
