using SCCB.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Web.Models
{
    public class UsersForGroupModel
    {
        public Guid GroupId { get; set; }

        public IEnumerable<UserProfile> Users { get; set; }
    }
}
