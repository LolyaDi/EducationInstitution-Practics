using System;

namespace EducationInstitution.Models
{
    public class Theme: Entity
    {
        public string Name { get; set; }
        
        public Guid SubjectId { get; set; }

        public Subject Subject { get; set; }
    }
}
