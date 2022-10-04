﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core.Entities
{
    public partial class Course
    {
        public Course()
        {
            CourseCalendar = new HashSet<CourseCalendar>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string About { get; set; }
        public string Content { get; set; }
        public short DurationDays { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Price { get; set; }
        public byte[] Icon { get; set; }
        public bool? IsActive { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<CourseCalendar> CourseCalendar { get; set; }
    }
}