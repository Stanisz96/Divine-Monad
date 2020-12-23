﻿using DivineMonad.Data;
using DivineMonad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DivineMonad.Tools
{
    public class DbContextHelper
    {
        public static async Task<Character> GetCharacter(int cId, ClaimsPrincipal user, ApplicationDbContext context)
        {
            string userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Character character = await context.Characters.Include(c => c.CBStats)
                                        .Include(c => c.GStats)
                                        .FirstOrDefaultAsync(c => c.ID == cId);

                if (userId.Equals(character.UserId)) return character;
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<Monster> GetMonster(int mId, ApplicationDbContext context)
        {
            try
            {
                Monster monster = await context.Monsters.Include(c => c.MonsterStats)
                                        .FirstOrDefaultAsync(c => c.ID == mId);

                return monster;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
