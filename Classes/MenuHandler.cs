using System.Diagnostics;
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
                //AnsiConsole.Write("Task Manager");
                AnsiConsole.Write("Task Manager\n");


                TaskManager.ShowTasks();

                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[green]Välj ett alternativ:[/]")
                    .AddChoices("Lägg till uppgift", "Ändra uppgift", "Flytta uppgift", "Ta bort uppgift", "Avsluta")
                );

                switch (userChoice)
                {
                    case "Lägg till uppgift":
                        TaskManager.AddTask();
                        break;
                    case "Avsluta":
                        Console.WriteLine("[red]Avslutar...");
                        return;
                    default:
                        AnsiConsole.MarkupLine("[yellow]Funktionen är ännu inte implementerad![/]");
                        break;
                }
            }
        }

    }
}
