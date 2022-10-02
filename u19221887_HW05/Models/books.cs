using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19221887_HW05.Models
{
    public class books
    {
        //create data types 

        public int bookId { get; set; }

        public string name { get; set; }
        public int pagecount { get; set; }
        public int point { get; set; }
        public int authorId { get; set; }
        public int typeId { get; set; }
    }
}