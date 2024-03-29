﻿using System.Collections.Generic;

namespace apbd3.Entities
{
    public class Studies
    {
        public Studies()
        {
            Enrollment = new HashSet<Enrollment>();
        }

        public int IdStudy { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Enrollment> Enrollment { get; set; }
    }
}