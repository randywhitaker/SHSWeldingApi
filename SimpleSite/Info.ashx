<%@ WebHandler Language="C#" Class="Info" %>

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

public class Info : IHttpHandler {

  public void ProcessRequest (HttpContext context) {
    string id = context.Request.QueryString["id"];

    if (context.Request.HttpMethod == "GET")
    {
      StringBuilder h = new StringBuilder();

      if (!String.IsNullOrEmpty(id) && id.Equals("settings"))
      {
          h.Append("{");
          h.Append("\"clients\": [");
          h.Append("{\"ClientID\": \"Energy One\",\"ClientState\": \"TX\"},");
          h.Append("{\"ClientID\": \"Tester\",\"ClientState\": \"NM\"}],");
          h.Append("\"projects\":[");
          h.Append("{\"ProjectID\":\"XTO:001\",\"ProjectName\":\"BETHANY\",\"ProjectState\": \"\",\"ClientID\":\"Energy One\"},");
          h.Append("{\"ProjectID\":\"XTO:002\",\"ProjectName\":\"BIG SANDY\",\"ProjectState\": \"\",\"ClientID\":\"Energy One\"},");
          h.Append("{\"ProjectID\":\"XTO:003\",\"ProjectName\":\"CARTHAGE\",\"ProjectState\": \"TX\",\"ClientID\":\"Energy One\"},");
          h.Append("{\"ProjectID\":\"XTO:004\",\"ProjectName\":\"CHEROKEE\",\"ProjectState\": \"\",\"ClientID\":\"Tester\"},");
          h.Append("{\"ProjectID\":\"XTO:006\",\"ProjectName\":\"DELROSE\",\"ProjectState\": \"OR\",\"ClientID\":\"Tester\"},");
          h.Append("{\"ProjectID\":\"XTO:007\",\"ProjectName\":\"EAST TEXAS\",\"ProjectState\": \"\",\"ClientID\":\"Tester\"},");
          h.Append("{\"ProjectID\":\"XTO:008\",\"ProjectName\":\"GILMER\",\"ProjectState\": \"\",\"ClientID\":\"Tester\"}],");
          h.Append("\"states\": [");
          h.Append("{\"Code\": \"AR\",\"Name\": \"Arkansas\"},");
          h.Append("{\"Code\": \"LA\",\"Name\": \"Louisana\"},");
          h.Append("{\"Code\": \"MS\",\"Name\": \"Mississippi\"},");
          h.Append("{\"Code\": \"NM\",\"Name\": \"New Mexico\"},");
          h.Append("{\"Code\": \"OK\",\"Name\": \"Oklahoma\"},");
          h.Append("{\"Code\": \"OR\",\"Name\": \"Oregon\"},");
          h.Append("{\"Code\": \"TX\",\"Name\": \"Texas\"}],");
          h.Append("\"counties\":[\"Anderson\",\"Angelina\",\"Camp\",\"Cherokee\",\"DeSoto\",\"Gregg\",\"Harrison\",\"Henderson\",\"Marion\",\"Nacogdoches\",\"Panola\",\"Rains\",\"Rusk\",\"Shelby\",\"Smith\",\"Upshur\",\"Van Zandt\",\"Wood\"],");
          h.Append("\"workers\":[");
          h.Append("{\"EmployeeID\":\"EP100\",\"EmpFName\":\"Alejandre\",\"EmpLName\":\"Anquiano\",\"FullName\":\"Alejandre Anquiano\"},");
          h.Append("{\"EmployeeID\":\"EP101\",\"EmpFName\":\"Alfredo\",\"EmpLName\":\"Acevedo\",\"FullName\":\"Alfredo Acevedo\"},");
          h.Append("{\"EmployeeID\":\"EP102\",\"EmpFName\":\"Eduardo Zapata\",\"EmpLName\":\"DeLeon\",\"FullName\":\"Eduardo Zapata DeLeon\"},");
          h.Append("{\"EmployeeID\":\"EP103\",\"EmpFName\":\"From\",\"EmpLName\":\"QuickBooks\",\"FullName\":\"From QuickBooks\"}],");
          h.Append("\"billingCategories\":[");
          h.Append("{\"ActivityID\":\"GP100\",\"ActivityDescription\":\"Crew Pusher\"},");
          h.Append("{\"ActivityID\":\"GP101\",\"ActivityDescription\":\"Operator\"},");
          h.Append("{\"ActivityID\":\"GP102\",\"ActivityDescription\":\"Supervisor\"},");
          h.Append("{\"ActivityID\":\"GP103\",\"ActivityDescription\":\"Crew Pusher\"},");
          h.Append("{\"ActivityID\":\"GP104\",\"ActivityDescription\":\"Foreman\"},");
          h.Append("{\"ActivityID\":\"GP105\",\"ActivityDescription\":\"Labor\"},");
          h.Append("{\"ActivityID\":\"GP106\",\"ActivityDescription\":\"Mechanic - incl truck\"},");
          h.Append("{\"ActivityID\":\"GP107\",\"ActivityDescription\":\"Welder\"}],");
          h.Append("\"equipment\":[");
          h.Append("{\"ExpID\":\"VC200\",\"ExpDescription\":\"Commercial Airfare\"},");
          h.Append("{\"ExpID\":\"VC201\",\"ExpDescription\":\"Auto Fuel\"},");
          h.Append("{\"ExpID\":\"VC202\",\"ExpDescription\":\"Licenses & Permits\"},");
          h.Append("{\"ExpID\":\"VC203\",\"ExpDescription\":\"Meals\"},");
          h.Append("{\"ExpID\":\"VC204\",\"ExpDescription\":\"Mileage\"},");
          h.Append("{\"ExpID\":\"VC205\",\"ExpDescription\":\"Parking\"},");
          h.Append("{\"ExpID\":\"VC206\",\"ExpDescription\":\"Postage\"},");
          h.Append("{\"ExpID\":\"VC207\",\"ExpDescription\":\"Lodging\"}],");
          h.Append("\"dayOfWeek\":[\"Sunday\",\"Monday\",\"Tuesday\",\"Wednesday\",\"Thursday\",\"Friday\",\"Saturday\"],");
          h.Append("\"lunchHours\":[\"0:30\",\"1:00\",\"1:30\",\"2:00\"]");
          h.Append("}");

          context.Response.ContentType = "application/json; charset=utf-8";
          context.Response.Write(h.ToString());
      }
      else
      {
          h.AppendLine("<!DOCTYPE html>");
          h.AppendLine("<html>");
          h.AppendLine("<head>");
          h.AppendLine("<title>Sample Sheet</title>");
          h.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
          h.AppendLine("</head>");
          h.AppendLine("<body>");
          h.AppendFormat("<p>ID: {0}</p>", id);
          h.AppendLine();
          h.AppendLine(ListSheets(id));
          h.AppendLine("</body>");
          h.AppendLine("</html>");

          context.Response.ContentType = "text/html";
          context.Response.Write(h.ToString());
      }
    }
    else if (context.Request.HttpMethod == "POST")
    {
      StreamReader reader = new StreamReader(context.Request.InputStream);
      string postBody = reader.ReadToEnd();

      if (String.IsNullOrEmpty(id))
        id = "sheet.txt";

      if (String.IsNullOrEmpty(postBody))
        postBody = "{ \"SheetID\": \"" + id + "\", \"Name\": \"Testing\"}";;

      sheetInfo o = new sheetInfo();
      o.SheetId = id;
      o.DateCreate = DateTime.Now.AddMinutes(5);
      o.SheetJson = postBody;

      context.Response.ContentType = "text/plain";
      context.Response.Write("ID: " + id + " ");

      SaveSheet(o);
    }
  }

