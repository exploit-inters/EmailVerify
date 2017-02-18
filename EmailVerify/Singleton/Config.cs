using System;
using System.Configuration;

namespace EmailVerify.Singleton
{
    /// <summary>
    /// Singleton: configuration class
    /// </summary>
    static class Config
    {
        #region --- Strongly typed settings ---
        // OWIN settings
        public static string OwinHost { get; private set; }
        #endregion

        static Config()
        {
            try
            {
                // Get OWIN settings
                OwinHost = ConfigurationManager.AppSettings["owin.host"];
            }
            catch (Exception ex)
            {
                Logger.Log.Fatal("Error at reading config: {0}", ex.Message);
                throw;
            }
        }
    }
}
