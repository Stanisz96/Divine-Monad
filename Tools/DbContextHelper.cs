using DivineMonad.Data;
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

        public static bool CanPutItOn(int from, int to, Backpack backpack)
        {
            try
            {
                if(from > to)
                {
                    CharacterItems cItem = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                    Item item = backpack.ItemsList.FirstOrDefault(i => i.ID == cItem.ItemId);
                    Character character = backpack.Character;

                    CategoryName itemCategory = (CategoryName)Enum.Parse(typeof(CategoryName), item.Category.Name, true);

                    return (int)itemCategory == to;
                }
                else
                {
                    CharacterItems cItem = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == to);
                    Item item = backpack.ItemsList.FirstOrDefault(i => i.ID == cItem.ItemId);
                    Character character = backpack.Character;

                    CategoryName itemCategory = (CategoryName)Enum.Parse(typeof(CategoryName), item.Category.Name, true);

                    return (int)itemCategory == to;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool CanMoveIt(int from, int to, Backpack backpack)
        {
            try
            {
                CharacterItems fromItem = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                CharacterItems toItem = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == to);
                if (fromItem is null) return false;
                else
                {
                    if (toItem is null && to >= 7) return true;
                    else return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool CanChangeIt(int from, int to, Backpack backpack)
        {
            try
            {
                CharacterItems fromItem = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                CharacterItems toItem = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == to);
                if (fromItem is null) return false;
                else
                {
                    if (toItem is null) return false;
                    else return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static async Task<List<CharacterItems>> UpdateBpSlotsId(int from, int to, Backpack backpack, ApplicationDbContext context, string option)
        {
            CharacterItems cItemFrom = null;
            CharacterItems cItemTo = null;

            if(option.Equals("putOn"))
            {
                cItemFrom = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                if (!(cItemFrom is null))
                {
                    cItemFrom.BpSlotId = to;
                    cItemFrom.IsEquipped = true;
                }
            }
            else if(option.Equals("takeOff"))
            {
                cItemFrom = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                if (!(cItemFrom is null))
                {
                    cItemFrom.BpSlotId = to;
                    cItemFrom.IsEquipped = false;
                }
            }
            else if (option.Equals("move"))
            {
                cItemFrom = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                if (!(cItemFrom is null))
                {
                    cItemFrom.BpSlotId = to;
                }
            }
            else if (option.Equals("putOnAndChange"))
            {
                cItemFrom = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                cItemTo = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == to);
                if (!(cItemFrom is null) && !(cItemTo is null))
                {
                    cItemFrom.BpSlotId = to;
                    cItemFrom.IsEquipped = true;
                    cItemTo.BpSlotId = from;
                    cItemTo.IsEquipped = false;
                }
            }
            else if (option.Equals("takeOffAndChange"))
            {
                cItemFrom = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                cItemTo = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == to);
                if (!(cItemFrom is null) && !(cItemTo is null))
                {
                    cItemFrom.BpSlotId = to;
                    cItemFrom.IsEquipped = false;
                    cItemTo.BpSlotId = from;
                    cItemTo.IsEquipped = true;
                }
            }
            else if (option.Equals("moveAndChange"))
            {
                cItemFrom = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == from);
                cItemTo = backpack.CharacterItemsList.FirstOrDefault(i => i.BpSlotId == to);
                if (!(cItemFrom is null) && !(cItemTo is null))
                {
                    cItemFrom.BpSlotId = to;
                    cItemTo.BpSlotId = from;
                }
            }

            try
            {
                if (!(cItemFrom is null))
                {
                    context.Update(cItemFrom);
                    await context.SaveChangesAsync();
                }

                if (!(cItemTo is null))
                {
                    context.Update(cItemTo);
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            List<CharacterItems> returnCItems = new List<CharacterItems>{
                cItemFrom,
                cItemTo
            };

            return returnCItems;

        }

        enum CategoryName
        {
            Helmet = 1,
            Gloves = 2,
            MeleeWeapon1H = 3,
            MeleeWeapon2H = 3,
            Bows = 3,
            Armor = 4,
            Arrows = 5,
            Shields = 5,
            Boots = 6
        }
    }
}
