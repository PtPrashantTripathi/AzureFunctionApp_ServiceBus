using System.Diagnostics.CodeAnalysis;

namespace EDaA_STIBO_VIM_Integration
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
        /// Adls StiboVIM Directory Path
        /// </summary>
        public const string StiboVIM_DirectoryPath = "StiboVIM_DirectoryPath";
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
