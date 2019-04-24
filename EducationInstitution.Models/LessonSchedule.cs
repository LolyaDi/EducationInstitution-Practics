using System;

namespace EducationInstitution.Models
{
    public class LessonSchedule: Entity
    {
        public Guid GroupId { get; set; }

        public Group Group { get; set; }

        public Guid AudienceId { get; set; }

        public Audience Audience { get; set; }

        public DayOfWeek Day { get; set; }

        public Guid LessonId { get; set; }

        public Lesson Lesson { get; set; }
    }
}
