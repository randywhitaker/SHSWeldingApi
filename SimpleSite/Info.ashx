<%@ WebHandler Language="C#" Class="Info" %>

using System;
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
    else if (context.Request.HttpMethod == "POST")
    {
      if (String.IsNullOrEmpty(id))
        id = "site-test.txt";

      sheetInfo o = new sheetInfo();
      o.SheetId = id;
      o.DateCreate = DateTime.Now.AddMinutes(5);
      o.SheetJson = "{ \"SheetID\": \"" + id + "\", \"Name\": \"Testing\"}";

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