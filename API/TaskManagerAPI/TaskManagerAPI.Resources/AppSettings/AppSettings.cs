namespace TaskManagerAPI.Resources.AppSettings
{
    /// <summary>
    /// Class to model the AppSettings JSON file
    /// </summary>
    public class AppSettings
    {
        public string Secret { get; set; }
        public bool UseInMemoryDB { get; set; }
    }
}
