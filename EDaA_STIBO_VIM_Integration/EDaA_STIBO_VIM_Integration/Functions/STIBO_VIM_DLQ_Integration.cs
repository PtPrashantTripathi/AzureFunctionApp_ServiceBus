using EDaA_STIBO_VIM_Integration.Data.Interface;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EDaA_STIBO_VIM_Integration
{
    /// <summary>
    /// STIBO_VIM_DLQ_Integration class
    /// </summary>
    public class STIBO_VIM_DLQ_Integration
    {
        #region Private Variables
        private readonly IAdlsAdapter DataAdapter;
        private ILoggerAdapter Logger { get; set; }
        private readonly string directoryPath = Environment.GetEnvironmentVariable(Constants.StiboVIM_DirectoryPath);
        #endregion

        #region Constructor
        /// <summary>
        /// STIBO_VIM_DLQ_Integration Constructors
        /// </summary>
        /// <param name="config"></param>
        /// <param name="adlsAdapter"></param>
        /// <param name="loggerAdapter"></param>
        public STIBO_VIM_DLQ_Integration(IConfiguration config, IAdlsAdapter adlsAdapter, ILoggerAdapter loggerAdapter)
        {
            DataAdapter = adlsAdapter;
            Logger = loggerAdapter;
        }
        #endregion

        #region Functions
        /// <summary>
        /// This Function fetches Product details from Topic
        /// </summary>
        /// <param name="MessageData"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("STIBO_VIM_DLQ_Integration")]
        public async Task Run([ServiceBusTrigger("%StiboVIM_ServiceBusTopic%" + "/" + "Subscriptions" + "/" + "%StiboVIM_ServiceBusSubscription%" + "%deadLetterQueuePath%", Connection = Constants.Service_Bus_URL)] Message MessageData, ILogger log)
        {
            try
            {
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
                var SequenceNumber = MessageData.SystemProperties.SequenceNumber;

                // Final File Name
                var FileName = $"{CreateGuid}_{SequenceNumber}.xml";

                // Get Message Body
                string ProductData = Encoding.UTF8.GetString(MessageData.Body);

                if (log != null)
                    log.LogInformation($"STIBO_VIM_DLQ_Integration - ServiceBus topic trigger Started Processing The messagesSequenceNumber : {SequenceNumber} ");

                // Write File to ADLS
                await DataAdapter.CreateFileAsync(ProductData, FilePath, FileName);

                if (log != null)
                    log.LogInformation("STIBO_VIM_DLQ_Integration - ServiceBus triggered successfully");
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    Logger.LogError(ex, $"Exception: Unable to write message STIBO_VIM_DLQ_Integration seen Exception {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
