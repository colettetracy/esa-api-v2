﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core
{
    public partial class LessonsPack
    {
        public LessonsPack()
        {
            StudentPacks = new HashSet<StudentPacks>();
        }

        public int Id { get; set; }
        public int Hours { get; set; }
        public decimal Price { get; set; }
        public decimal Savings { get; set; }
        public bool IsActive { get; set; }
        public int Minutes { get; set; }

        public virtual ICollection<StudentPacks> StudentPacks { get; set; }
    }
}