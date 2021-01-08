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
                ViewData["reqExp"] = CharacterHelper.RequiredExperience(character.CBStats.Level);
                return View(character);
            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Character(int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
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
                characterAdvanceStats.GameStats = character.GStats;

                return PartialView(characterAdvanceStats);

            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Battle(int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                var monsters = await _context.Monsters.ToListAsync();

                ViewData["cId"] = cId;
                return PartialView(monsters);
            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Raport(int cId, int mId, bool qf)
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
            fightRaport.QuickFight = qf;

            string fightRaportJson = JsonConvert.SerializeObject(fightRaport, Formatting.Indented);

            System.IO.File.WriteAllText(@"wwwroot/raports/testRaport.json", fightRaportJson);

            return PartialView(fightRaport);
        }

        public async Task<IActionResult> Backpack(int cId)
        {
            Backpack backpack = new Backpack();
            backpack.Character = await DbContextHelper.GetCharacter(cId, User, _context);
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
            backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);

            return PartialView(backpack);
        }

        public async Task<IActionResult> Market(int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);
            return PartialView(character);
        }

        public async Task<IActionResult> News(int cId)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);
            return PartialView(character);
        }

        public async Task<object> SlotsChange(int cId, int from, int to, bool isEmpty)
        {
            bool valid = false;
            List<CharacterItems> characterItemsList = new List<CharacterItems>();
            Backpack backpack = new Backpack();
            backpack.Character = await DbContextHelper.GetCharacter(cId, User, _context);
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
            backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);


            if (isEmpty)
            {
                if (to < 7 && from >= 7)
                {
                    valid = DbContextHelper.CanPutItOn(from, to, backpack);
                    if (valid)
                    {
                        characterItemsList = await DbContextHelper.UpdateBpSlotsId(from, to, backpack, _context, "putOn");
                        try
                        {
                            _context.Update(characterItemsList[0]);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception) { throw; };
                    }
                        
                    return new { from, to, valid, option = "putOn" };
                }
                else if (from < 7)
                {
                    valid = DbContextHelper.CanMoveIt(from, to, backpack);
                    if (valid)
                    {
                        characterItemsList = await DbContextHelper.UpdateBpSlotsId(from, to, backpack, _context, "takeOff");
                        try
                        {
                            _context.Update(characterItemsList[0]);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception) { throw; };
                    }

                    return new { from, to, valid, option = "takeOff" };
                }
                else
                {
                    valid = DbContextHelper.CanMoveIt(from, to, backpack);
                    if (valid)
                    {
                        characterItemsList = await DbContextHelper.UpdateBpSlotsId(from, to, backpack, _context, "move");
                        try
                        {
                            _context.Update(characterItemsList[0]);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception) { throw; };
                    }

                    return new { from, to, valid, option = "move" };
                }
            }
            else
            {
                if (from >= 7 && to < 7)
                {
                    valid = DbContextHelper.CanPutItOn(from, to, backpack);
                    if (valid)
                    {
                        characterItemsList = await DbContextHelper.UpdateBpSlotsId(from, to, backpack, _context, "putOnAndChange");
                        try
                        {
                            _context.Update(characterItemsList[0]);
                            await _context.SaveChangesAsync();
                            _context.Update(characterItemsList[1]);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception) { throw; };
                    }

                    return new { from, to, valid, option = "putOnAndChange" }; 
                }
                else if (to >= 7 && from < 7)
                {
                    valid = DbContextHelper.CanPutItOn(from, to, backpack);
                    if (valid)
                    {
                        characterItemsList = await DbContextHelper.UpdateBpSlotsId(from, to, backpack, _context, "takeOffAndChange");
                        try
                        {
                            _context.Update(characterItemsList[0]);
                            await _context.SaveChangesAsync();
                            _context.Update(characterItemsList[1]);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception) { throw; };
                    }

                    return new { from, to, valid, option = "takeOffAndChange" };
                }
                else if (from == to)
                { 
                    return new { from, to, valid = true, option = "nothing" };
                }
                else if (from < 7 && to < 7)
                {
                    return new { from, to, valid = false, option = "not valid" };
                }
                else
                {
                    valid = DbContextHelper.CanChangeIt(from, to, backpack);
                    if (valid)
                    {
                        characterItemsList = await DbContextHelper.UpdateBpSlotsId(from, to, backpack, _context, "moveAndChange");
                        try
                        {
                            _context.Update(characterItemsList[0]);
                            await _context.SaveChangesAsync();
                            _context.Update(characterItemsList[1]);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception) { throw; };
                    }

                    return new { from, to, valid, option = "moveAndChange" };
                }
            }
        }

        public async Task<IActionResult> ItemInfo(int cId, int bpSlotId)
        {
            Backpack backpack = new Backpack();
            backpack.Character = await DbContextHelper.GetCharacter(cId, User, _context);
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
            backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);


            Item item = backpack.ItemsList.FirstOrDefault
                (i => i.ID == backpack.CharacterItemsList.
                FirstOrDefault(i => i.BpSlotId == bpSlotId).ItemId);

            return PartialView(item);
        }

        public async Task<object> DistributePoints(int cId, int dp, int sta, int str, int agi, int dex, int luc)
        {
            Character character = await DbContextHelper.GetCharacter(cId, User, _context);

            if (character is null) return new { valid = false };

            if (character.CBStats.StatsPoints >= dp)
            {
                character.CBStats.StatsPoints -= dp;
                character.CBStats.Stamina += sta;
                character.CBStats.Strength += str;
                character.CBStats.Agility += agi;
                character.CBStats.Dexterity += dex;
                character.CBStats.Luck += luc;

                try
                {
                    _context.Update(character.CBStats);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return new { valid = false };
                }

                return new { valid = true };
            }
            else return new { valid = false };
        }
    }
}
