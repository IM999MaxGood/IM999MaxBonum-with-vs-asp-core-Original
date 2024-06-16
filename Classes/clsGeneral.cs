using System;
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using System.Linq;
//using System.Web.Mvc;
using System.Drawing;
using System.Net.Mail;
using System.Reflection;
//using System.Data.Entity;
using System.Data.Common;
using System.Drawing.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Security.Principal;
using System.Collections.Generic;
using System.Security.AccessControl;

//999/ for class SessionExtensions
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

//999/ for using in database region
using IM999MaxBonum.Models;

using Microsoft.EntityFrameworkCore;

//999/ for using color
//susing System.Drawing;

//999/ for using encrypt
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;


//using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

using IM999MaxBonum.Data;


namespace IM999MaxBonum.Classes
{
    public class LinkAddress
    {
        public string Name;
        public string Address;
    }

    public class clsGeneralProperty
    {
        public const string _SessionCurrentLang = "_CurrentLang";
        public const string _SessionCurrentUser = "_CurrentUser";

        public static string fromEmail = "sajima1994@gmail.com";
        public static string mailServer = "smtp.gmail.com";
        public static string fromName = "یونس منوچهری";
        public static string portMailServer = "587";
        public static string passwordMailserver = "5138814488";
        public static string defaultLanguage = "Fa";
        public static string companyPhone = "+98-5138814565";
        public static string companyEmail = "info@IM999MaxBonum.com";
        public static string Developer = "IM999MaxBonum";
        public static string DeveloperEmail = "iounes.manoochehri@outlook.com";

        //public static string FilePath = "~/App_Data/UserFiles/";
        //public static string FilePath = "~/UserFiles/";
        public static string FilePath = "\\UserFiles\\";
        public static string FilePathFull = "\\wwwroot\\UserFiles\\";
        public static string PageFilePathFull = "\\wwwroot\\UserFiles\\Pages\\";

        /*
        public static DateTime _TodayM;
        public static DateTime TodayM
        {
            get
            {
                if (_TodayM == null || _TodayM == new DateTime())
                    _TodayM = clsGeneralFunction.GetServerDateTime().Date;
                return _TodayM;
            }
        }
        */
    }

    public class Encryptor
    {
        private readonly IDataProtector _protector;

        public Encryptor(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(GetType().FullName);
        }

        public string Encrypt(string plaintext)
        {
            return _protector.Protect(plaintext);
        }

        public string Decrypt(string encryptedText)
        {
            return _protector.Unprotect(encryptedText);
        }
    }

