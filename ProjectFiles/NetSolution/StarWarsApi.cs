#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.Core;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using System.Net.Http;
using Newtonsoft.Json;
#endregion

public class StarWarsApi : BaseNetLogic
{
    [ExportMethod]
    public async void GetStarWarsShips()
    {
        var id = LogicObject.GetVariable("Id");
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($@"https://swapi.dev/api/starships/{id.Value.Value}/");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<StarShips>(responseBody);
        Log.Info(result.name + " - Manufacturer: " + result.manufacturer + " - Class: " + result.starship_class);
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

    [ExportMethod]
    public void Method2()
    {
        // Insert code to be executed by the method
    }
}

