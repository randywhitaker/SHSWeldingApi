using System;
using System.IO;
using System.Text;
using System.Configuration;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace SHSWeldingApi.Models
{
  public class TimeSheetReport
  {
    public TimeSheetReport()
    {
      this.FontSize = 10;
      this.LeftMargin = 30;

      if (ConfigurationManager.AppSettings["company_name"] != null &&
          ConfigurationManager.AppSettings["mailing_address"] != null &&
          ConfigurationManager.AppSettings["mailing_city_state_zip"] != null &&
          ConfigurationManager.AppSettings["contact_numbers"] != null)
      {
        this.CompanyName = ConfigurationManager.AppSettings["company_name"];
        this.MailingAddress = ConfigurationManager.AppSettings["mailing_address"];
        this.MailingCityStateZip = ConfigurationManager.AppSettings["mailing_city_state_zip"];
        this.ContactNumbers = ConfigurationManager.AppSettings["contact_numbers"];

      }
    }
    public string CompanyName { get; set; }
    public string MailingAddress { get; set; }
    public string MailingCityStateZip { get; set; }
    public string ContactNumbers { get; set; }
    public double FontSize { get; set; }
    public double LeftMargin { get; set; }
    public double CurrentLine { get; set; }
    public double NextLine(int line)
    {
      this.CurrentLine += (this.FontSize * line);
      return this.CurrentLine;
    }

    public void GenerateTimeSheet(sheet data, string SaveTo, string SignaturePath)
    {
      string tmp = TimesheetTemplate(data);
      PdfDocument pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(tmp, PdfSharp.PageSize.A4);
      pdf.Info.Title = "Daily Timesheet";
      pdf.Info.Author = CompanyName;
      pdf.Info.Subject = String.Format("Timesheet for {0}", DateTime.Now.ToShortDateString());

      int LastPageNumber = pdf.Pages.Count - 1;

      if (LastPageNumber > -1)
      {
        XGraphics gfx = XGraphics.FromPdfPage(pdf.Pages[LastPageNumber]);
        double BottonMargin = 30;
        XFont font = new XFont("Verdana", this.FontSize, XFontStyle.Regular);

        if (File.Exists(SignaturePath))
        {
          XImage xImage = XImage.FromFile(SignaturePath);

          XSize p = gfx.PageSize;
          double yPos = p.Height - xImage.PointHeight - BottonMargin - this.FontSize;
          double xPos1 = this.LeftMargin + 30;
          double xPos2 = (p.Width / 2) - 30;
          double xPos3 = (p.Width - this.LeftMargin - 100);

          gfx.DrawString("Representative Signature", font, XBrushes.Black, Convert.ToInt32(xPos1), Convert.ToInt32(yPos));
          gfx.DrawString("Printed Name", font, XBrushes.Black, Convert.ToInt32(xPos2), Convert.ToInt32(yPos));
          gfx.DrawString("Date", font, XBrushes.Black, Convert.ToInt32(xPos3), Convert.ToInt32(yPos));

          yPos += this.FontSize + 5;

          gfx.DrawImage(xImage, Convert.ToInt32(LeftMargin), Convert.ToInt32(yPos), xImage.PointWidth, xImage.PointHeight);
          gfx.DrawString(data.printedName, font, XBrushes.Black, Convert.ToInt32(xPos2), Convert.ToInt32(yPos));
          gfx.DrawString(data.signatureDate, font, XBrushes.Black, Convert.ToInt32(xPos3), Convert.ToInt32(yPos));
        }
      }

      pdf.Save(SaveTo);
      pdf.Close();     
    }

    private string TimesheetTemplate(sheet s)
    {
      StringBuilder h = new StringBuilder();

      h.Append("<div>");
      h.Append("<header style=\"text-align: center;\">");
      h.AppendFormat("<h2>{0}</h2>", this.CompanyName);
      h.AppendFormat("<div>{0}<br />{1}<br />{2}</div>", this.MailingAddress, this.MailingCityStateZip, this.ContactNumbers);
      h.Append("<hr />");
      h.Append("<div style=\"font-size: 16px; font-weight: bold;\">Daily Timesheet</div>");
      h.Append("<hr />");
      h.Append("</header>");

      h.Append("<table style=\"width: 98%; margin: auto\">");
      h.Append("<tbody>");
      h.Append("<tr>");
      h.Append("<td>Client</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.client.ClientID);
      h.Append("<td>State</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.state.Name);
      h.Append("</tr>");
      h.Append("<tr>");
      h.Append("<td>Field Name</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.fieldName.ProjectName);
      h.Append("<td>County / Parish</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.countyOrParish);
      h.Append("</tr>");
      h.Append("<tr>");
      h.Append("<td>Location/Well Name</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.locationOrWellName);
      h.Append("<td>Date</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.date);
      h.Append("</tr>");
      h.Append("<tr>");
      h.Append("<td>AFE #</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.afeNumber);
      h.Append("<td>Day of Week</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.dayOfWeek);
      h.Append("</tr>");
      h.Append("<tr>");
      h.Append("<td>Approval #</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.approvalNumber);
      h.Append("<td>Truck No.</td>");
      h.AppendFormat("<td style=\"border: 1px solid #000000; width: 25%; padding: 0px 5px;\">{0}</td>", s.truckNumber);
      h.Append("</tr>");
      h.Append("</tbody>");
      h.Append("</table>");
      h.Append("<br />");
      h.Append("<br />");

      h.Append("<table style=\"width: 98%; margin: auto;\" cellpadding=\"0\" cellspacing=\"0\">");
      h.Append("<thead>");
      h.Append("<tr style=\"background-color: silver;\">");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\"></th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Worker</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Billing Category</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Start Time</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">End Time</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Lunch Hrs.</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Reg. Hrs.</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">O.T. Hrs.</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-right: 1px solid #000000;\">Wrkd. Hrs.</th>");
      h.Append("</tr>");
      h.Append("</thead>");
      h.Append("<tbody>");

      // Add rows here...
      int items = 0;
      double totalLunch = 0;
      double totalReg = 0;
      double totalOt = 0;
      double totalWkd = 0;

      foreach (var itm in s.workcrew)
      {
        items++;
        totalLunch += ConvertTimeToMinutes(itm.lunchHour);
        totalReg += ConvertTimeToMinutes(itm.regularHours);
        totalOt += ConvertTimeToMinutes(itm.overTimeHours);
        totalWkd += ConvertTimeToMinutes(itm.workedHours);

        h.Append("<tr>");
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", items);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;padding: 0px 5px;\">{0}</td>", itm.worker);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;padding: 0px 5px;\">{0}</td>", itm.billingCategory);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", itm.startTime);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", itm.endTime);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", itm.lunchHour);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", itm.regularHours);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", itm.overTimeHours);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-right: 1px solid #000000;text-align:center;\">{0}</td>", itm.workedHours);
        h.Append("</tr>");
      }

      h.Append("</tbody>");
      h.Append("<tfoot>");
      h.Append("<tr>");
      h.Append("<td colspan=\"4\" style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;\"></td>");
      h.Append("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;text-align:right; font-weight:bold;padding: 0px 5px;\">Total</td>");
      h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;text-align:center;\">{0}</td>", ConvertMinutesToTime(totalLunch));
      h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;text-align:center;\">{0}</td>", ConvertMinutesToTime(totalReg));
      h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;text-align:center;\">{0}</td>", ConvertMinutesToTime(totalOt));
      h.AppendFormat("<td style=\"border: 1px solid #000000;text-align:center;\">{0}</td>", ConvertMinutesToTime(totalWkd));
      h.Append("</tr>");
      h.Append("</tfoot>");
      h.Append("</table>");

      h.Append("<br />");
      h.Append("<br />");

      h.Append("<table style=\"width: 98%; margin: auto;\" cellpadding=\"0\" cellspacing=\"0\">");
      h.Append("<thead>");
      h.Append("<tr style=\"background-color: silver;\">");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\"></th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Equipment or Tools</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;\">Quartity</th>");
      h.Append("<th style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-right: 1px solid #000000;\">Notes:</th>");
      h.Append("</tr>");
      h.Append("</thead>");
      h.Append("<tbody>");

      int index = 0;
      int totalquantity = 0;
      foreach (var itm in s.equipmentList)
      {
        index++;
        totalquantity += itm.quantity;

        h.Append("<tr>");
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", index);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;padding: 0px 5px;\">{0}</td>", itm.equipment);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;text-align:center;\">{0}</td>", itm.quantity);
        h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-right: 1px solid #000000;padding: 0px 5px;\">{0}</td>", itm.notes);
        h.Append("</tr>");
      }

      h.Append("</tbody>");
      h.Append("<tfoot>");
      h.Append("<tr>");
      h.Append("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;\"></td>");
      h.Append("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;text-align:right; font-weight:bold;padding: 0px 5px;\">Total Hours</td>");
      h.AppendFormat("<td style=\"border-top: 1px solid #000000;border-left: 1px solid #000000;border-bottom: 1px solid #000000;text-align:center;\">{0}</td>", totalquantity);
      h.Append("<td style=\"border: 1px solid #000000;\"></td>");
      h.Append("</tr>");
      h.Append("</tfoot>");
      h.Append("</table>");

      h.Append("<br />");
      h.Append("<div style=\"width: 98%; margin: auto; border: 1px solid #000000; padding: 5px;\">");
      h.AppendFormat("Remarks:<br />{0}", s.remarks);
      h.Append("</div>");

      h.Append("</div>");

      return h.ToString();
    }
    private double ConvertTimeToMinutes(string value)
    {
      TimeSpan ts;

      if (TimeSpan.TryParse(value, out ts))
        return ts.TotalMinutes;
      else
        return 0;
    }
    private string ConvertMinutesToTime(double value)
    {
      var result = TimeSpan.FromMinutes(value);
      return result.ToString(@"hh\:mm");
    }
  }
}