using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo20220530
{
    public class YY_callback
    {
        public string code { get; set; }
        public object msg { get; set; }
        public YY_callback_Obj obj { get; set; }
        public string hdcode { get; set; }
    }

    public class YY_callback_Obj
    {
        public string callbackURL { get; set; }
        public object redirectURL { get; set; }
        public object vk { get; set; }
        public object vt { get; set; }
        public string pos { get; set; }
        public object verifyid { get; set; }
        public string qin { get; set; }
        public long yyuid { get; set; }
        public string passport { get; set; }
        public object svpic { get; set; }
        public object itvjs { get; set; }
        public object strategy { get; set; }
    }
}
