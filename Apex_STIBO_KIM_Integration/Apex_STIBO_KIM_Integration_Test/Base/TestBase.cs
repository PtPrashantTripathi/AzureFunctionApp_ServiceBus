using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Apex_STIBO_KIM_Integration_Test
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        /// <summary>
        /// Configuration Instance
        /// </summary>
        private IConfiguration _config;

        /// <summary>
        /// Configuration Public property        
        /// </summary>
        protected IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder();
                    _config = builder.Build();
                }
                return _config;
            }
        }
        protected Message BuildMessage(string messageBody)
        {
            Message message = new(Encoding.UTF8.GetBytes(messageBody));
            message.UserProperties["TimeStampUTC"] = DateTime.UtcNow;
            return message;
        }
    }
}
