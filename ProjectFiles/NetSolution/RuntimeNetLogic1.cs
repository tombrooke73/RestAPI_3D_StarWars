#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.Core;
using FTOptix.CoreBase;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
#endregion

public class RuntimeNetLogic1 : BaseNetLogic
{
    private Label classLabel, manufacturerLabel, costLabel, hyperdriveLabel;
    public override void Start()
    {
        classLabel = Owner.Get<Label>("Class");
        manufacturerLabel = Owner.Get<Label>("Manufacturer");
        costLabel = Owner.Get<Label>("Cost");
        hyperdriveLabel = Owner.Get<Label>("Hyperdrive");
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void GetStarWarsShips(int id)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync($@"https://swapi.dev/api/starships/{id}/").Result;
        response.EnsureSuccessStatusCode();
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<StarShips>(responseBody);
        SetLabels(result.starship_class, result.manufacturer, result.cost_in_credits, result.hyperdrive_rating);
        Load3D(id);
    }

    private void SetLabels(string classValue, string manufacturerValue, string costValue, string hyperdriveValue)
    {
        string value = classValue.Replace("\"", "");
        classLabel.Text = value;
        manufacturerLabel.Text = manufacturerValue.Replace("\"", "");
        costLabel.Text = costValue.Replace("\"", "");
        hyperdriveLabel.Text = hyperdriveValue.Replace("\"", "");
    }

    private void Load3D(int id)
    {
        Owner.Get<WebBrowser>("WebBrowser").Visible = false;
        //String projectPath = (ResourceUri.FromProjectRelativePath("").Uri);
        //String folderSeparator = Path.DirectorySeparatorChar.ToString();

        // Get template name and create destination path
        string templatePath = @"C:\3d Test\Template.html";
        string filePath = @"C:\3d Test\index.html";

        // Read template page content
        string text = File.ReadAllText(templatePath);

        // Insert values
        switch (id)
        {
            case 9:
                text = text.Replace("$file", "deathstar");
                break;
            case 10:
                text = text.Replace("$file", "milleniumfalcon");
                break;
            case 3:
                text = text.Replace("$file", "star-destroyer");
                break;
            case 13:
                text = text.Replace("$file", "tie-interceptor");
                break;
            case 12:
                text = text.Replace("$file", "x-wing");
                break;
            default:
                break;
        }
        
        // Write to file
        File.WriteAllText(filePath, text);

        // Refresh WebBrowser page
        Owner.Get<WebBrowser>("WebBrowser").Refresh();
        Owner.Get<WebBrowser>("WebBrowser").Visible = true;
    }

    public class StarShips
    {
        public string name { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public string cost_in_credits { get; set; }
        public string length { get; set; }
        public string max_atmosphering_speed { get; set; }
        public string crew { get; set; }
        public string passengers { get; set; }
        public string cargo_capacity { get; set; }
        public string consumables { get; set; }
        public string hyperdrive_rating { get; set; }
        public string MGLT { get; set; }
        public string starship_class { get; set; }
        public string[] pilots { get; set; }
        public string[] films { get; set; }
        public string created { get; set; }

    }

}
