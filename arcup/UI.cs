using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arcup
{
    public class UI
    {
        #region Constants

        public enum Options
        {
            Error        = -1,
            Exit         = 0,
            Download     = 1,
            DownloadAll  = 2
        }

        #endregion Constants


        #region Methods

        /// <summary>
        /// Shows the startup message for the application
        /// </summary>
        /// <param name="defaultOption">Default option</param>
        public void Startup(Options defaultOption)
        {
            string header = string.Format(
                "Welcome to ArcUp\n" +
                "Latest version of ArcDps found: {0}\n\n" +
                "Select an option to continue:\n" +
                "1. Download latest version\n" +
                "2. Download latest version (Build templates included)\n" +
                "0. Exit", ArcUp.GetArcVersion().ToString("yyyy-MM-dd"));

            Console.WriteLine(header);

            // Waits for the user to introduce a valid value if default value was not given
            int opt = (int)defaultOption;

            while (opt == (int)Options.Error)
            {
                opt = ToInt(Console.ReadLine());

                if (opt == -1)
                    Console.Error.WriteLine("Invalid value. Please try again");
            }

            StartupCallback((Options)opt);
        }


        /// <summary>
        /// Callback for the startup method after the user introduces an option
        /// </summary>
        /// <param name="opt">option to be executed. Available options are defined in the "OPTIONS" top-level enum</param>
        private void StartupCallback(Options opt)
        {
            switch (opt)
            {
                case Options.Exit:
                    Environment.Exit(0);
                    break;

                case Options.Download:
                    ArcUp.DownloadBin(ArcUp.ARC_DOWNLOAD_URL);

                    ArcUp.StartGW2();
                    break;

                case Options.DownloadAll:
                    ArcUp.DownloadBin(ArcUp.ARC_DOWNLOAD_URL);
                    ArcUp.DownloadBin(ArcUp.ARC_DOWNLOAD_URL_BUILD);

                    ArcUp.StartGW2();
                    break;
            }
        }


        /// <summary>
        /// Converts a string to int safely
        /// </summary>
        /// <param name="input">String to be converted</param>
        /// <param name="defaultValue">Default value if cannot convert string</param>
        /// <returns>Integer representation of the string</returns>
        private int ToInt(string input, int defaultValue = -1)
        {
            // If couldn't convert, return defaultValue
            if (!int.TryParse(input, out int output))
                return defaultValue;

            return output;
        }

        #endregion Methods
    }
}
