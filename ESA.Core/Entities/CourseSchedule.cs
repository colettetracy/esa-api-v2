﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ESA.Core
{
    public partial class CourseSchedule
    {
        public CourseSchedule()
        {
            CourseStudent = new HashSet<CourseStudent>();
        }

        public int Id { get; set; }
        public int CourseCalendarId { get; set; }
        public bool IsPrivate { get; set; }
        public short StudentsAllowed { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime Schedule { get; set; }
        public int Minutes { get; set; }

        public virtual CourseCalendar CourseCalendar { get; set; }
        public virtual ICollection<CourseStudent> CourseStudent { get; set; }
    }
}