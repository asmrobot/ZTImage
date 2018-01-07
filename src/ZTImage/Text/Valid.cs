using System.Text.RegularExpressions;

namespace ZTImage.Text
{
    public sealed class Valid
    {


        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
                return false;

            if (strNumber.Length < 1)
                return false;

            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                    return false;
            }
            return true;
        }


        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|\*|!|\']");
        }

        /// <summary>
        /// 是否是日期字符串
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static bool IsDate(string dateString)
        {
            Regex reg = new Regex(@"^\d{4}-\d{1,2}-\d{1,2}\s*(\d{1,2}:\d{1,2}(:\d{1,2})?)?$", RegexOptions.IgnoreCase);
            return reg.IsMatch(dateString);
        }

        // 邮箱
        public static bool IsEmail(string email)
        {
            Regex reg = new Regex(@"^[\w\.\-]{1,32}@[\w\-\.]{1,30}\.[a-z]{2,8}$",RegexOptions.IgnoreCase);
            return reg.IsMatch(email);
        }

        // 密码
        public static bool IsPassword(string password)
        {
            Regex reg = new Regex(@"^[A-Za-z0-9]{6,32}$");
            return reg.IsMatch(password);
        }
        
        // 网址
        public static bool IsWebSite(string webSite)
        {
            Regex reg = new Regex(@"[\w\s\.\,\(\)\d]{7,256}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(webSite);
        }

        // 天线高度和距离
        public static bool IsData(string data)
        {
            Regex reg = new Regex(@"^[-+]?((\d{1,8})\.\d{1,2}|\d{1,11})$");
            return reg.IsMatch(data);
        }

        // 组织名称/公司名称
        public static bool IsOrgName(string input)
        {
            Regex reg = new Regex(@"^[A-Za-z0-9\-\u4E00-\u9FA5\s\(\-\.\@\&\)\（\）\,]{3,100}$");
            return reg.IsMatch(input);
        }

        // 联系人名称
        public static bool IsName(string input)
        {
            Regex reg = new Regex(@"^[A-Za-z\u4E00-\u9FA5\s]{2,100}$",RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 电话
        public static bool IsPhone(string input)
        {
            Regex reg = new Regex(@"^[\(\+\d\-\)]{10,25}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        public static bool IsMobilePhone(string input)
        {
            Regex reg = new Regex(@"^1\d{10,10}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 邮编
        public static bool IsZip(string input)
        {
            Regex reg = new Regex(@"^[a-z0-9]{6,10}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 城市名称
        public static bool IsCity(string input)
        {
            Regex reg = new Regex(@"^[a-z0-9\s]{2,40}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 传真
        public static bool IsFax(string input)
        {
            Regex reg = new Regex(@"^[\(\+\d\-\)]{10,25}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 地址
        public static bool IsAddress(string input)
        {
            Regex reg = new Regex(@"^[\(\w\s\-\u4E00-\u9FA5\-\,\.\)\@]{4,256}$", RegexOptions.IgnoreCase);
            return reg.IsMatch(input);
        }

        // 是否为ip
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }


        public static bool IsUserIDList(string ids)
        {
            return Regex.IsMatch(ids, @"^(\d{1,},?){1,}$");
        }
    }
}
