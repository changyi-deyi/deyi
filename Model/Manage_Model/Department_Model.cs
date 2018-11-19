﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
    public class Department_Model
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int Status { get; set; }
        public int UpperID { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string UpperName { get; set; }
    }
}
