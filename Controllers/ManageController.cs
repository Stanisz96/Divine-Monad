using DivineMonad.Data;
using DivineMonad.Models;
using DivineMonad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Controllers
{
    [Authorize(Policy = "user")]
    [Route("Characters/Manage/")]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbContextHelper _contextHelper;

        public ManageController(ApplicationDbContext context, IDbContextHelper contextHelper)
        {
            _context = context;
            _contextHelper = contextHelper;
        }

        public async Task<IActionResult> Index([FromForm, Bind("cId")] int cId)
        {
            Character character = await _contextHelper.GetCharacter(cId, User, _context);

            if (!(character is null))
            {
                return View(character);
            }
            else return RedirectToAction("Index", "Characters");
        }
    }
}
