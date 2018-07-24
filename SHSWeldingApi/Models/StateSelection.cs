using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHSWeldingApi.Models
{
  public class StateSelection
  {
    public StateSelection()
    {
      this.Code = String.Empty;
      this.Name = String.Empty;
    }
 
    public string Code { get; set; }
    public string Name { get; set; }
  }
}