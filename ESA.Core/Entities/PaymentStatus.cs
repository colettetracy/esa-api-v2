﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core.Entities
{
    public partial class PaymentStatus
    {
        public PaymentStatus()
        {
            Payment = new HashSet<Payment>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
    }
}