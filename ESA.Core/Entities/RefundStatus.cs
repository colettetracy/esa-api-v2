﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core
{
    public partial class RefundStatus
    {
        public RefundStatus()
        {
            Refund = new HashSet<Refund>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Refund> Refund { get; set; }
    }
}