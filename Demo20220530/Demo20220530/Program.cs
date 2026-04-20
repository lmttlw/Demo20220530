using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Demo20220530
{
    class Program
    {
        private static HttpHelper helper = null;
        private static string oauth_token = null;
        private static YY_token _Token = null;
        static void Main(string[] args)
        {
            helper = new HttpHelper();
            Init();
            if (Login("15573313867", "lw20010208"))
            {
                //{"code":"0","msg":"succ","obj":{"yyid":"2875814880","nickName":"2875814880yy","headPhotoUrl":"https://udbres.yy.com/aq/images/headerlogo/person/1.jpg"}}
                string res = GetUserInfo();
                YY_userinfo _Userinfo = JsonConvert.DeserializeObject<YY_userinfo>(res);
                Console.WriteLine("您的YY帳號信息：");
                Console.WriteLine($"呢稱：{_Userinfo.obj.nickName}");
                Console.WriteLine($"ID：{_Userinfo.obj.yyid}");
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        private static void Init()
        {
            string url = "https://aq.yy.com/";
            string result = helper.GetAndGetHtml(url, null, null, false, Encoding.UTF8);
            if (result.ToLower().Contains("<meta name"))
            {
                url = "https://aq.yy.com/p/wklogin.do?callbackURL=https://aq.yy.com/welcome.do";
                //{"success":"1","url":"https://lgn.yy.com/lgn/oauth/authorize.do?oauth_token=8a9e6d5bb772b84f6c0a438f8de5895ea5ba441d24722736cc1027e0027adb9bed5bb3c796e2c2acc6ba237b01f1cdd75ba22d283459bc4ae11d4bb13ae46b75","ttokensec":"9A2791D0112E477F68AA5A6D28A3A023F54CA426CF8837EBD24C048420927B6E72DCEA6E219EA8E349802C08A0A2ACA4","ttoken":"E7D3C6D27502BA0364FA6805CB4A8A73CB425BA6BD8AEB36107F5CCA5BDB3CC25041DA06DF79EF7568E2BAA613A20989C0938A0F25812BA7D9509DBD5C674DAF702ECCAA78474FA1BD0569E10E762FF3F8AA12C842405C9ABA4F4A03BC057C718FFA573C37806D2AD7DB58B093D791E938AA8852675D3D11761BFD814DAC964272DCEA6E219EA8E349802C08A0A2ACA4"}
                result = helper.PostAndGetHtml(url, "", null, null, false, Encoding.UTF8);
                _Token = JsonConvert.DeserializeObject<YY_token>(result);
                Regex regex = new Regex(@"oauth_token=(?<oauth_token>[^""]+)");
                oauth_token = regex.Match(result).Groups["oauth_token"].Value;
            }
        }
        private static bool Login(string name, string pwd)
        {

            string url = "https://lgn.yy.com/lgn/oauth/x2/s/login_asyn.do";
            string postData = string.Format("username={0}&pwdencrypt={1}&oauth_token={2}&denyCallbackURL=&UIStyle=xelogin&appid=1&cssid=1&mxc=&vk=&isRemMe=0&mmc=&vv=&hiido=1&baiduSecurityInfo=%7B%22data%22%3A%22d8d38dd56b2cf511e794c5dd407a16639348a2ea85c6c1a7d84745202cc526f0208b936afa2a555f91f09ccbac19f5c9ebc518cb270fdc3d1e7ebb8ce63331569af0870710e8321f804a5276983a843289f3135b3d97eeb9bff191518fa7af00%22%2C%22key_id%22%3A%2286%22%2C%22sign%22%3A%22613bc285%22%7D", name, JsTool.GetYYPwd(pwd), oauth_token);
            //{"code":"0","msg":null,"obj":{"callbackURL":"https://aq.yy.com/p/logincbk.do?jump=https://aq.yy.com/welcome.do&oauth_token=8a9e6d5bb772b84f6c0a438f8de5895ea5ba441d24722736cc1027e0027adb9b2002b6e3c99f67ae6db0be0900148a7114c16b6b76d1c8a6421cc031915a3031&oauth_verifier=8569d17d40e78565c713651e58f913eb&isRemMe=0","redirectURL":null,"vk":null,"vt":null,"pos":"1","verifyid":null,"qin":"up","yyuid":2876902836,"passport":"15573313867","svpic":null,"itvjs":null,"strategy":null},"hdcode":"0"}
            string result = helper.PostAndGetHtml(url, postData, null, null, false, Encoding.UTF8);
            if (result.Contains("\"code\":\"0\""))
            {
                YY_callback _Callback = JsonConvert.DeserializeObject<YY_callback>(result);
                //https://aq.yy.com/p/logincbk.do?jump=https://aq.yy.com/welcome.do&oauth_token=8a9e6d5bb772b84f6c0a438f8de5895ed6632af3f41b1ad1a4a20d4af4eb3414fec9de3f1d7a154a260e474ce119bee9c1dcd9c2b57a66d685f63d5690859e52&oauth_verifier=96ea8a6dca4f15c3c35a362df2cef336&isRemMe=0
                url = _Callback.obj.callbackURL;
                //<html><head><script language="JavaScript" type="text/javascript">function udb_callback(){self.parent.UDB.sdk.PCWeb.writeCrossmainCookieWithCallBack('https://lgn.yy.com/lgn/oauth/wck_n.do?oauth_mckey4cookie=e696c0f4d4e44049588b3fa4a48744364b79c415624b557aa7962eca3011245f1877b2869c57da07bb39d30714c549c7082fe362674d9c8c5c2618e97f6b4706&oauth_signature=ZolAlSCmJxL64Ru5h%2BXB9WHQVdk%3D',function(){self.parent.document.location="https://aq.yy.com/welcome.do";});};udb_callback();</script></head><body></body></html>
                helper.CC.Add(new System.Net.Cookie()
                {
                    Name = "udboauthtmptoken",
                    Value = _Token.ttoken,
                    Domain = "aq.yy.com",
                    Expires = DateTime.Now.AddDays(30)
                });
                helper.CC.Add(new System.Net.Cookie()
                {
                    Name = "udboauthtmptokensec",
                    Value = _Token.ttokensec,
                    Domain = "aq.yy.com",
                    Expires = DateTime.Now.AddDays(30)
                });
                result = helper.GetAndGetHtml(url, null, null, false, Encoding.UTF8);
                Regex regex = new Regex(@"writeCrossmainCookieWithCallBack\('(?<url>[^']+)");
                url = regex.Match(result).Groups["url"].Value;
                result = helper.GetAndGetHtml(url, null, null, false, Encoding.UTF8);
                if (result.Contains("write cookie for oauth"))
                {
                    return true;

                }
            }
            return false;
        }
        private static string GetUserInfo()
        {
            string url = "https://aq.yy.com/initUserInfo.do";
            string postData = "type=webdb";
            string result = helper.PostAndGetHtml(url, postData, null, null, false, Encoding.UTF8);
            return result;
        }
    }
}
