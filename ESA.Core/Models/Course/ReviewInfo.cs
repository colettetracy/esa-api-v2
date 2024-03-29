﻿using ESA.Core.Models.Student;

namespace ESA.Core.Models.Course
{
    public class ReviewInfo
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public int CourseStudentId { get; set; }

        public string Comment { get; set; } = string.Empty;

        public float Rating { get; set; }

        public DateTime LastUpdate { get; set; }

        public CalendarInfo CourseCalendar { set; get; }
        public StudentInfo CourseStudent { set; get; }
    }

    public class ReviewBaseInfo
    {
        public int CalendarId { get; set; }

        public int CourseStudentId { get; set; }

        public string Comment { get; set; } = string.Empty;

        public float Rating { get; set; }
    }
}
