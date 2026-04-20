using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo20220530
{
    public class YY_userinfo
    {
        public string code { get; set; }
        public string msg { get; set; }
        public YY_userinfo_Obj obj { get; set; }
    }

    public class YY_userinfo_Obj
    {
        public string yyid { get; set; }
        public string nickName { get; set; }
        public string headPhotoUrl { get; set; }
    }
}
