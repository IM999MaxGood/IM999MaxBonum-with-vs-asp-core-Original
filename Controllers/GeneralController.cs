using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Mvc;
using IM999MaxBonum.Classes;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;


namespace IM999MaxBonum.Controllers
{
    //public class GeneralController : BaseController
    public class GeneralController(IHostingEnvironment environment) : BaseController(environment)
    {
        //999/توجه شود در زمان گرفتن تصویر کوکی ذخیره شده اما در ای.اس.پی در دسترس نیست و باید به فرم پاس داده شود
        [HttpGet]//, ChildActionOnly]
        //برای کش نکردن تصاویر
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [ResponseCache(Duration = 0)]
        //public IActionResult CaptchaImage(string prefix, bool noisy = true) 
        public IActionResult CaptchaImage(string guids, string prefix, bool noisy = true) 
        {

            var n = clsGeneralFunction.GetGuidNumber(Guid.NewGuid().ToString());

            var rand = new Random(n);//(int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99); 
            int b = rand.Next(0, 9);

            var captcha = string.Format("{0} + {1} = ?", a, b); 
 
            //store answer 
            //HttpContext.Session.Set<string>("Captcha_" + prefix , (a + b).ToString()); 
            //SC.SetValue("Captcha_" + prefix , (a + b).ToString()); 
            SD.SetValue("Captcha_" + prefix , (a + b).ToString()); 

            //image stream 
            FileContentResult img = null; 
            using (var mem = new MemoryStream()) 
            using (var bmp = new Bitmap(130, 30)) 
            using (var gfx = Graphics.FromImage((Image)bmp)) 
            { 
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit; 
                gfx.SmoothingMode = SmoothingMode.AntiAlias; 
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height)); 
 
 
                //add noise 
                if (noisy) 
                { 
                    int i, r, x, y; 
                    var pen = new Pen(Color.Yellow); 
                    for (i = 1; i < 10; i++) 
                    { 
                        pen.Color = Color.FromArgb( 
                        (rand.Next(0, 255)), 
                        (rand.Next(0, 255)), 
                        (rand.Next(0, 255))); 
 
                        r = rand.Next(0, (130 / 3)); 
                        x = rand.Next(0, 130); 
                        y = rand.Next(0, 30); 
 
                        gfx.DrawEllipse(pen, x-r, y-r, r, r); 
                    } 
                } 
 
                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3); 
 
                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");


                /*
                //متد ذخیره فایل در مسیر موقت و لود دوباره برای نمایش این روش بعلت کش نکردن عکسها انتخاب شده
                var s = Guid.NewGuid();
                var dir = Server.MapPath(ConfigurationManager.AppSettings["temp-path"]);
                var path = Path.Combine(dir, s + ".jpeg");
                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                FileStream file = new FileStream(path, FileMode.Open);
                file.CopyTo(mem);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
                 * */
            }  

            return img; 
        }
    }
}