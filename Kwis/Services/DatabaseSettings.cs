using System;
namespace Kwis.Services
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CollectionNameQuiz { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string CollectionNameQuiz { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
