using System;
using System.Collections.Generic;

namespace SCCB.Web.Models
{
    public class GroupOptionsModel
    {
        public Guid? GroupId { get; set; }

        public IEnumerable<GroupModel> Groups { get; set; }
    }
}
