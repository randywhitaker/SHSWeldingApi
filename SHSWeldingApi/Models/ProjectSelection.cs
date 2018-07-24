using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHSWeldingApi.Models
{
  public class ProjectSelection
  {
    public ProjectSelection()
    {
      this.ProjectID = String.Empty;
      this.ProjectName = String.Empty;
      this.ProjectState = String.Empty;
      this.ClientID = String.Empty;
    }

    public string ProjectID { get; set; }
    public string ProjectName { get; set; }
    public string ProjectState { get; set; }
    public string ClientID { get; set; }
  }
}