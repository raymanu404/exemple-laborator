﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emanuel_Caprariu_lab4.Data.Models
{
    public class OrderHeaderDbo
    {
        public int OrderId { get; set; }
        public string Address { get; set; }
        public float? Total { get; set; }
    }
}
