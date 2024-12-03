using Spectre.Console;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagerApp.Classes
{
    public class JSONHandler
    {
        [JsonPropertyName("tasks")]

        public List<Task> AllTasksFromJSON { get; set; } = new List<Task>();

        public string filePathJSON = "JSON/TaskManagerData.json";

        public void LoadDataFromFile()
        {
            try
            {

                if (!File.Exists(filePathJSON) || new FileInfo(filePathJSON).Length == 0)
                {
                    AllTasksFromJSON = new List<Task>();
                    SaveDataToFile();
                    return;
                }

                if (File.Exists(filePathJSON))
                {
                    string allJSONData = File.ReadAllText(filePathJSON);

                    var loadedDataFromJSON = JsonSerializer.Deserialize<JSONHandler>(allJSONData);

                    if (loadedDataFromJSON != null)
                    {
                        AllTasksFromJSON = loadedDataFromJSON.AllTasksFromJSON ?? new List<Task>();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]JSON-fil hittades inte.");
                        AllTasksFromJSON = new List<Task>();
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]JSON-fil hittades inte.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid laddning av JSON-data: {ex.Message}[/]");

            }
        }

        public void SaveDataToFile()
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
