using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Web.Models
{
    public class ClassroomModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Building { get; set; }
    }
}
