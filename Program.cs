using TaskManagerApp.Classes;

namespace TaskManagerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            MenuHandler menuHandler = new MenuHandler();

            menuHandler.MainMenu();
        }
    }
}
