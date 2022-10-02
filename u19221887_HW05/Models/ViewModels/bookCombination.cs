using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19221887_HW05.Models.ViewModels
{
    public class bookCombination
    {

        public int bookId { get; set; }
        public string name { get; set; }
        public int pagecount { get; set; }
        public int point { get; set; }
        public string authorSurname { get; set; }
        public int authorId { get; set; }
        public int typeId { get; set; }
        public string typeName { get; set; }

        public bool status { get; set; }

        public int totalBorrows { get; set; }
        public List<borrowsCombination> borrowedRecords { get; set; }
        public int studentId { get; set; }
    }
}