using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using CosmosTableSamples.Model;
using System.Linq;
using System.Collections.Generic;

namespace Company.Function
{
    public static class TimerTriggerMonitorDeClases
    {
        [FunctionName("TimerTriggerMonitorDeClases")]
        public static async Task Run([TimerTrigger("5 0/5 5-23 * * *")]TimerInfo myTimer, ILogger log)
        {
            CloudStorageAccount storageAccount;
            string storageConnectionString=Environment.GetEnvironmentVariable("TableStorageConnectionString");
            string minutos=DateTime.Now.Minute<10 && DateTime.Now.Hour>0?"0"+DateTime.Now.Minute.ToString(): DateTime.Now.Minute.ToString();
            string hora=DateTime.Now.Hour>0?DateTime.Now.Hour.ToString():"";
            string horaActual=hora+minutos;
            string fechaActual = DateTime.Now.ToString("MM/dd/yyyy HH:mm")+":00";
            string fechaFin=DateTime.Now.ToString("MM/dd/yyyy");
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                CloudTable table = tableClient.GetTableReference(Environment.GetEnvironmentVariable("NombreTablaClases"));
                //IQueryable<ClassEntity> linqQuery = table.CreateQuery<ClassEntity>()
                //.Where(x => x.horainiciosesionzoom == horaActual && x.indhorarioviernes!="NaN" )
                //.Select(x => new ClassEntity() { PartitionKey = x.PartitionKey, RowKey = x.RowKey, claveejercicioacademico = x.claveejercicioacademico, 
                //                                correoelectronicoprofesor=x.correoelectronicoprofesor, crn=x.crn, horafinclase=x.horafinclase, horainicioclase=x.horainicioclase,
                //                                horainiciosesionzoom=x.horainiciosesionzoom, indhorariodomingo=x.indhorariodomingo,indhorariolunes=x.indhorariolunes,
                //                                indhorariomartes=x.indhorariomartes,indhorariomiercoles=x.indhorariomiercoles,indhorariojueves=x.indhorariojueves,
                //                                indhorarioviernes=x.indhorarioviernes,indhorariosabado=x.indhorariosabado});
                
                //
                //var query = from entity in table.CreateQuery<ClassEntity>()  
                //where entity.horainiciosesionzoom == horaActual   && entity.indhorarioviernes!="NaN"
                //select entity;  
                List<ClassEntity>subitems;
                if (DateTime.Now.DayOfWeek==DayOfWeek.Monday){
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorariolunes!="NaN").ToList();
                }
                else if (DateTime.Now.DayOfWeek==DayOfWeek.Tuesday){
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorariomartes!="NaN").ToList();
                }
                else if (DateTime.Now.DayOfWeek==DayOfWeek.Wednesday){
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorariomiercoles!="NaN").ToList();
                }
                else if (DateTime.Now.DayOfWeek==DayOfWeek.Thursday){
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorariojueves!="NaN").ToList();
                }
                else if (DateTime.Now.DayOfWeek==DayOfWeek.Friday){
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorarioviernes!="NaN").ToList();
                }
                else if (DateTime.Now.DayOfWeek==DayOfWeek.Saturday){
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorariosabado!="NaN").ToList();
                }
                else{
                 subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default" && e.horainiciosesionzoom == horaActual && e.indhorariodomingo!="NaN").ToList();
                }


                //subitems =  table.CreateQuery<ClassEntity>().AsQueryable<ClassEntity>().Where(e=>e.PartitionKey=="Default").ToList();
                
                
                List<string> dbOperations = new List<string>();
                foreach (var clase in subitems)
                {
                    string minutosFin=clase.horafinclase.Length>3?clase.horafinclase.Substring(2): clase.horafinclase.Length>2?clase.horafinclase.Substring(1): clase.horafinclase;
                    string horaFin=clase.horafinclase.Length>3?clase.horafinclase.Substring(0,2): clase.horafinclase.Length>2?clase.horafinclase.Substring(0,1):"0";
                    //if (clase.horainiciosesionzoom==horaActual){}
                    dbOperations.Add($"EXEC insertarClase @inicio_zoom='{fechaActual}', @fin_clase='{fechaFin+" "+horaFin+":"+minutosFin+":00"}', @inicio_meeting='{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}', @crn={clase.crn}, @term={clase.claveejercicioacademico}, @correo='{clase.correoelectronicoprofesor}', @correolike='{clase.correoelectronicoprofesor.Split('@')[0]}@%'");
                    //Console.WriteLine(dbOperations[dbOperations.Count-1]);
                }
                dbOperations.Add($"EXEC eliminarClasesPasadas @hora_fin='{fechaActual}'");
                //Console.WriteLine($"\n\n\n\n\nLength de lista: {dbOperations.Count}\nParametro: {DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString()}");
                
                // Get the connection string from app settings and use it to create a connection.
                var str = Environment.GetEnvironmentVariable("sqldb_connection");

                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    //var text = "UPDATE SalesLT.SalesOrderHeader " +
                    //        "SET [Status] = 5  WHERE ShipDate < GetDate();";

                    //using (SqlCommand cmd = new SqlCommand(text, conn))
                    //{
                        // Execute the command and log the # rows affected.
                        //var rows = await cmd.ExecuteNonQueryAsync();
                        //log.LogInformation($"{rows} rows were updated");
                    //}
                    SqlTransaction transaction = conn.BeginTransaction();
                    foreach (string commandString in dbOperations)
                    {
                        //Console.WriteLine("Ejecutando:\n");
                        //Console.WriteLine(commandString);
                        SqlCommand cmd = new SqlCommand(commandString, conn, transaction);
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();

                }
                Console.WriteLine($"Se Ejecutaron correctamente: {dbOperations.Count} queries.");
                log.LogInformation($"Se Ejecutaron correctamente: {dbOperations.Count} queries a las {horaActual}.");
            }
            catch (FormatException)
            {
                Console.WriteLine("\n\n\n\n\n\nInvalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }


            //log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            

        }
    }
}
