using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHSWeldingApi.Models
{
  public class WorkerSelection
  {
    public string EmployeeID { get; set; }
    public string EmpFName { get; set; }
    public string EmpLName { get; set; }
    public string FullName
    {
      get
      {
        string fullname = String.Empty;

        if (!String.IsNullOrEmpty(EmpFName))
          fullname = EmpFName.Trim();

        if (!String.IsNullOrEmpty(EmpLName))
          fullname += " " + EmpLName.Trim();

        return fullname;      }
    }
  }
}