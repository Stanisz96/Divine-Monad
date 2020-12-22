using DivineMonad.Data;
using DivineMonad.Engine;
using DivineMonad.Engine.Raport;
using DivineMonad.Models;
using DivineMonad.Tools;
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

        public GameController(ApplicationDbContext context, ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo)
        {
            _context = context;
            _baseStatsRepo = baseStatsRepo;
            _characterItemsRepo = characterItemsRepo;
            _itemsStatsRepo = itemsStatsRepo;
        }


        public async Task<IActionResult> Index([FromForm, Bind("cId")] int cId)
        {
            var character = await Validate.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                ViewData["menu"] = "character";
                return View(character);
            }
            else return RedirectToAction("Index", "Characters");
        }

/*        [HttpPost]
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
        }*/

        public IActionResult Character(int cId)
        {
            var characterItems = _characterItemsRepo.GetCharactersItemsList(cId, true);
            List<int> isIds = characterItems.Result.Select(i => i.ItemId).ToList();

            CharacterBaseStats baseStats = _baseStatsRepo.GetStatsById(1).Result;
            IEnumerable<ItemStats> itemStatsList = _itemsStatsRepo.GetListStatsByIds(isIds).Result;

            var characterAdvanceStats = new AdvanceStats();
            characterAdvanceStats.IsPlayer = true;
            characterAdvanceStats.CharacterId = cId;
            characterAdvanceStats.CalculateWithoutEq(baseStats);
            characterAdvanceStats.CalculateWithEq(itemStatsList);
            return View(characterAdvanceStats);
        }

        public async Task<IActionResult> Battle(int cId)
        {
            Character character = await Validate.GetCharacter(cId, User, _context);
            return View(character);
        }

        public async Task<IActionResult> Raport(int cId)
        {
            Character character = await Validate.GetCharacter(cId, User, _context);
            return View(character);
        }

        public async Task<IActionResult> Backpack(int cId)
        {
            Character character = await Validate.GetCharacter(cId, User, _context);
            return View(character);
        }

        public async Task<IActionResult> Market(int cId)
        {
            Character character = await Validate.GetCharacter(cId, User, _context);
            return View(character);
        }

        public async Task<IActionResult> News(int cId)
        {
            Character character = await Validate.GetCharacter(cId, User, _context);
            return View(character);
        }

    }
}
