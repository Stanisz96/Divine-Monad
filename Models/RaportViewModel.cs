using DivineMonad.Engine.Raport;

namespace DivineMonad.Models
{
    public class RaportViewModel
    {
        public RaportGenerator Raport { get; set; }
        public Character Player { get; set; }
        public Monster Opponent { get; set; }
    }
}
