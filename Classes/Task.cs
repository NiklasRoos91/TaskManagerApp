namespace TaskManagerApp.Classes
{
    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ID { get; set; }

        public Task(string title, string description, Guid iD)
        {
            Title = title;
            Description = description;
            ID = iD;
        }
    }
}
