using Spectre.Console;
using System.Net.WebSockets;
using static System.Reflection.Metadata.BlobBuilder;

namespace TaskManagerApp.Classes
{
    public class TaskManager
    {
        public List<Task> taskList { get; set; } = new List<Task>();

        public TaskManager()
        {
            new List<Task>();
        }

        public void AddTask()
        {

            string newTitle = "";

            while (string.IsNullOrWhiteSpace(newTitle))
            {
                newTitle = AnsiConsole.Ask<string>("Skriv in titeln på din nya uppgift:");

                Console.WriteLine($"Debug: Title entered: '{newTitle}'");


                if (string.IsNullOrWhiteSpace(newTitle))
                {
                    AnsiConsole.MarkupLine("[red]Inmatningen får inte vara tom. Vänligen ange ett giltigt värde.[/]");
                }
            }

            string newDescription = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                       .Title("Vill du lägga till en detaljerad beskrivning av uppgiften:")
                    .AddChoices(new[] { "Ja", "Nej" }));

            if (newDescription == "Ja")
            {
                newDescription = AnsiConsole.Ask<string>("Skriv in den detaljerade beskrivningen på uppgiften: ");

                while (string.IsNullOrWhiteSpace(newDescription))
                {
                    AnsiConsole.MarkupLine("[red]Inmatningen får inte vara tom. Vänligen ange ett giltigt värde.[/]");
                    newDescription = AnsiConsole.Ask<string>("Skriv in den detaljerade beskrivningen på uppgiften: ");
                }
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
                Console.WriteLine("Uppgifter:");
                
                foreach (var task in taskList)
                {
                    Console.WriteLine(task.Title);
                }
            }
            else
            {
                Console.WriteLine("Tom lista");
            }
        }
    }
}
