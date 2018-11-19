using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    public class LoginView_Model
    {
        public User_Model user { get; set; }
        public Doctor_Model doctor { get; set; }
        public Staff_Model staff { get; set; }
    }
}
