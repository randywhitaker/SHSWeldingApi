using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHSWeldingApi.Models
{
  public class AppSettings
  {
    public AppSettings()
    {
      ShsWeldingDB db = new ShsWeldingDB();

      this.clients = db.ClientSelections();
      this.projects = db.ProjectSelections();
      this.states = new List<StateSelection>();
      this.states.Add(new StateSelection { Name = "Arkansas", Code = "AR" });
      this.states.Add(new StateSelection { Name = "Louisana", Code = "LA" });
      this.states.Add(new StateSelection { Name = "Mississippi", Code = "MS" });
      this.states.Add(new StateSelection { Name = "New Mexico", Code = "NM" });
      this.states.Add(new StateSelection { Name = "Oklahoma", Code = "OK" });
      this.states.Add(new StateSelection { Name = "Oregon", Code = "OR" });
      this.states.Add(new StateSelection { Name = "Texas", Code = "TX" });

      this.counties = db.CountySelections();
      this.trucks = db.TruckSelections();
      this.workers = db.WorkerSelections();
      this.billingCategories = db.BillingSelections();
      this.equipment = db.EquipmentSelections();
      this.dayOfWeek = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
      this.lunchHours = new string[] { "0:30", "1:00", "1:30", "2:00" };
      this.job = new sheet();
    }

    public List<ClientSelection> clients { get; set; }
    public List<ProjectSelection> projects { get; set; }
    public List<StateSelection> states { get; set; }
    public string[] counties { get; set; }
    public List<TruckSelection> trucks { get; set; }
    public List<WorkerSelection> workers { get; set; }
    public List<BillingSelection> billingCategories { get; set; }
    public List<EquipmentSelection> equipment { get; set; }
    public string[] dayOfWeek { get; set; }
    public string[] lunchHours { get; set; }
    public sheet job { get; set; }
  }
  public class sheet
  {
    public sheet()
    {
      this.fieldName = new ProjectSelection();
      this.locationOrWellName = String.Empty;
      this.client = new ClientSelection();
      this.afeNumber = String.Empty;
      this.approvalNumber = String.Empty;
      this.state = new StateSelection();
      this.countyOrParish = String.Empty;
      this.date = String.Empty;
      this.truckNumber = String.Empty;
      this.remarks = String.Empty;
      this.signatureImage = String.Empty;
      this.printedName = String.Empty;
      this.signatureDate = String.Empty;
      this.workcrew = new List<workcrew>();
      this.equipmentList = new List<equipmentRecord>();
    }
    public ProjectSelection fieldName { get; set; }
    public string locationOrWellName { get; set; }
    public ClientSelection client { get; set; }
    public string afeNumber { get; set; }
    public string approvalNumber { get; set; }
    public StateSelection state { get; set; }
    public string countyOrParish { get; set; }
    public string date { get; set; }
    public string dayOfWeek { get; set; }
    public string truckNumber { get; set; }
    public string remarks { get; set; }
    public string signatureImage { get; set; }
    public string printedName { get; set; }
    public string signatureDate { get; set; }
    public List<workcrew> workcrew { get; set; }
    public List<equipmentRecord> equipmentList { get; set; }
  }
  public class workcrew
  {
    public workcrew()
    {
      this.worker = String.Empty;
      this.billingCategory = String.Empty;
      this.startTime = String.Empty;
      this.endTime = String.Empty;
    }
    public string worker { get; set; }
    public string billingCategory { get; set; }
    public string startTime { get; set; }
    public string endTime { get; set; }
    public string lunchHour { get; set; }
    public string regularHours { get; set; }
    public string overTimeHours { get; set; }
    public string workedHours { get; set; }
  }
  public class equipmentRecord
  {
    public equipmentRecord()
    {
      this.equipment = String.Empty;
      this.notes = String.Empty;
    }
    public string equipment { get; set; }
    public int duration { get; set; }
    public int quantity { get; set; }
    public string notes { get; set; }
  }
}