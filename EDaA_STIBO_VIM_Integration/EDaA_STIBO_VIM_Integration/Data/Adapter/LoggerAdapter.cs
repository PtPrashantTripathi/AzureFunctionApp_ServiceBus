using EDaA_STIBO_VIM_Integration.Data.Interface;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EDaA_STIBO_VIM_Integration.Data.Adapter
{
    /// <summary>
    /// Telemetry logging for all activities
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggerAdapter : ILoggerAdapter
    {
        #region Constants
        private const string LOG_INFO_TEXT = "EDaA_STIBO_VIM_Integration";
        #endregion

        #region Properties
        /// <summary>
        /// Telemetry client
        /// </summary>
        TelemetryClient _appInsights { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="telemetryClient"></param>
        public LoggerAdapter(TelemetryClient telemetryClient)
        {
            _appInsights = telemetryClient;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="informationMessage"></param>
        public void LogInformation(string informationMessage)
        {
            try
            {
                var properties = GetLogInfo();
                _appInsights.TrackTrace(LOG_INFO_TEXT + " - " + informationMessage, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Log Information with body
        /// </summary>
        /// <param name="informationMessage"></param>
        /// <param name="body"></param>
        public void LogInformation(string informationMessage, string body)
        {
            try
            {
                var properties = GetLogInfo(body);
                _appInsights.TrackTrace(LOG_INFO_TEXT + " - " + informationMessage, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Log event without body
        /// </summary>
        /// <param name="informationMessage"></param>
        public void LogEvent(string informationMessage)
        {
            try
            {
                var properties = GetLogInfo();
                _appInsights.TrackEvent(LOG_INFO_TEXT + " - " + informationMessage, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Log event with a body / parameters
        /// </summary>
        /// <param name="informationMessage"></param>
        /// <param name="body"></param>
        public void LogEvent(string informationMessage, string body)
        {
            try
            {
                var properties = GetLogInfo(body);
                _appInsights.TrackEvent(LOG_INFO_TEXT + " - " + informationMessage, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Log error without body
        /// </summary>
        /// <param name="exception"></param>
        public void LogError(Exception exception)
        {
            try
            {
                var properties = GetLogInfo();
                _appInsights.TrackException(exception, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Log Error with a body / parameters
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="body"></param>
        public void LogError(Exception exception, string body)
        {
            try
            {
                var properties = GetLogInfo(body);
                _appInsights.TrackException(exception, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Log error with exception body and apiUrl
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="body"></param>
        /// <param name="apiUrl"></param>
        public void LogError(Exception exception, string body, string apiUrl)
        {
            try
            {
                var properties = GetLogInfo(body, apiUrl);
                _appInsights.TrackException(exception, properties);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the Log Info in Dictionary object
        /// </summary>
        /// <param name="body"></param>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetLogInfo(string body = "", string apiUrl = "")
        {
            try
            {
                Dictionary<string, string> properties = null;
                properties = new Dictionary<string, string>
                {
                    { "Application Log", LOG_INFO_TEXT }
                };

                if (!string.IsNullOrWhiteSpace(apiUrl))
                    properties.Add("URL", apiUrl);

                if (!string.IsNullOrWhiteSpace(body))
                {
                    var strBody = body.Split('|');
                    foreach (var item in strBody)
                    {
                        var strLog = item.Split('#');
                        if (strLog.Length > 1)
                            properties.Add(strLog[0], strLog[1]);
                        else
                            properties.Add("Additional Info", strLog[0]);
                    }
                }

                return properties;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
