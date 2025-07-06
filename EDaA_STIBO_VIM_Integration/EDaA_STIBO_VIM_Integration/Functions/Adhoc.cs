using EDaA_STIBO_VIM_Integration.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace EDaA_STIBO_VIM_Integration
{
    /// <summary>
    /// Adhoc class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Adhoc
    {
        #region Private Variables
        private readonly IAdlsAdapter DataAdapter;
        private ILoggerAdapter Logger { get; set; }
        private readonly string directoryPath = Environment.GetEnvironmentVariable(Constants.StiboVIM_DirectoryPath);
        #endregion


        #region Constructor
        /// <summary>
        /// test Constructors
        /// </summary>
        /// <param name="config"></param>
        /// <param name="adlsAdapter"></param>
        /// <param name="loggerAdapter"></param>
        public Adhoc(IConfiguration config, IAdlsAdapter adlsAdapter, ILoggerAdapter loggerAdapter)
        {
            DataAdapter = adlsAdapter;
            Logger = loggerAdapter;
        }
        #endregion

        #region Functions
        /// <summary>
        /// This Function fetches Product details via API insted of Topic
        /// </summary>
        /// <param name="req"></param>

        [FunctionName("stibo_vim_adhoc")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stibo_vim_adhoc")]
            HttpRequest req)
        {
            try
            {
                // Get message data from Request body
                var MessageData = await new StreamReader(req.Body).ReadToEndAsync();

                // Get Time in UTC
                var CurrentTime = DateTime.UtcNow;
                TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                // Change Time to Eastern Standard Time
                DateTime TargetTime = TimeZoneInfo.ConvertTime(CurrentTime, est);

                // Parse DateTime
                DateTime DateValue = Convert.ToDateTime(TargetTime.ToString());

                // Get Date, Month, Year and Hour
                var date = DateValue.Day.ToString();
                var month = DateValue.Month.ToString();
                var year = DateValue.Year.ToString();
                var hour = DateValue.Hour.ToString();

                // Create Directory Path
                var FilePath = $"{directoryPath}/Year={year}/Month={month}/Date={date}/Hour={hour}";

                // Create File Name
                var CreateGuid = (Guid.NewGuid().ToString());
                Random rand = new Random();
                var SequenceNumber = rand.Next(0, 10000); //MessageData.SystemProperties.SequenceNumber;

                // Final File Name
                var FileName = $"{CreateGuid}_{SequenceNumber}.xml";

                if (Logger != null)
                    Logger.LogInformation($"STIBO_VIM_DLQ_Integration - ServiceBus topic trigger Started Processing");

                if (req.Headers.ContainsKey("path"))
                {
                    String path = req.Headers["path"];
                    FileName = path.Split('/')[^1];
                    FilePath = path.Replace($"/{FileName}", "");
                }

                // Write File to ADLS
                await DataAdapter.CreateFileAsync(MessageData, FilePath, FileName);

                if (Logger != null)
                    Logger.LogInformation("STIBO_VIM_ADHOC - ServiceBus triggered successfully");

                // return success
                return new OkObjectResult("ok");
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    Logger.LogError(ex, $"Exception: Unable to write message STIBO_VIM_ADHOC seen Exception {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
