using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHSWeldingApi.Models
{
  public class ClientSelection
  {
    public ClientSelection()
    {
      this.ClientID = String.Empty;
      this.ClientState = String.Empty;
    }

    public string ClientID { get; set; }
    public string ClientState { get; set; }
  }
}