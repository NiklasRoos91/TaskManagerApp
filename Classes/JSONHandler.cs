using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagerApp.Classes
{
    public class JSONHandler
    {
        [JsonPropertyName("tasks")]

        public List<Task> AllTasksFromJSON { get; set; } = new List<Task>();

        public string filePathJSON = "TaskManagerData.json";

        public void LoadDataFromFile()
        {
            if (File.Exists(filePathJSON))
            {
                string allJSONData = File.ReadAllText(filePathJSON);

                var loadedDataFromJSON = JsonSerializer.Deserialize<JSONHandler>(allJSONData);

                if (loadedDataFromJSON != null)
                {
                    AllTasksFromJSON = loadedDataFromJSON.AllTasksFromJSON ?? new List<Task>();
                }
            }
        }

        public void SaveData()
        {
            var JSONHandlerData = new JSONHandler
            {
                AllTasksFromJSON = AllTasksFromJSON,
            };

            string jsonOutput = JsonSerializer.Serialize(JSONHandlerData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePathJSON, jsonOutput);
        }
    }
}
