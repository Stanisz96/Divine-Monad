using DivineMonad.Data;
using DivineMonad.Engine;
using DivineMonad.Engine.Raport;
using DivineMonad.Models;
using DivineMonad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Controllers
{
    [Authorize(Policy = "user")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbContextHelper _contextHelper;
        private readonly ICharacterHelper _characterHelper;
        private readonly ICharacterBaseStatsRepo _baseStatsRepo;
        private readonly ICharacterItemsRepo _characterItemsRepo;
        private readonly IItemStatsRepo _itemsStatsRepo;
        private readonly IItemRepo _itemsRepo;
        private readonly IRarityRepo _rarityRepo;
        private readonly IWebHostEnvironment _hostingEnv;

        public GameController(ApplicationDbContext context, ICharacterBaseStatsRepo baseStatsRepo, ICharacterItemsRepo characterItemsRepo,
            IItemStatsRepo itemsStatsRepo, IItemRepo itemsRepo, IRarityRepo rarityRepo, IDbContextHelper contextHelper, ICharacterHelper characterHelper, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _baseStatsRepo = baseStatsRepo;
            _characterItemsRepo = characterItemsRepo;
            _itemsStatsRepo = itemsStatsRepo;
            _itemsRepo = itemsRepo;
            _rarityRepo = rarityRepo;
            _contextHelper = contextHelper;
            _characterHelper = characterHelper;
            _hostingEnv = hostEnv;
        }


        public async Task<IActionResult> Index([FromForm, Bind("cId")] int cId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                ViewData["menu"] = "character";
                ViewData["reqExp"] = _characterHelper.RequiredExperience(character.CBStats.Level);
                return View(character);
            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Character(int cId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                var characterItems = await _characterItemsRepo.GetCharactersItemsList(cId, true);
                List<int> itemsIds = characterItems.Select(i => i.ItemId).ToList();

                IEnumerable<Item> itemsList = await _itemsRepo.GetItemsList(itemsIds);
                var itemStatsList = itemsList.Select(i => i.Statistics).ToList();
                var characterAdvanceStats = new AdvanceStats();
                characterAdvanceStats.IsPlayer = true;
                characterAdvanceStats.CharacterId = cId;
                characterAdvanceStats.CalculateWithoutEq(character.CBStats);
                characterAdvanceStats.CalculateWithEq(itemStatsList);
                characterAdvanceStats.GameStats = character.GStats;

                return PartialView(characterAdvanceStats);

            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Battle(int cId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                var monsters = await _context.Monsters.ToListAsync();

                ViewData["cId"] = cId;
                return PartialView(monsters);
            }
            else return RedirectToAction("Index", "Characters");
        }

        public async Task<IActionResult> Raport(int cId, int? mId, bool? qf, bool isHistory, string raportName)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);
            RaportViewModel raportView = null;

            if (isHistory)
            {
                string a = _hostingEnv.WebRootPath;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string raportPath = Path.Combine(a, "data\\" + userId.ToString() + "\\" + character.Name + "\\raports" + "\\" + raportName);

                RaportGenerator fightRaport = null;

                using (StreamReader r = new StreamReader(raportPath))
                {
                    string raportJson = r.ReadToEnd();
                    fightRaport = JsonConvert.DeserializeObject<RaportGenerator>(raportJson);
                }

                Monster monster = await _context.Monsters.Where(m => m.ID == fightRaport.Opponent.ID).FirstOrDefaultAsync();

                raportView = new RaportViewModel()
                {
                    Player = character,
                    Opponent = monster,
                    Raport = fightRaport
                };

                ViewData["gold"] = "null";
                ViewData["level"] = "null";
                ViewData["exp"] = "null";
                ViewData["reqExp"] = "null";
            }
            else
            {
                var characterItemsEquipped = await _characterItemsRepo.GetCharactersItemsList(cId, true);
                List<int> itemsIds = characterItemsEquipped.Select(i => i.ItemId).ToList();

                IEnumerable<Item> itemsList = await _itemsRepo.GetItemsList(itemsIds);
                var itemStatsList = itemsList.Select(i => i.Statistics).ToList();

                AdvanceStats attacker = new AdvanceStats();
                attacker.IsPlayer = true;
                attacker.CharacterId = cId;
                attacker.CharacterName = character.Name;
                attacker.CalculateWithoutEq(character.CBStats);
                attacker.CalculateWithEq(itemStatsList);


                Monster monster = await _contextHelper.GetMonster((int)mId, _context);

                AdvanceStats defender = new AdvanceStats();
                defender.IsPlayer = false;
                defender.CharacterId = (int)mId;
                defender.CharacterName = monster.Name;
                defender.CalculateMonster(monster.MonsterStats);

                var monsterItemsIds = await _context.MonstersLoot.Where(i => i.MonsterId == monster.ID).Select(i => i.ItemId).ToListAsync();
                var monsterItems = await _itemsRepo.GetItemsList(monsterItemsIds);

                FightGenerator fight = new FightGenerator(attacker, defender, monster, monsterItems, _rarityRepo);
                RaportGenerator fightRaport = await fight.GenerateFight();
                fightRaport.QuickFight = (bool)qf;

                IEnumerable<CharacterItems> characterItems = await _context.CharactersItems.Where(i => i.CharacterId == character.ID).ToListAsync();

                CharacterItems newItem = null;
                (character, newItem) = _contextHelper.AssignRewards(fightRaport, character, _context, _characterHelper, characterItems);
                try
                {
                    if (!(newItem is null))
                    {
                        _context.Add(newItem);
                        await _context.SaveChangesAsync();
                    }
                    _context.Update(character);
                    await _context.SaveChangesAsync();

                    ViewData["gold"] = character.CBStats.Gold;
                    ViewData["level"] = character.CBStats.Level;
                    ViewData["exp"] = character.CBStats.Experience;
                    ViewData["reqExp"] = _characterHelper.RequiredExperience(character.CBStats.Level);
                }
                catch (Exception)
                {

                    throw;
                }
                string fightRaportJson = JsonConvert.SerializeObject(fightRaport, Formatting.Indented);

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string a = _hostingEnv.WebRootPath;
                string characterDataPath = Path.Combine(a, "data\\" + userId.ToString() + "\\" + character.Name + "\\raports");

                if (!Directory.Exists(characterDataPath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(characterDataPath);
                }

                string dateTime = DateTime.Now.Ticks.ToString();
                string resultVal = fightRaport.Result == "lose" ? "0" : fightRaport.Result == "win" ? "2" : "1";
                raportName = fightRaport.Player.ID.ToString() + "_" + fightRaport.Opponent.ID.ToString() + "_"
                    + (fightRaport.IsPvp ? 1 : 0).ToString() + "_" + resultVal + "_" + dateTime + ".json";
                System.IO.File.WriteAllText(Path.Combine(characterDataPath, raportName), fightRaportJson);

                raportView = new RaportViewModel()
                {
                    Player = character,
                    Opponent = monster,
                    Raport = fightRaport
                };
            }


            return PartialView(raportView);
        }

        public async Task<IActionResult> Backpack(int cId)
        {
            Backpack backpack = new Backpack();
            backpack.Character = await _contextHelper.GetCharacter(cId, User, _context);
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
            backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);

            return PartialView(backpack);
        }

        public async Task<IActionResult> Market(int cId)
        {
            MarketViewModel marketView = new MarketViewModel();
            marketView.Character = await _contextHelper.GetCharacter(cId, User, _context);
            marketView.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = marketView.CharacterItemsList.Select(i => i.ItemId).ToList();
            marketView.ItemsList = await _itemsRepo.GetItemsList(itemIds);
            marketView.MarketItems = await _context.Markets.Where(m => m.LevelMin <= marketView.Character.CBStats.Level &&
                                        m.LevelMax >= marketView.Character.CBStats.Level).Include(i => i.Item).ToListAsync();
            return PartialView(marketView);
        }

        public async Task<IActionResult> Raports(int cId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

            string a = _hostingEnv.WebRootPath;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string raportsPath = Path.Combine(a, "data\\" + userId.ToString() + "\\" + character.Name + "\\raports");
            DirectoryInfo di = new DirectoryInfo(raportsPath);
            FileInfo[] Files = di.GetFiles("*.json");

            if (Files.Length == 0) PartialView(null);

            RaportsViewModel raportsView = new RaportsViewModel();
            raportsView.RaportsNames = new List<string>();
            raportsView.MonstersList = new List<Monster>();

            for (int i = 0; i < Files.Length; i++)
            {
                string raportName = Files[i].Name;
                raportsView.RaportsNames.Add(raportName);
                string[] subRaportName = raportName.Split("_");
                if (raportsView.Character is null) raportsView.Character = character.ID == Int32.Parse(subRaportName[0]) ? character : null;
                Monster monster = await _context.Monsters.Where(m => m.ID == Int32.Parse(subRaportName[1])).FirstOrDefaultAsync();
                if (!raportsView.MonstersList.Contains(monster)) raportsView.MonstersList.Add(monster);
            }

            ViewData["cId"] = cId;

            raportsView.RaportsNames = raportsView.RaportsNames.OrderByDescending(r => Int64.Parse(r.Split("_")[4].Replace(".json", ""))).ToList();

            return PartialView(raportsView);
        }

        public async Task<object> SlotsChange(int cId, int from, int to, bool isEmpty)
        {
            bool valid = false;
            List<CharacterItems> characterItemsList = new List<CharacterItems>();
            Backpack backpack = new Backpack();
            backpack.Character = await _contextHelper.GetCharacter(cId, User, _context);
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
            List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
            backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);

            bool isOneItemChanged = true;
            string updateOption = "";

            if (isEmpty)
            {
                if (to < 7 && from >= 7) updateOption = "putOn";
                else if (from < 7) updateOption = "takeOff";
                else updateOption = "move";
            }
            else
            {
                isOneItemChanged = false;
                if (from >= 7 && to < 7) updateOption = "putOnAndChange";
                else if (to >= 7 && from < 7) updateOption = "takeOffAndChange";
                else if (from == to)
                {
                    isOneItemChanged = true;
                    updateOption = "nothing";
                }
                else if (from < 7 && to < 7)
                {
                    isOneItemChanged = true;
                    updateOption = "not valid";
                }
                else updateOption = "moveAndChange";
            }

            if (updateOption.Equals("putOn") || updateOption.Equals("putOnAndChange") || updateOption.Equals("takeOffAndChange"))
                valid = _contextHelper.CanPutItOn(from, to, backpack);
            else if (updateOption.Equals("takeOff") || updateOption.Equals("move"))
                valid = _contextHelper.CanMoveIt(from, to, backpack);
            else if (updateOption.Equals("moveAndChange"))
                valid = _contextHelper.CanChangeIt(from, to, backpack);
            else if (updateOption.Equals("nothing") || updateOption.Equals("not valid"))
                valid = false;

            if (valid)
            {
                characterItemsList = await _contextHelper.UpdateBpSlotsId(from, to, backpack, _context, updateOption);
                try
                {
                    if (isOneItemChanged) _context.Update(characterItemsList[0]);
                    else
                    {
                        _context.Update(characterItemsList[0]);
                        await _context.SaveChangesAsync();
                        _context.Update(characterItemsList[1]);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception) { throw; };
            }

            return new { from, to, valid, option = updateOption };
        }

        public async Task<IActionResult> ItemInfo(int cId, int? bpSlotId, int? iId)
        {
            Item item = null;

            if (bpSlotId != null)
            {
                Backpack backpack = new Backpack();
                backpack.Character = await _contextHelper.GetCharacter(cId, User, _context);
                backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);
                List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
                backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);


                item = backpack.ItemsList.FirstOrDefault
                    (i => i.ID == backpack.CharacterItemsList.
                    FirstOrDefault(i => i.BpSlotId == bpSlotId).ItemId);
            }
            if (iId != null)
            {
                item = await _itemsRepo.GetItemById((int)iId);
                item.Price *= 5;
            }

            return PartialView(item);
        }

        public async Task<object> DistributePoints(int cId, int dp, int sta, int str, int agi, int dex, int luc)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

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

        public async Task<object> Trade(int cId, int? bpSlotId, int? iId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);
            Item item = null;
            Backpack backpack = new Backpack();
            backpack.Character = character;
            backpack.CharacterItemsList = await _characterItemsRepo.GetCharactersItemsList(cId, false);

            if (bpSlotId != null)
            {
                List<int> itemIds = backpack.CharacterItemsList.Select(i => i.ItemId).ToList();
                backpack.ItemsList = await _itemsRepo.GetItemsList(itemIds);


                item = backpack.ItemsList.FirstOrDefault
                    (i => i.ID == backpack.CharacterItemsList.
                    FirstOrDefault(i => i.BpSlotId == bpSlotId).ItemId);

                character.CBStats.Gold += item.Price;
                CharacterItems removeCharacterItem = await _context.CharactersItems.Where(i => i.CharacterId == character.ID
                                                                    && i.ItemId == item.ID && i.BpSlotId == bpSlotId).FirstOrDefaultAsync();
                _context.CharactersItems.Remove(removeCharacterItem);
                _context.Characters.Update(character);
                await _context.SaveChangesAsync();

                return new { valid = true, bpSlotId, gold = character.CBStats.Gold };
            }
            else if (iId != null)
            {
                item = await _itemsRepo.GetItemById((int)iId);
                if (character.CBStats.Gold >= 5 * item.Price)
                {
                    character.CBStats.Gold -= 5 * item.Price;
                    CharacterItems newItem = new CharacterItems()
                    {
                        BpSlotId = _characterHelper.GetFirstEmptySlot(character, backpack.CharacterItemsList),
                        CharacterId = character.ID,
                        IsEquipped = false,
                        ItemId = item.ID
                    };
                    _context.CharactersItems.Add(newItem);
                    _context.Characters.Update(character);
                    await _context.SaveChangesAsync();

                    return new { valid = true, bpSlotId = newItem.BpSlotId, iId, gold = character.CBStats.Gold };
                }
                else return new { valid = false };
            }

            return new { valid = false };
        }



        public string Test()
        {
            Random random = new Random();
            string val = "";
            double temp = 0;
            for (int i = 0; i < 1000; i++)
            {
                temp = random.NextDouble();
                if (temp * 1000 <= 1) val += temp.ToString() + "\n";
            }
            return val;
        }
    }
}
