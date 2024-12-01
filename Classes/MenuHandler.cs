﻿using System.Diagnostics;
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

                TaskManager.ShowTasks();

                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[green]Välj ett alternativ:[/]")
                    .AddChoices("Lägg till uppgift", "Markera uppgift som slutförd",  "Ändra uppgift", "Flytta uppgift", "Ta bort uppgift", "Avsluta")
                );

                switch (userChoice)
                {
                    case "Lägg till uppgift":
                        TaskManager.AddTask();
                        break;
                    case "Markera uppgift som slutförd":
                        TaskManager.MarkTaskAsComleted(); 
                        break;
                    case "Ändra uppgift":
                        TaskManager.EditTask();
                        break;
                    case "Flytta uppgift":
                        TaskManager.MoveTask();
                        break;
                    case "Ta bort uppgift":
                        TaskManager.DeleteTask();
                        break;                        
                    case "Avsluta":
                        AnsiConsole.MarkupLine("[red]Avslutar...[/]");
                        return;
                    default:
                        AnsiConsole.MarkupLine("[yellow]Funktionen är ännu inte implementerad![/]");
                        break;
                }

                TaskManager.JSONHandler.SaveData();
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
