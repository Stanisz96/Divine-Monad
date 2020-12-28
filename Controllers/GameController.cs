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
        private readonly IItemRepo _itemsRepo;

        public GameController(ApplicationDbContext context, ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo, IItemRepo itemsRepo)
        {
            _context = context;
            _baseStatsRepo = baseStatsRepo;
            _characterItemsRepo = characterItemsRepo;
            _itemsStatsRepo = itemsStatsRepo;
            _itemsRepo = itemsRepo;
        }


        public async Task<IActionResult> Index([FromForm, Bind("cId")] int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                ViewData["menu"] = "character";
                return View(character);
            }
            else return RedirectToAction("Index", "Characters");
        }

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
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                var monsters = await _context.Monsters.ToListAsync();

                ViewData["cId"] = cId;
                return View(monsters);
            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Raport(int cId, int mId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);

            var characterItems =  await _characterItemsRepo.GetCharactersItemsList(cId, true);
            List<int> isIds = characterItems.Select(i => i.ItemId).ToList();

            IEnumerable<ItemStats> itemStatsList = await _itemsStatsRepo.GetListStatsByIds(isIds);

            AdvanceStats attacker = new AdvanceStats();
            attacker.IsPlayer = true;
            attacker.CharacterId = cId;
            attacker.CharacterName = character.Name;
            attacker.CalculateWithoutEq(character.CBStats);
            attacker.CalculateWithEq(itemStatsList);


            Monster monster = await DbContextHelper.GetMonster(mId, _context);

            AdvanceStats defender = new AdvanceStats();
            defender.IsPlayer = false;
            defender.CharacterId = mId;
            defender.CharacterName = monster.Name;
            defender.CalculateMonster(monster.MonsterStats);

            
            FightGenerator fight = new FightGenerator(attacker, defender);
            RaportGenerator fightRaport = fight.GenerateFight();

            string fightRaportJson = JsonConvert.SerializeObject(fightRaport, Formatting.Indented);

            System.IO.File.WriteAllText(@"wwwroot/raports/testRaport.json", fightRaportJson);



            return View(fightRaport);
        }

        public async Task<IActionResult> Backpack(int cId)
        {
            Backpack backpack = new Backpack();
            backpack.Character = await DbContextHelper.GetCharacter(cId, User, _context);
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
            backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);

            return View(backpack);
        }

        public async Task<IActionResult> Market(int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);
            return View(character);
        }

        public async Task<IActionResult> News(int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);
            return View(character);
        }

        public object SlotsChange(int from, int to, bool isEmpty)
        {
            if(isEmpty)
            {
                /* return "Move item: " + from.ToString() + " -> " + to.ToString();*/
                return new { from, to, valid = true };
            }
            else
            {
                /*return "Change items: " + from.ToString() + " <-> " + to.ToString();*/
                return new { from, to, valid = true };
            }
        }
    }
}