    //999/for useing session
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            var data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }
    }


        #region session_in_form_and_db
        [Serializable]
        public class KeyValuePair<K,V>
        {
            public K Key {get;set;}
            public V Value {get;set;}
            public KeyValuePair(K key, V value){
                Key=key;
                Value=value;
            }
        }

        //999/ این بخش برای ذخیره سازی و مدیریت سشن خودم در فرم ایجاد شده مثل ویوفرم در ای.اس.پی دات نت
        public class SessionCookie{
            private List<KeyValuePair<string, string>> SC;
            public bool IsEmpty()
            {
                if(SC== null|| SC.Count() ==0)
                    return true;
                return false;
            }

            public void SetValue(string key, string value)
            {
                if(SC == null)
                    SC = new List<KeyValuePair<string, string>>();

                var a = SC.Where(x=> x.Key==key).FirstOrDefault();
                if(a==null)
                    SC.Add(new KeyValuePair<string, string>(key, value));
                else{
                    a.Value= value;
                }
            }
            
            public string GetValue(string key)
            {
                if(SC == null)
                    return null;

                var a = SC.Where(x=> x.Key==key).FirstOrDefault();
                if(a==null)
                    return null;

                return a.Value;                                     
            } 

            public void LoadSC(HttpContext context){
                //var data =  HttpContext.Request.Cookies["SF"];
                var data =  context.Request.Cookies["SC"];
                if (data == null)
                {
                    SC = null;
                } else {
                    SC =  Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>( clsGeneralFunction.BackFromHtml(data));
                }
            }

            public string GetSC(){
                //return clsGeneralFunction.EncryptAndSerializeToHtml(SF);
                return clsGeneralFunction.SerializeAndEncryptAndToHtml(SC);
            }
        }

        //999/ این بخش برای ذخیره سازی و مدیریت سشن خودم در بانک داده ایجاد شده مثل کلاس سشن در ای.اس.پی دات نت
        public class SessionDb{
            private List<KeyValuePair<string, string>> SD;
            public bool IsEmpty()
            {
                if(SD== null|| SD.Count() ==0)
                    return true;
                return false;
            }

            public void SetValue(string key, string value)
            {
                if(SD == null)
                    SD = new List<KeyValuePair<string, string>>();

                var a = SD.Where(x=> x.Key==key).FirstOrDefault();
                if(a==null)
                    SD.Add(new KeyValuePair<string, string>(key, value));
                else{
                    a.Value= value;
                }
            }
            
            public string GetValue(string key)
            {
                if(SD == null)
                    return null;

                var a = SD.Where(x=> x.Key==key).FirstOrDefault();
                if(a==null)
                    return null;

                return a.Value;                                     
            } 

            public object LoadSD(LoginLog ll){
                if (ll == null || ll.SD == null)
                {
                    return null;
                }
                //ll = clsLoginLog.GetLoginLogByGuid(ll.GUID);
                SD = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>( ll.SD);
                return SD;
            }

            public LoginLog SaveSD(LoginLog ll){
                if(SD == null)
                    return ll;
                var s = Newtonsoft.Json.JsonConvert.SerializeObject(SD);
                return clsLoginLog.SaveSessionDB(ll, s);
            }
        }
        #endregion session_in_form_and_db

    public static class clsGeneralFunction
    {
        #region calender
        public static DateTime ConvertShamsiToMiladi(string date)
        {
            if (date == "")
                return new DateTime();

            PersianCalendar pc = new PersianCalendar();
            return pc.ToDateTime(Convert.ToInt16(date.Substring(0, 4)), Convert.ToInt16(date.Substring(5, 2)), Convert.ToInt16(date.Substring(8, 2)), 0, 0, 0, 0, 0);
        }

        public static CultureInfo GetPersianCulture()
        {
            CultureInfo culture = new CultureInfo("fa");
            //CultureInfo culture = new CultureInfo("fa-IR");
            DateTimeFormatInfo info = culture.DateTimeFormat;
            info.AbbreviatedDayNames = new String[] { "ى", "د", "س", "چ", "پ", "ج", "ش" };
            info.DayNames = new String[] { "يکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه", "شنبه" };
            info.AbbreviatedMonthNames = new String[] { "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن", "اسفند", "" };
            info.MonthNames = new String[] { "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن", "اسفند", "" };
            info.MonthGenitiveNames = new String[] { "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن", "اسفند", "" };
            info.AMDesignator = "ق.ظ";
            info.PMDesignator = "ب.ظ";
            info.ShortDatePattern = "yyyy/MM/dd";
            info.FirstDayOfWeek = DayOfWeek.Saturday;
            culture.DateTimeFormat = info;
            //culture.DateTimeFormat = info;
            PersianCalendar PersianCal = new PersianCalendar();
            typeof(DateTimeFormatInfo).GetField("calendar", (BindingFlags.NonPublic | (BindingFlags.Public | BindingFlags.Instance))).SetValue(info, PersianCal);
            typeof(CultureInfo).GetField("calendar", (BindingFlags.NonPublic | (BindingFlags.Public | BindingFlags.Instance))).SetValue(culture, PersianCal);


            NumberFormatInfo infoNum = culture.NumberFormat;
            infoNum.NativeDigits = new string[] { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
            infoNum.CurrencyDecimalSeparator = "/";
            infoNum.CurrencySymbol = "ريال";
            infoNum.DigitSubstitution = DigitShapes.NativeNational;

            culture.NumberFormat = infoNum;

            //numberFormatInfoReadOnly = typeof(NumberFormatInfo).GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            //typeof(CultureInfo).GetField("NumberFormat", (BindingFlags.NonPublic | (BindingFlags.Public | BindingFlags.Instance))).SetValue(culture, infoNum);
            return culture;
        }

         public static string GetPersianMonth(int month) 
        {
            string name = "";
            switch (month)
            {
                case 1:
                    name = "فروردین";
                    break;
                case 2:
                    name = "اردیبهشت";
                    break;
                case 3:
                    name = "خرداد";
                    break;
                case 4:
                    name = "تیر";
                    break;
                case 5:
                    name = "مرداد";
                    break;
                case 6:
                    name = "شهریور";
                    break;
                case 7:
                    name = "مهر";
                    break;
                case 8:
                    name = "آبان";
                    break;
                case 9:
                    name = "آذر";
                    break;
                case 10:
                    name = "دی";
                    break;
                case 11:
                    name = "بهمن";
                    break;
                case 12:
                    name = "اسفند";
                    break;
            }
            return name;
        }
        #endregion calender 
    
        #region database
        public static object ExecSqlAndGetReturn(DbContext Db, string storedProcName, List<KeyValuePair<string, string>> paramsNameValue)
        {
            if(storedProcName.IndexOf("dbo.")==-1)
                storedProcName = "dbo."+storedProcName;
                
            var ps = " ";
            var i = 0;
            foreach(var kv in paramsNameValue)
            {
                i++;
                ps+= "@"+kv.Key.Trim()+"="+kv.Value.Trim();
                if(i<paramsNameValue.Count){
                    ps+=", ";
                }
            }

            SqlParameter[] @params = 
            {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };

            Db.Database.ExecuteSqlCommand("exec @returnVal=" + storedProcName + ps, @params);
            
            return @params[0].Value;
        }
        #endregion database

        #region color
        /*
        public static System.Drawing.Color FromHex(string hex)
        {
            FromHex(hex, out var a, out var r, out var g, out var b);

            return Color.FromArgb(a, r, g, b);
        }

        public static void FromHex(string hex, out byte a, out byte r, out byte g, out byte b)
        {
            hex = ToRgbaHex(hex);
            if (hex == null || !uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var packedValue))
            {
                throw new ArgumentException("Hexadecimal string is not in the correct format.", nameof(hex));
            }

            a = (byte) (packedValue >> 0);
            r = (byte) (packedValue >> 24);
            g = (byte) (packedValue >> 16);
            b = (byte) (packedValue >> 8);
        }


        private static string ToRgbaHex(string hex)
        {
            hex = hex.StartsWith("#") ? hex.Substring(1) : hex;

            if (hex.Length == 8)
            {
                return hex;
            }

            if (hex.Length == 6)
            {
                return hex + "FF";
            }

            if (hex.Length < 3 || hex.Length > 4)
            {
                return null;
            }

            //Handle values like #3B2
            string red = char.ToString(hex[0]);
            string green = char.ToString(hex[1]);
            string blue = char.ToString(hex[2]);
            string alpha = hex.Length == 3 ? "F" : char.ToString(hex[3]);


            return string.Concat(red, red, green, green, blue, blue, alpha, alpha);
        }
        */
        #endregion color

        #region رمزنگاری

        public static string Encrypt(string data)
        {
            if(data==null)
                return null;

            //use data protection services
            var SCollection = new ServiceCollection();
            //add protection services
            SCollection.AddDataProtection();
            var SerPro = SCollection.BuildServiceProvider();
            // create an instance of classfile using 'CreateInstance' method
            var instance = ActivatorUtilities.CreateInstance<Encryptor>(SerPro);

            //data = instance.Decrypt(data.Replace("cbabc", ";"));
            return instance.Encrypt(data);
        }

        public static string Decrypt(string data)
        {
            if(data==null)
                return null;

            //use data protection services
            var SCollection = new ServiceCollection();
            //add protection services
            SCollection.AddDataProtection();
            var SerPro = SCollection.BuildServiceProvider();
            // create an instance of classfile using 'CreateInstance' method
            var instance = ActivatorUtilities.CreateInstance<Encryptor>(SerPro);

            //data = instance.Decrypt(data.Replace("cbabc", ";"));
            return instance.Decrypt(data);
        }

        public static string SerializeAndEncryptAndToHtml(object obj)
        {   
            if(obj == null)
                return null;

            var s = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var e = Encrypt(s);
            var h = clsGeneralFunction.ReadyToSaveInHtml(s);
            return h;

/*        
            var e = Encrypt(data);
            object sTo = Convert.ChangeType(e, typeof(object));
            var s = Newtonsoft.Json.JsonConvert.SerializeObject(sTo);
            //var r = s.Replace("\"","abcba");
            var r = clsGeneralFunction.ReadyToSaveInHtml(s);
            return r;
*/
        }

        public static string DeserializeAfterDecryptAfterFromHtml(string cryptedData)
        {
            if(cryptedData == null)
                return null;
            
            var h = clsGeneralFunction.BackFromHtml(cryptedData);
            var d = Decrypt(h);
            var de = Newtonsoft.Json.JsonConvert.DeserializeObject(d);
            return de.ToString();
/*
            //var r = cryptedData.Replace("abcba","\"");
            var r = clsGeneralFunction.BackFromHtml(cryptedData);
            var s = Newtonsoft.Json.JsonConvert.DeserializeObject(r);
            string oTs = Convert.ToString(s);
            var d = Decrypt(oTs);
            return d;
*/
        }

        public static string DecryptAfterFromHtml(string data)
        {
            if(data == null)
                return null;
            var h = clsGeneralFunction.BackFromHtml(data);
            var r = clsGeneralFunction.Decrypt(h);
            return r;
        }

        public static string EncryptAndToHtml(string data)
        {
            if(data == null)
                return null;
            var e = clsGeneralFunction.Encrypt(data);
            var h = clsGeneralFunction.ReadyToSaveInHtml(e);
            return h;
        }

/* درست برنمیگرداند برای جی.یو.آی.دی
        public static string EncryptString(string text, string keyString)
        {
            if(text == null)
                return null;

            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            if(cipherText == null)
                return null;

            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public static string EncryptStatic(string data)
        {
            return EncryptString(data, "E546A8DF2789D5931069C522E695D4B2");
        }

        public static string DecryptStatic(string cryptedData)
        {
            return DecryptString(cryptedData, "E546A8DF2789D5931069C522E695D4B2");
        }

        public static string EncryptAndSerializeStatic(string data)
        {   
            if(data == null)
                return null;

            var e = EncryptStatic(data);
            object sTo = Convert.ChangeType(e, typeof(object));
            var s = Newtonsoft.Json.JsonConvert.SerializeObject(sTo);
            //var r = s.Replace("\"","abcba");
            var r = clsGeneralFunction.ReadyToSaveInHtml(s);
            return r;
        }

        public static string DecryptAndSerializeStatic(string cryptedData)
        {
            if(cryptedData == null)
                return null;

            //var r = cryptedData.Replace("abcba","\"");
            var r = clsGeneralFunction.BackFromHtml(cryptedData);
            var s = Newtonsoft.Json.JsonConvert.DeserializeObject(r);
            string oTs = Convert.ToString(s);
            var d = DecryptStatic(oTs);
            return d;
        }
*/


        public static string EncryptOneWay(string data)
        {
            var dataByte = Encoding.UTF8.GetBytes(data);
            //byte[] dataByte = new byte[DATA_SIZE];
            byte[] result;
            SHA256 shaM = new SHA256Managed();
            return shaM.ComputeHash(dataByte).ToString();
        }
/*
        //private static byte[] Generate256BitsOfRandomEntropy()
        private static byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
*/
        #endregion رمزنگاری


        //999/تابعی که گزینه های سلکت اچ.تی.ام.الی را میسازد
        public static string GetSelectOptions(List<KeyValuePair<int, string>> _items, int selectedKey){
            string ret = "";
            foreach(var item in _items){
                ret+="\n\t <option value='"+item.Key.ToString()+"' ";
                if(item.Key == selectedKey)
                    ret+=" selected='selected'";
                ret+=">"+item.Value+"</option>";
            }
            return ret;
        }

        //999/تابعی که گزینه های دراپداون بوت.استرپی را میسازد
        public static string GetDropdownItems(List<KeyValuePair<int, string>> _items, int selectedKey){
            string ret = "";
            foreach(var item in _items){
                ret+="\n\t <li data-id='"+item.Key.ToString()+"' ";
                if(item.Key == selectedKey)
                    ret+=" class='active'";
                //ret+="><a href='#'>"+item.Value+"</a></li>";
                ret+="><a>"+item.Value+"</a></li>";
            }
            return ret;
        }

        //999/تابعی که یک دراپ داون بوت.استرپی را میسازد بهمراه یک سلکت اچ.تی.ام.الی مخفی که مقدار انتخابی
        //کاربر را میگیرد و بصورت مخفی در فرم نگه میدارد
        //در موقع استفاده از این دراپ.داون تابع جاوا اسکریپت آن فراموش نشود
        public static string GetHtmlBootstrapDropDown(List<KeyValuePair<int, string>> _items, string id, string nameInupt, string displayName, int selectedKey){
            var selectedVal = _items.Where(a=> a.Key == selectedKey).FirstOrDefault().Value;
            var selOptions = GetSelectOptions( _items, selectedKey);
            var dropdownItems = GetDropdownItems(_items, selectedKey);
            string ret = "\n <div class='div-invisible'> <select id='"+id+"_select' name='"+nameInupt+"'> "
                            + selOptions 
                            +"\n </select> </div>";

            ret += "\n <div class='dropdown' id='"+id+"'>";
            ret += "\n\t <button class='btn btn-primary dropdown-toggle' type='button' data-toggle='dropdown'>"+displayName+" : "+selectedVal;
            ret += "\n\t\t <span class='caret'></span>";
            ret += "\n\t </button>";
            ret += "\n\t <ul class='dropdown-menu'>"
                    + dropdownItems + "\n\t </ul> \n </div> ";
            return ret;
        }
         
        public static string GetHtmlBootstrapDropDownLanguageItems(List<KeyValuePair<int, string>> _items, string _langMark){
            string ret = "";
            var page = Resource.GetData(_langMark, "page");;
            foreach(var i in _items){
                var str = clsLanguage.GetLanguages().Where(a => a.LangId == i.Key).FirstOrDefault().LangMark;
                var str2 = clsLanguage.GetLanguages().Where(a => a.LangId == i.Key).FirstOrDefault().LangNameInter;
                ret += "<li data-id='"+str+"'><a href='/Admin/"+_langMark+"/Pages/Create?pageLang="+str+"'>"+page+" "+str2+"</a></li>";
            }
            return ret;
        } 

        public static string GetHtmlBootstrapDropDownLanguage(List<KeyValuePair<int, string>> _items, string _displayName, string _langMark)
        {
            string dropdownItems = GetHtmlBootstrapDropDownLanguageItems(_items, _langMark);

            string ret = "\n <div class='dropdown'>";
            ret += "\n\t <button class='btn btn-primary dropdown-toggle' type='button' data-toggle='dropdown'>"+ _displayName;
            ret += "\n\t\t <span class='caret'></span>";
            ret += "\n\t </button>";
            ret += "\n\t <ul class='dropdown-menu'>";
            ret += dropdownItems;
            ret += "\n\t </ul>";
            ret += "\n </div>";
            return ret;
        }

        //تابعی برای تغییر کاراکترهایی که در جاوا اسکریپت مشکل درست میکنند
        public static string ReadyToSaveInHtml(string data)
        {
            string ret = data.Replace("\"", "=abcdedcba=");//شاید بهتر باشد با جی.یو.آی.دی تعویض شود
            ret = ret.Replace("\'", "=bcdefedcb=");
            ret = ret.Replace(";", "=cdeabaedc=");
            ret = ret.Replace("+", "=deabcbaed=");
            ret = ret.Replace( ",", "=eabcdcbae=");
            //ret = ret.Replace("-", "");
            //ret = ret.Replace("=", "");
            return ret;            
        }

        public static string BackFromHtml(string data)
        {
            string ret = data.Replace("=abcdedcba=", "\"");//شاید بهتر باشد با جی.یو.آی.دی تعویض شود
            ret = ret.Replace("=bcdefedcb=", "\'");
            ret = ret.Replace("=cdeabaedc=", ";");
            ret = ret.Replace("=deabcbaed=", "+");
            ret = ret.Replace("=eabcdcbae=", ",");
            //ret = ret.Replace("", "-");
            //ret = ret.Replace("", "=");
            return ret;            
        }


        //تابعی برای تولید عدد تصادفی با احتمال تکراری بودن کم
        public static int GetGuidNumber(string guid)
        {
            int x = 0;
            for (int i = 0; i < guid.Length; i++)
            {
                if (char.IsNumber(guid, i))
                    x += Convert.ToInt16(guid[i]);
            }
            return x;
        }

        public static void DeleteFolderAndAllContent(string _path){
            System.IO.DirectoryInfo di = new DirectoryInfo(_path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "clsGeneralFunction.DirectoryCopy: Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    
        public static bool InsideIIS() {
            return System.Environment.GetEnvironmentVariable("APP_POOL_ID") is string;
        }

        //تابعی که چک میکند اگر این اولین لاگین امروز است کار پاک کردن شاخه های موقت صفحه ها را انجام دهد 
        public static bool DoCheckDaily(DateTime date, string mainPath){

            //var dbContext = new IM999Max_DotNetCore();
            var opt = new DbContextOptions<IM999Max_DotNetCore>();
            var dbContext = new IM999Max_DotNetCore(opt);

            //DbContext dbContext;
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select Isnull(COUNT(*), 0) from dbo.LoginLog ll where ll.CreateDate >  CONVERT(datetime, '"+date.ToString("yyyy-MM-dd")+"')";
                dbContext.Database.OpenConnection();

                Int32 count = (Int32) command.ExecuteScalar();

                if(count == 0){
                    clsGeneralFunction.DoDeleteTempDirPages(mainPath);
                }
            }

            return true;
        }

        //im999 پاک کردن شاخههای موقت ایجاد و ویرایش صفحه که شش روز از تاریخ ایجاد آنها گذشته
        public static bool DoDeleteTempDirPages(string mainPath){
            DirectoryInfo dirs = new DirectoryInfo(mainPath);

            foreach (DirectoryInfo dir in dirs.GetDirectories())
            {
                if(dir.Name.Length > 36){
                    var str = dir.Name.Substring(37);
                    str = str.Substring(0,str.IndexOf("_"));
                    var folderDate = DateTime.Parse(str);
                    if((DateTime.Now - folderDate).TotalDays > 6){
                        dir.Delete(true);
                    }
                } else if(dir.Name.Length == 36){
                    DateTime creatDate = dir.CreationTime;
                    if((DateTime.Now - creatDate).TotalDays > 6){
                        dir.Delete(true);
                    }
                }
            }

            return true;
        }

        public static bool RemoveDir(string path){
            DirectoryInfo dir = new DirectoryInfo(path);
            dir.Delete(true);  
            return true;          
        }
    }
}
