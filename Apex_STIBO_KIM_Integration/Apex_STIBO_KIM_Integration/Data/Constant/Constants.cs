using System.Diagnostics.CodeAnalysis;

namespace Apex_STIBO_KIM_Integration
{
    /// <summary>
    /// Holds all the information that are constant
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Constants
    {
        /// <summary>
        /// Adls Account Name
        /// </summary>
        public const string Account_Name = "AccountName";
        /// <summary>
        /// Adls StiboKIM Directory Path
        /// </summary>
        public const string StiboKIM_DirectoryPath = "StiboKIM_DirectoryPath";
        /// <summary>
        /// Name of Container
        /// </summary>
        public const string Container_Name = "ContainerName";
        /// <summary>
        /// Service Bus URL
        /// </summary>
        public const string Service_Bus_URL = "ServiceBusURL";
    }
}
