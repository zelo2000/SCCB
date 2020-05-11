using Microsoft.AspNetCore.Mvc.Rendering;
using SCCB.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Model for approve booking page.
    /// </summary>
    public class BookingFilter
    {
        public DateTime? Date { get; set; }

        public int? LessonNumber { get; set; }

        public Guid? ClassroomId { get; set; }

        public SelectList Classrooms { get; set; }
    }
}
