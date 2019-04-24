using System;

namespace EducationInstitution.Models
{
    public class Group: Entity
    {
        public string Name { get; set; }

        public Guid StudentId { get; set; }

        public Student Student { get; set; }

        public Guid LecturerId { get; set; }

        public Lecturer Lecturer { get; set; }
    }
}
