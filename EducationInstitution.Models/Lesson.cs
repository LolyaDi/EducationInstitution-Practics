using System;

namespace EducationInstitution.Models
{
    public class Lesson: Entity
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
