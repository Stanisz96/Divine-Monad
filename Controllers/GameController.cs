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

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int id, string m)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Character character = await _context.Characters.Include(c => c.CBStats)
                                        .Include(c => c.GStats)
                                        .FirstOrDefaultAsync(m => m.ID == id);
                ViewData["id"] = id;
                if (m.Length == 0) ViewData["menu"] = "character";
                else ViewData["menu"] = m;

                if (userId.Equals(character.UserId)) return View(character);
                else return View("NotNice");
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        public IActionResult ReloadViewComponent(int? _cId, int? _bsId, string type)
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
        public string TestFight(AdvanceStats playerStats)
        {
            FightGenerator fightGenerator = new FightGenerator(playerStats, playerStats);
            fightGenerator.GenerateFight();

            RaportGenerator raportGenerator = new RaportGenerator();
            raportGenerator.Player = new Player() { ID = 1 };
            raportGenerator.Opponent = new Opponent() { ID = 3 };
            raportGenerator.IsPvp = true;
            raportGenerator.Result = "win";
            raportGenerator.Rounds = new List<Round>
            {
                new Round
                {
                    Number = 1,
                    Attacker = new Attacker() { Crit = false, Damage = 25, HP = 200, ID = 1, Miss = false },
                    Defender = new Defender() { Block = false, ID = 3, HP = 185, Receive = 15 }
                },
                new Round
                {
                    Number = 2,
                    Attacker = new Attacker() { Crit = true, Damage = 90, HP = 185, ID = 3, Miss = false },
                    Defender = new Defender() { Block = false, ID = 1, HP = 120, Receive = 80 }
                },
                new Round
                {
                    Number = 3,
                    Attacker = new Attacker() { Crit = false, Damage = 20, HP = 120, ID = 1, Miss = false },
                    Defender = new Defender() { Block = true, ID = 3, HP = 180, Receive = 5 }
                },
                new Round
                {
                    Number = 4,
                    Attacker = new Attacker() { Crit = true, Damage = 130, HP = 180, ID = 3, Miss = false },
                    Defender = new Defender() { Block = false, ID = 1, HP = 0, Receive = 120 }
                }
            };

            string stringjson = JsonConvert.SerializeObject(raportGenerator, Formatting.Indented);

            System.IO.File.WriteAllText(@"wwwroot/raports/testRaport.json", stringjson);


            return stringjson;
        }
    }
}
