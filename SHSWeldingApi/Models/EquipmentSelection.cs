using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHSWeldingApi.Models
{
  public class EquipmentSelection
  {
    public string ExpID { get; set; }
    public string ExpDescription { get; set; }
    public string ExpLabel
    {
      get
      {
        return String.Format("{0} {1}", this.ExpID, this.ExpDescription);
      }
    }
  }
}