﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class SetMemberLevel_Model
    {
        public int LevelID { get; set; }
        public string Name { get; set; }
        public string IconURL { get; set; }
        public string Summary { get; set; }
        public int TermYears { get; set; }
        public decimal OriginPrice { get; set; }
        public decimal PromPrice { get; set; }
        public int LastLevelID { get; set; }
        public int NextLevelID { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
