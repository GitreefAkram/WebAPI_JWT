﻿using System;
using System.Collections.Generic;

namespace WebAPI_JWT.DataDB
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int? ProductCost { get; set; }
        public int? ProductStock { get; set; }
    }
}
