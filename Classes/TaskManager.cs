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
            try
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
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid tillägg av uppgift: {ex.Message}[/]");
            }
        }

        public void EditTask(Task selectedTask)
        {
            try
            {
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
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid ändring av uppgift: {ex.Message}[/]");
            }
        }

        private string GetValidInput(string prompt)
        {
            try
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
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid inmatning: {ex.Message}[/]");
                return string.Empty;
            }
        }

        public void MoveTask(Task selectedTask)
        {
            try
            {
                if (taskList.Count < 2)
                {
                    AnsiConsole.MarkupLine("[red]Det finns inte tillräckligt många uppgifter för att flytta.[/]");
                    return;
                }

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
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid flytt av uppgift: {ex.Message}[/]");
            }
        }
        public void DeleteTask(Task selectedTask)
        {
            try
            {
                if (selectedTask != null)
                {
                    bool confirmDelete = AskUserForYesOrNo($"Är du säker på att du vill ta bort uppgiften '{selectedTask.Title}'?");
                    if (confirmDelete)
                    {
                        taskList.Remove(selectedTask);
                        AnsiConsole.MarkupLine("[green] Uppgiften har raderats.[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Ingen uppgift valdes.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid radering av uppgift: {ex.Message}[/]");
            }
        }

        public void MarkTaskAsComleted(Task selectedTask)
        {
            try
            {
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
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid markering av uppgift som slutförd: {ex.Message}[/]");
            }
        }
        public bool AskUserForYesOrNo(string yesOrNoPrompt)
        {
            try
            {
                string userChoiceYesOrNo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(yesOrNoPrompt)
                .AddChoices(new[] { "Ja", "Nej" }));

                return userChoiceYesOrNo == "Ja";
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Fel vid fråga om ja/nej: {ex.Message}[/]");
                return false;
            }
        }
    }
}