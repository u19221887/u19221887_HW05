using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19221887_HW05.Models
{
    public class borrows
    {
        //create data types 

        public int borrowId { get; set; }

        public int studentId { get; set; }

        public int bookId { get; set; }

        public DateTime takenDate { get; set; }

        public DateTime broughtDate { get; set; }
    }
}