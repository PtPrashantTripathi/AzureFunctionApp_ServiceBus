using Azure.Storage.Files.DataLake;
using Apex_STIBO_KIM_Integration.Data.Interface;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Apex_STIBO_KIM_Integration
{
    /// <summary>
    /// AdlsAdapter class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AdlsAdapter : IAdlsAdapter
    {
        #region Private variables
        private readonly DataLakeServiceClient dataLakeServiceClient;
        private readonly string containerName = Environment.GetEnvironmentVariable(Constants.Container_Name);
        private ILoggerAdapter Logger { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_datalakeserviceClient"></param>
        /// <param name="loggerAdapter"></param>
        public AdlsAdapter(DataLakeServiceClient _datalakeserviceClient, ILoggerAdapter loggerAdapter)
        {
            this.dataLakeServiceClient = _datalakeserviceClient;
            Logger = loggerAdapter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Connect and update data file into adls raw layer
        /// </summary>
        /// <param name="PosData"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public async Task CreateFileAsync(string PosData, string FilePath, string FileName)
        {
            try
            {
                if (Logger != null)
                    Logger.LogInformation($"AdlsAdapter Started Processing FileName : {FileName}");

                // Connecting to zone
                DataLakeFileSystemClient fileSystemClient = dataLakeServiceClient.GetFileSystemClient(containerName);

                if (fileSystemClient != null)
                {
                    // Create Directory if needed
                    await fileSystemClient.CreateDirectoryAsync(FilePath);

                    // Connect to directory
                    DataLakeDirectoryClient directoryClient = fileSystemClient.GetDirectoryClient(FilePath);

                    // Create File Client
                    DataLakeFileClient fileClient = await directoryClient.CreateFileAsync(FileName);
                    if (fileClient != null)
                    {
                        // Convert json property into a stream
                        byte[] byteArray = Encoding.ASCII.GetBytes(PosData);
                        Stream stream = new MemoryStream(byteArray);

                        // Write the stream to the file and close
                        await fileClient.AppendAsync(stream, 0);

                        fileClient.Flush(stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    Logger.LogError(ex, $"Custom Message#Apex_STIBO_KIM_Integration/AdlsAdapter : Failed! => Unable to process CreateFileAsync function. {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}

