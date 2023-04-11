﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import.Categories
{
    [JsonObject]
    public class ImportCategoryDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
