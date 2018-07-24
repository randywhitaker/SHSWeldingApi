using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using System.Net.Http;
using System.Web.Http;
using SHSWeldingApi.Models;

namespace SHSWeldingApi.Controllers
{
  public class timesheetController : ApiController
  {
    public Boolean Post(sheet s)
    {
      if (s != null)
      {
        DateTime dt = DateTime.Now;
        string folder = HostingEnvironment.MapPath("~/timesheets");

        try
        {
          string pdfName = String.Format("SHSWelding_{0:u}.pdf", dt).Replace(":", "");
          string pdfPath = Path.Combine(folder, pdfName);
          string imgName = String.Format("signature_{0:u}.jpg", dt).Replace(":", "");
          string imgPath = Path.Combine(folder, imgName);

          if (!String.IsNullOrEmpty(s.signatureImage) && s.signatureImage.StartsWith("data:image/jpeg;base64,/"))
          {
            byte[] bytes = Convert.FromBase64String(s.signatureImage.Replace("data:image/jpeg;base64,", ""));

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
              image = Image.FromStream(ms);
              image.Save(imgPath);
            }
          }
          else
          {
            imgPath = String.Empty;
          }

          TimeSheetReport rpt = new TimeSheetReport();
          rpt.GenerateTimeSheet(s, pdfPath, imgPath);

          return true;
        }
        catch (Exception ex)
        {
          string errName = String.Format("error_{0:u}.log", dt).Replace(":", "");
          string errPath = Path.Combine(folder, errName);
          LogData(errPath, ex.Message);
          LogData(errPath, ex.StackTrace);
        }
      }

      return false;
    }
    private void LogData(string name, string message)
    {
      try
      {
        using (var sw = new StreamWriter(name, true))
        {
          sw.WriteLine(String.Format("[{0}] - {1}", DateTime.Now, message));
          sw.Flush();
          sw.Close();
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}