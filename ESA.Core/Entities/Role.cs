﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core.Entities
{
    public partial class Role
    {
        public Role()
        {
            Account = new HashSet<Account>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}