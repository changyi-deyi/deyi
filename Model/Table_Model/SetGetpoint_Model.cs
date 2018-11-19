using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class SetGetpoint_Model
    {
        public string IntroText { get; set; }
        public decimal SingleReward { get; set; }
        public int Cycle { get; set; }
        public decimal CycleReward { get; set; }
        public decimal UploadReward { get; set; }
        public decimal JoinReward { get; set; }
    }
}
