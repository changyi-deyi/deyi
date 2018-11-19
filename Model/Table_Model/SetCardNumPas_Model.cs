using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class SetCardNumPas_Model
    {
        public string CardNumber { get; set; }
        public string Passeord { get; set; }
        public int LevelID { get; set; }
        public int IsExchange { get; set; }
        public int Exchanger { get; set; }
        public int ChannelID { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
