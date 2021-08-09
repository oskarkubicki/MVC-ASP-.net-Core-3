namespace apbd3
{
    public class HelloWorldOptions
    {
        public HelloWorldOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}