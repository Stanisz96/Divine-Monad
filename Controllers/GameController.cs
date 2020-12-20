using DivineMonad.Data;
using DivineMonad.Engine;
using DivineMonad.Engine.Raport;
using DivineMonad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Controllers
{
    [Authorize(Policy = "user")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICharacterBaseStatsRepo _baseStatsRepo;
        private readonly ICharacterItemsRepo _characterItemsRepo;
        private readonly IItemStatsRepo _itemsStatsRepo;
        //private readonly IFightGenerator _fightGenerator;

        public GameController(ApplicationDbContext context, ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo/*, IFightGenerator fightGenerator*/)
        {
            _context = context;
            _baseStatsRepo = baseStatsRepo;
            _characterItemsRepo = characterItemsRepo;
            _itemsStatsRepo = itemsStatsRepo;
            //_fightGenerator = fightGenerator;
        }


        public async Task<IActionResult> Index(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Character character = await _context.Characters.Include(c => c.CBStats)
                                        .Include(c => c.GStats)
                                        .FirstOrDefaultAsync(m => m.ID == id);
                ViewData["id"] = id;
                ViewData["menu"] = "character";

                if (userId.Equals(character.UserId)) return View(character);
                else return View("NotNice");
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        public IActionResult ReloadViewComponent(int _cId, int _bsId, string type)
        {
            if (type.Equals("game-character")) return ViewComponent("GameCharacter", new { cId = _cId, bsId = _bsId });
            else if (type.Equals("game-backpack")) return ViewComponent("GameBackpack");
            else if (type.Equals("game-battle")) return ViewComponent("GameBattle", new { cId = _cId, bsId = _bsId });
            else if (type.Equals("game-market")) return ViewComponent("GameMarket");
            else if (type.Equals("game-news")) return ViewComponent("GameNews");
            else return NotFound();
        }

        [HttpPost]
        public IActionResult ReloadViewComponent(string type)
        {
            if (type.Equals("game-battle"))
                return ViewComponent("GameBattle", new { test = true });
            else
                return NotFound();
        }

        [HttpPost]
        public string TestFight()
        {

            int cId = Int32.Parse(Request.Headers["Referer"].ToString().Split("Index/").Last());


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var character = _context.Characters.Where(c => (c.UserId == userId) && (c.ID == cId))
                .Include(c => c.CBStats).FirstOrDefault();

            var character2 = _context.Characters.Where(c => (c.UserId == userId) && (c.ID == 3))
                                    .Include(c => c.CBStats).FirstOrDefault();

            var characterItems = _characterItemsRepo.GetCharactersItemsList(cId, true);
            List<int> isIds = characterItems.Select(i => i.ItemId).ToList();

            var characterItems2 = _characterItemsRepo.GetCharactersItemsList(3, true);
            List<int> isIds2 = characterItems2.Select(i => i.ItemId).ToList();

            CharacterBaseStats baseStats = _baseStatsRepo.GetStatsById(character.CBStatsId);
            IEnumerable<ItemStats> itemStatsList = _itemsStatsRepo.GetListStatsByIds(isIds);

            CharacterBaseStats baseStats2 = _baseStatsRepo.GetStatsById(character2.CBStatsId);
            IEnumerable<ItemStats> itemStatsList2 = _itemsStatsRepo.GetListStatsByIds(isIds2);

            var player1 = new AdvanceStats();
            player1.IsPlayer = true;
            player1.CharacterId = cId;
            player1.CalculateWithoutEq(baseStats);
            player1.CalculateWithEq(itemStatsList);

            var player2 = new AdvanceStats();
            player2.IsPlayer = true;
            player2.CharacterId = 3;
            player2.CalculateWithoutEq(baseStats2);
            player2.CalculateWithEq(itemStatsList2);

            FightGenerator fg = new FightGenerator(player1, player2);
            RaportGenerator raportGenerator = fg.GenerateFight();

            string fightReport = JsonConvert.SerializeObject(raportGenerator, Formatting.Indented);

            System.IO.File.WriteAllText(@"wwwroot/raports/testRaport.json", fightReport);


            return fightReport;
        }
    }
}
