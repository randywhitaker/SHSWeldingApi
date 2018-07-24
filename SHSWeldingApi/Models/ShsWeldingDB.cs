using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Text;

namespace SHSWeldingApi.Models
{
  public class ShsWeldingDB
  {
    const string ConnectionName = "SHSDB";
    DbProviderFactory factory = null;
    ConnectionStringSettings cnnSettings = null;

    public ShsWeldingDB()
    {
      if (ConfigurationManager.ConnectionStrings[ConnectionName] != null)
      {
        cnnSettings = ConfigurationManager.ConnectionStrings[ConnectionName];
        factory = DbProviderFactories.GetFactory(cnnSettings.ProviderName);
      }
      else
      {
        throw new Exception(String.Format("Missing connection string named: {0}", ConnectionName));
      }
    }
    public List<ClientSelection> ClientSelections()
    {
      List<ClientSelection> lst = new List<ClientSelection>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT ClientID, ISNULL(ClientState, '') ClientState FROM dbo.Client WHERE Status = 'Active' ORDER BY ClientID";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              lst.Add(FillClientSelection(reader));
            }
          }
          cnn.Close();
        }
      }

      return lst;
    }
    public string[] CountySelections()
    {
      List<String> lst = new List<String>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT [Description] FROM dbo.CustomddListDetail WHERE ListID = 'BA772F52-2459-4544-8AFA-B98C1C86D6E1' ORDER BY [Description]";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              var itm = FillItem(reader);
              if (!String.IsNullOrEmpty(itm))
              {
                lst.Add(itm);
              }
            }
          }
          cnn.Close();
        }
      }

      return lst.ToArray();
    }
    public List<TruckSelection> TruckSelections()
    {
      List<TruckSelection> lst = new List<TruckSelection>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT ExpID, ExpCode, ExpDescription FROM dbo.Expense WHERE ExpID like 'SCT%' OR ExpID Like 'SPUH%' ORDER BY ExpID";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              lst.Add(FillTruckSelection(reader));
            }
          }
          cnn.Close();
        }
      }

      return lst;
    }
    public List<BillingSelection> BillingSelections()
    {
      List<BillingSelection> lst = new List<BillingSelection>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT ActivityID, ActivityDescription FROM dbo.Activity WHERE isinactive = 0 ORDER BY ActivityID";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              lst.Add(FillBillingSelection(reader));
            }
          }
          cnn.Close();
        }
      }

      return lst;
    }
    public List<EquipmentSelection> EquipmentSelections()
    {
      List<EquipmentSelection> lst = new List<EquipmentSelection>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT ExpID, ExpDescription FROM dbo.Expense WHERE DefaultGroupID = 'EQ' ORDER BY ExpID";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              lst.Add(FillEquipmentSelection(reader));
            }
          }
          cnn.Close();
        }
      }

      return lst;
    }
    public List<WorkerSelection> WorkerSelections()
    {
      List<WorkerSelection> lst = new List<WorkerSelection>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT EmployeeID, EmpFName, EmpLName FROM dbo.Employee WHERE (VendorType = 1 OR EmpManager IS NOT NULL) AND Status = 'Active' ORDER BY EmpFName, EmpLName";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              lst.Add(FillWorkerSelection(reader));
            }
          }
          cnn.Close();
        }
      }

      return lst;
    }
    public List<ProjectSelection> ProjectSelections()
    {
      List<ProjectSelection> lst = new List<ProjectSelection>();

      using (DbConnection cnn = factory.CreateConnection())
      {
        cnn.ConnectionString = cnnSettings.ConnectionString;

        using (DbCommand cmd = cnn.CreateCommand())
        {
          cmd.CommandType = System.Data.CommandType.Text;
          cmd.CommandText = "SELECT ProjectID, ProjectName, ISNULL(ProjectState, '') ProjectState, ClientID FROM dbo.Project WHERE ProjectStatus = 'Active' ORDER BY ProjectID";
          cmd.CommandTimeout = cnn.ConnectionTimeout;

          cnn.Open();
          DbDataReader reader = cmd.ExecuteReader();

          if (reader.HasRows)
          {
            while (reader.Read())
            {
              lst.Add(FillProjectSelection(reader));
            }
          }
          cnn.Close();
        }
      }

      return lst;
    }
    private string FillItem(DbDataReader reader)
    {
      if (reader != null && reader.FieldCount > 0 && !reader.IsDBNull(0))
      {
        return reader.GetString(0);
      }

      return String.Empty;
    }
    private ClientSelection FillClientSelection(DbDataReader reader)
    {
      ClientSelection o = new ClientSelection();

      for (int index = 0; index < reader.FieldCount; index++)
      {
        if (!reader.IsDBNull(index))
        {
          switch (reader.GetName(index))
          {
            case "ClientID":
              o.ClientID = reader.GetString(index);
              break;
            case "ClientState":
              o.ClientState = reader.GetString(index);
              break;
          }
        }
      }

      return o;
    }
    private TruckSelection FillTruckSelection(DbDataReader reader)
    {
      TruckSelection o = new TruckSelection();

      for (int index = 0; index < reader.FieldCount; index++)
      {
        if (!reader.IsDBNull(index))
        {
          switch (reader.GetName(index))
          {
            case "ExpID":
              o.TruckId = reader.GetString(index);
              break;
            case "ExpCode":
              o.TruckCode = reader.GetString(index);
              break;
            case "ExpDescription":
              o.TruckName = reader.GetString(index);
              break;
          }
        }
      }

      return o;
    }
    private BillingSelection FillBillingSelection(DbDataReader reader)
    {
      BillingSelection o = new BillingSelection();

      for (int index = 0; index < reader.FieldCount; index++)
      {
        if (!reader.IsDBNull(index))
        {
          switch (reader.GetName(index))
          {
            case "ActivityID":
              o.ActivityID = reader.GetString(index);
              break;
            case "ActivityDescription":
              o.ActivityDescription = reader.GetString(index);
              break;
          }
        }
      }

      return o;
    }
    private EquipmentSelection FillEquipmentSelection(DbDataReader reader)
    {
      EquipmentSelection o = new EquipmentSelection();

      for (int index = 0; index < reader.FieldCount; index++)
      {
        if (!reader.IsDBNull(index))
        {
          switch (reader.GetName(index))
          {
            case "ExpID":
              o.ExpID = reader.GetString(index);
              break;
            case "ExpDescription":
              o.ExpDescription = reader.GetString(index);
              break;
          }
        }
      }

      return o;
    }
    private WorkerSelection FillWorkerSelection(DbDataReader reader)
    {
      WorkerSelection o = new WorkerSelection();

      for (int index = 0; index < reader.FieldCount; index++)
      {
        if (!reader.IsDBNull(index))
        {
          switch (reader.GetName(index))
          {
            case "EmployeeID":
              o.EmployeeID = reader.GetString(index);
              break;
            case "EmpFName":
              o.EmpFName = reader.GetString(index);
              break;
            case "EmpLName":
              o.EmpLName = reader.GetString(index);
              break;
          }
        }
      }

      return o;
    }
    private ProjectSelection FillProjectSelection(DbDataReader reader)
    {
      ProjectSelection o = new ProjectSelection();

      for (int index = 0; index < reader.FieldCount; index++)
      {
        if (!reader.IsDBNull(index))
        {
          switch (reader.GetName(index))
          {
            case "ProjectID":
              o.ProjectID = reader.GetString(index);
              break;
            case "ProjectName":
              o.ProjectName = reader.GetString(index);
              break;
            case "ProjectState":
              o.ProjectState = reader.GetString(index);
              break;
            case "ClientID":
              o.ClientID = reader.GetString(index);
              break;
          }
        }
      }

      return o;
    }
  }
}