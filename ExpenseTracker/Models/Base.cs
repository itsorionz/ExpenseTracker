﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Base
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSynced { get; set; }
    }
}
