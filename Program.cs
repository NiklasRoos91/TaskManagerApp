using TaskManagerApp.Classes;

namespace TaskManagerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuHandler menuHandler = new MenuHandler();

            menuHandler.MainMenu();
        }
    }
}
