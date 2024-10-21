using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicMe.Core.Entities
{
    public class Root
    {
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
