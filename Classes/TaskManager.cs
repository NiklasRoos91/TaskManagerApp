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

            string newDescription;

            bool doesUserWantToAddDescription = AskUserForYesOrNo("Vill du lägga till en detaljerad beskrivning av uppgiften:");

            if (doesUserWantToAddDescription)
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
                    var checkbox = task.IsCompleted ? "[green]✔[/]" : "[red]✘[/]";

                    AnsiConsole.MarkupLine($"{checkbox} {task.Title} - {task.Description}");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Inga uppgifter finns.[/]");
            }
        }

        public void EditTask()
        {
            Task selectedTask = SelectTask("Vilken uppgift vill du ändra?");

            if (selectedTask != null)
            {
                string newTitle;
                bool doesUserWantToChangeTitle = AskUserForYesOrNo("Vill du ändra titeln:");

                if (doesUserWantToChangeTitle)
                {
                    newTitle = GetValidInput("Skriv in den nya titeln på uppgiften: ");

                    selectedTask.Title = newTitle;
                }

                string newDescription;
                bool doesUserWantToChangeDescription = AskUserForYesOrNo("Vill du ändra den detaljerad beskrivning av uppgiften:");

                if (doesUserWantToChangeDescription)
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

            Task selectedTask = SelectTask("Vilken uppgift vill du flytta?");

            if (selectedTask != null)
            {
                const string FirstPosition = "(Första Platsen)";

                string whereToMoveTask = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Välj uppgiften som ska ligga före den flyttade uppgiften (eller välj första platsen)")
                    .AddChoices(taskList.Select(task => task.Title).ToArray().Prepend(FirstPosition).ToList()));

                int currentIndex = taskList.IndexOf(selectedTask);
                taskList.RemoveAt(currentIndex);

                if (whereToMoveTask == FirstPosition)
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
            Task selectedTask = SelectTask("Vilken uppgift vill du ta bort?");

            if (selectedTask != null)
            {
                taskList.Remove(selectedTask);
                AnsiConsole.MarkupLine("[green] Uppgiften har raderats.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Ingen uppgift valdes.[/]");
            }
        }

        public Task SelectTask(string prompt)
        {
            if (taskList == null || taskList.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Det finns inga uppgifter att välja.[/]");
                return null;
            }

            string taskToSelect = AnsiConsole.Prompt(
                new SelectionPrompt<String>()
                .Title(prompt)
                .AddChoices(taskList.Select(task => task.Title).ToArray()));

            Task selectedTask = taskList.FirstOrDefault(task => task.Title == taskToSelect)!;

            if (selectedTask == null)
            {
                AnsiConsole.MarkupLine("[red]Den valda uppgiften kunde inte hittas.[/]");
                return null;
            }

            return selectedTask;
        }

        public bool AskUserForYesOrNo(string yesOrNoPrompt)
        {
                string userChoiceYesOrNo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(yesOrNoPrompt)
                .AddChoices(new[] { "Ja", "Nej" }));

            return userChoiceYesOrNo == "Ja";
        }

        public void MarkTaskAsComleted()
        {
            Task selectedTask = SelectTask("Vilken uppgift vill du markera som slutförd?");

            if (selectedTask != null)
            {
                selectedTask.IsCompleted = true;
                AnsiConsole.MarkupLine($"[green]Uppgiften '{selectedTask.Title}' är nu markerad som slutförd.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Den valda uppgiften kunde inte hittas.[/]");
            }
        }
    }
}