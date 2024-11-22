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
                AnsiConsole.MarkupLine("[red]Inmatningen får inte vara tom eller endast innehålla mellanslag. Vänligen försök igen.[/]");
            }
        }

        public void MoveTask()
        {
            if (taskList.Count < 2)
            {
                AnsiConsole.MarkupLine("[red]Det finns inte tillräckligt många uppgifter för att flytta.[/]");
                return;
            }

            string taskToMove = AnsiConsole.Prompt(
                new SelectionPrompt<String>()
                .Title("Vilken uppgift vill du flytta?")
                .AddChoices(taskList.Select(task => task.Title).ToArray()));

            var selectedTask = taskList.FirstOrDefault(task => task.Title == taskToMove);

            if (selectedTask == null)
            {
                AnsiConsole.MarkupLine("[red]Den valda uppgiften kunde inte hittas.[/]");
                return;
            }


            if (selectedTask != null)
            {
                string whereToMoveTask = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Välj uppgiften som ska ligga före den flyttade uppgiften (eller välj första platsen)")
                    .AddChoices(taskList.Select(task => task.Title).ToArray().Prepend("(Första Platsen)").ToList()));



                taskList.Remove(selectedTask);

                if (whereToMoveTask == "(Första platsen)")
                {
                    taskList.Insert(0, selectedTask);
                }
                else
                {
                    int targetIndex = taskList.FindIndex(task => task.Title == whereToMoveTask);
                    taskList.Insert(targetIndex + 1, selectedTask);
                }
                AnsiConsole.MarkupLine($"[green]Uppgiften har flyttats.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Den valda uppgiften kunde inte hittas.[/]");
            }
        }

        public void DeleteTask()
        {
            string taskToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Vilken uppgift vill du ta bort?")
                    .AddChoices(taskList.Select(task => task.Title).ToArray()));

            var selectedTask = taskList.FirstOrDefault(task => task.Title == taskToDelete);

            if (selectedTask != null)
            {
                taskList.Remove(selectedTask);
                AnsiConsole.MarkupLine("[green] Uppgiften har raderats.[]");

            }
            else
            {
                AnsiConsole.MarkupLine("[red]Den valda uppgiften kunde inte hittas.[/]");
            }
        }
    }
}
