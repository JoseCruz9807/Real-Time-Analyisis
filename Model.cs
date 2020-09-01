namespace CosmosTableSamples.Model
{
    using Microsoft.Azure.Cosmos.Table;
    using Newtonsoft.Json;
    public class ClassEntity : TableEntity
    {
        public ClassEntity()
        {
        }

        public ClassEntity(string partition, string row)
        {
            PartitionKey = partition;
            RowKey = row;
        }
        public string claveejercicioacademico { get; set; }
        public string correoelectronicoprofesor { get; set; }
        public string crn { get; set; }
        public string horafinclase { get; set; }
        public string horainicioclase { get; set; }
        public string horainiciosesionzoom { get; set; }
        public string indhorariodomingo { get; set; }
        public string indhorariolunes { get; set; }
        public string indhorariomartes { get; set; }
        public string indhorariomiercoles { get; set; }
        public string indhorariojueves { get; set; }
        public string indhorarioviernes { get; set; }
        public string indhorariosabado { get; set; }
    }

    public class UserEnity : TableEntity
    {
        public UserEnity()
        {
        }

        public UserEnity(string partition, string row)
        {
            PartitionKey = partition;
            RowKey = row;
        }
        public string email { get; set; }
        public string id { get; set; }
        public string type{get; set;}
    }
}