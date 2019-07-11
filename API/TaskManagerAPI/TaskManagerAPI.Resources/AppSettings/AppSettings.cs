namespace TaskManagerAPI.Resources.AppSettings
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public bool UseInMemoryDB { get; set; }
        public string ConnectionString { get; set; }
    }
}