  public bool IsReusable {
    get {
      return false;
    }
  }
  private string ListSheets(string id)
  {
    string name = "SHSDB";
    StringBuilder h = new StringBuilder();

    if (ConfigurationManager.ConnectionStrings[name] != null)
    {
      string cnnStr = ConfigurationManager.ConnectionStrings[name].ConnectionString;
      DbProviderFactory factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[name].ProviderName);

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnStr;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT * FROM dbo.sheet";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            h.AppendLine("<table border=\"1\">");
            h.AppendLine("<tr><th>Sheet Name</th><th>Created</th></tr>");

            while (reader.Read())
            {
              sheetInfo o = FillSheetInfo(reader);
              h.Append("<tr>");
              h.AppendFormat("<td>{0}</td><td>{1:d}</td>", o.SheetId, o.DateCreate);
              h.AppendLine("</tr>");
            }
            h.AppendLine("</table>");
          }
          cnn.Close();
        }
      }
    }
    return h.ToString();
  }
  private void SaveSheet(sheetInfo o)
  {
    int affected = 0;
    string name = "SHSDB";

    if (ConfigurationManager.ConnectionStrings[name] != null)
    {
      string cnnStr = ConfigurationManager.ConnectionStrings[name].ConnectionString;
      DbProviderFactory factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[name].ProviderName);

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnStr;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "INSERT INTO dbo.sheet (SheetID, DateCreated, SheetJson) VALUES(@SheetID, @DateCreated, @SheetJson)";
          cmd.CommandTimeout = cnn.ConnectionTimeout;
          cmd.Parameters.Add(AddWithValue(cmd, "@SheetID", o.SheetId));
          cmd.Parameters.Add(AddWithValue(cmd, "@DateCreated", o.DateCreate));
          cmd.Parameters.Add(AddWithValue(cmd, "@SheetJson", o.SheetJson));

          cnn.Open();
          affected = cmd.ExecuteNonQuery();
          cnn.Close();
        }
      }
      HttpContext.Current.Response.Write("Saved " + affected.ToString() + " items.");
    }
  }
  private DbParameter AddWithValue(DbCommand cmd, string name, object value)
  {
    DbParameter p = cmd.CreateParameter();
    p.Direction = System.Data.ParameterDirection.Input;
    p.ParameterName = name;
    p.Value = value;
    //p.DbType = System.Data.DbType.DateTime;

    return p;
  }
  private sheetInfo FillSheetInfo(DbDataReader reader)
  {
    sheetInfo o = new sheetInfo();

    for (int index = 0; index < reader.FieldCount; index++)
    {
      if (!reader.IsDBNull(index))
      {
        switch (reader.GetName(index))
        {
          case "SheetID":
            o.SheetId = reader.GetString(index);
            break;
          case "DateCreate":
            o.DateCreate = reader.GetDateTime(index);
            break;
          case "SheetJson":
            o.SheetJson = reader.GetString(index);
            break;
        }
      }
    }

    return o;
  }
}

public class sheetInfo
{
  public sheetInfo()
  {
    this.SheetId = String.Empty;
    this.DateCreate = DateTime.Now;
    this.SheetJson = String.Empty;
  }
  public string SheetId { get; set; }
  public DateTime DateCreate { get; set; }
  public string SheetJson { get; set; }
}