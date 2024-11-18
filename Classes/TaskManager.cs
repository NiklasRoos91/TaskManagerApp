using Spectre.Console;
using System.Net.WebSockets;
using static System.Reflection.Metadata.BlobBuilder;

namespace TaskManagerApp.Classes
{
    public class TaskManager
    {
        public JSONHandler JSONHandler { get; set; }
        public List<Task> taskList { get; set; } = new List<Task>();

        public TaskManager()
        {
            JSONHandler = new JSONHandler();
            JSONHandler.LoadDataFromFile();
            taskList = JSONHandler.AllTasksFromJSON;
        }

        public void AddTask()
        {

            string newTitle = "";

            while (string.IsNullOrWhiteSpace(newTitle))
            {
                newTitle = GetValidInput("Skriv in titeln på din nya uppgift:");
            }

            string newDescription = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                       .Title("Vill du lägga till en detaljerad beskrivning av uppgiften:")
                    .AddChoices(new[] { "Ja", "Nej" }));

            if (newDescription == "Ja")
            {
                newDescription = GetValidInput("Skriv in den detaljerade beskrivningen på uppgiften: ");
            }
            else
            {
                newDescription = "Ingen beskrivning tillagd";
            }

            Guid newID = Guid.NewGuid();

            taskList.Add(new Task(newTitle, newDescription, newID));
        }

        public void ShowTasks()
        {
            if (taskList.Count > 0)
            {
                AnsiConsole.MarkupLine("Uppgifter:");

                foreach (var task in taskList)
                {
                    AnsiConsole.MarkupLine($"{task.Title} - {task.Description}");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Inga uppgifter finns.[/]");
            }
        }

        public void EditTask()
        {
            string taskToEdit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Vilken uppgift vill du ändra?")
                    .AddChoices(taskList.Select(task => task.Title).ToArray()));

            var selectedTask = taskList.FirstOrDefault(task => task.Title == taskToEdit);

            if (selectedTask != null)
            {
                string newTitle = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Vill du ändra titeln:")
                        .AddChoices(new[] { "Ja", "Nej" }));

                if (newTitle == "Ja")
                {
                    newTitle = GetValidInput("Skriv in den nya titeln på uppgiften: ");

                    selectedTask.Title = newTitle;
                }

                string newDescription = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Vill du ändra den detaljerad beskrivning av uppgiften:")
                    .AddChoices(new[] { "Ja", "Nej" }));

                if (newDescription == "Ja")
                {
                    newDescription = GetValidInput("Skriv in den nya beskrivningen för uppgiften: ");

                    selectedTask.Description = newDescription;
                }

                AnsiConsole.MarkupLine("[green]Uppgiften har uppdaterats.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Den valda uppgiften kunde inte hittas.[/]");
            }
        }

        private string GetValidInput(string prompt)
        {
            while (true)
            {
            string input = AnsiConsole.Ask<string>(prompt);

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                Console.WriteLine("Inmatningen får inte vara tom eller endast innehålla mellanslag. Vänligen försök igen.");
            }
        }

        public void MoveTask()
        {

        }

        public void RemoveTask() { 
        }
    }
}
