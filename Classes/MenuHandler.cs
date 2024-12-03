using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

namespace TaskManagerApp.Classes
{
    public class MenuHandler
    {
        TaskManager TaskManager { get; set; }

        public MenuHandler()
        {
            TaskManager = new TaskManager();
        }

        public void MainMenu()
        {
            while (true)
            {
                AnsiConsole.Write("Task Manager\n");

                var menuChoices = new List<string> { "Lägg till en uppgift" };

                menuChoices.AddRange(TaskManager.taskList.Select(task =>
                    $"{(task.IsCompleted ? "[green]✔[/]" : "[red]✘[/]")} {task.Title}: {task.Description}"));

                if (TaskManager.taskList.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]Inga uppgifter att visa. Lägg till en uppgift för att komma igång![/]");
                }

                menuChoices.Add("Avsluta");

                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Vad vill du göra?")
                    .AddChoices(menuChoices));

                switch (userChoice)
                {
                    case "Lägg till en uppgift":
                        TaskManager.AddTask();
                        break;
                    case "Avsluta":
                        AnsiConsole.MarkupLine("[red]Avslutar...[/]");
                        return;
                    default:
                        var selectedTask = TaskManager.taskList.FirstOrDefault(task => userChoice.Contains(task.Title));

                        if (selectedTask != null)
                        {
                            HandleTaskMenu(selectedTask);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Ogiltigt val, försök igen.[/]");
                        }
                        break;
                }

                TaskManager.JSONHandler.SaveDataToFile();
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        public void HandleTaskMenu(Task selectedTask)
        {
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Välj vad du vill göra")
                .AddChoices("Markera uppgift som slutförd", "Ändra uppgift", "Flytta uppgift", "Ta bort uppgift", "Gå tillbaka"));

            switch (userChoice)
            {
                case "Markera uppgift som slutförd":
                    TaskManager.MarkTaskAsComleted(selectedTask);
                    break;
                case "Ändra uppgift":
                    TaskManager.EditTask(selectedTask);
                    break;
                case "Flytta uppgift":
                    TaskManager.MoveTask(selectedTask);
                    break;
                case "Ta bort uppgift":
                    TaskManager.DeleteTask(selectedTask);
                    break;
                case "Gå tillbaka":
                    break;
            }
        }
    }
}
