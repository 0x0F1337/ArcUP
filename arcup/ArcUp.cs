using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace arcup
{
    class ArcUp
    {
        #region Constants

        const string DLL_FILENAME       = "d3d9.dll";
        const string DLL_FILENAME_BUILD = "d3d9_arcdps_buildtemplates.dll";

        const string DATE_REGEXP = "<a href=\"d3d9\\.dll\">.+<\\/a> +(2[0-9]*-[0-1][0-9]-[0-3][0-9])";

        const string ARC_REPO_URL                  = "https://www.deltaconnected.com/arcdps/x64/";
        public const string ARC_DOWNLOAD_URL       = "https://www.deltaconnected.com/arcdps/x64/d3d9.dll";
        public const string ARC_DOWNLOAD_URL_BUILD = "https://www.deltaconnected.com/arcdps/x64/buildtemplates/d3d9_arcdps_buildtemplates.dll";

        const int CHUNK_DOWNLOAD_SIZE = 1024;

        #endregion Constants


        #region Methods

        /// <summary>
        /// Gets the latest version (date) of ArcDPS found
        /// </summary>
        /// <returns>Date of the latest version of ArcDps found in the page</returns>
        public static DateTime GetArcVersion()
        {
            DateTime arcDate = default(DateTime);
            WebRequest req = WebRequest.Create(ARC_REPO_URL);

            using (StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string html = sr.ReadToEnd();

                // Searches for the date in the HTML retrieved
                Regex regxp = new Regex(DATE_REGEXP);
                string date = regxp.Match(html).Groups[1].Value;

                arcDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture); 
            }

            return arcDate;
        }


        /// <summary>
        /// Downloads a binary file
        /// </summary>
        /// <param name="downloadUrl">URL of the file to be downloaded</param>
        public static void DownloadBin(string downloadUrl)
        {
            WebRequest req = WebRequest.Create(downloadUrl);

            // Filename is gotten from the url itself
            var aux = downloadUrl.Split('/');
            string filename = aux[aux.Length - 1];

            Console.WriteLine(string.Format("Downloading from {0} please wait...", downloadUrl));

            using (BinaryReader br = new BinaryReader(req.GetResponse().GetResponseStream()))
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create, FileAccess.Write)))
                {
                    var chunk = br.ReadBytes(CHUNK_DOWNLOAD_SIZE);

                    while (chunk.Length > 0)
                    {
                        bw.Write(chunk);
                        chunk = br.ReadBytes(CHUNK_DOWNLOAD_SIZE);
                    }
                }
            }

            Console.WriteLine("Download completed");
        }

        #endregion Methods

    }
}
