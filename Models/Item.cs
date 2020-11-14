﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int StatisticsId { get; set; }
        public ItemStats Statistics { get; set; }
        public ItemCategory Category { get; set; }

    }
}
