using System;

namespace EducationInstitution.Models
{
    public class Curriculum: Entity
    {
        public Guid LecturerId { get; set; }

        public Lecturer Lecturer { get; set; }

        public Guid GroupId { get; set; }

        public Group Group { get; set; }

        public Guid ThemeId { get; set; }

        public Theme Theme { get; set; }

        public DateTime Date { get; set; }
    }
}
