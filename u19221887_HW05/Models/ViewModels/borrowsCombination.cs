using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19221887_HW05.Models.ViewModels
{
    public class borrowsCombination
    {
        public string studentName { get; set; }

        // nullable because it can be empty
        public Nullable<int> borrowId { get; set; }

        public Nullable<DateTime> takenDate { get; set; }

        public Nullable<DateTime> broughtDate { get; set; }
    }
}