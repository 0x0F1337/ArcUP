using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace arcup
{
    class ArcUp
    {
        const string BIN_64_FOLDER = "bin64/";

        public const string ARC_DOWNLOAD_URL       = "https://www.deltaconnected.com/arcdps/x64/d3d9.dll";
        public const string ARC_DOWNLOAD_URL_BUILD = "https://www.deltaconnected.com/arcdps/x64/buildtemplates/d3d9_arcdps_buildtemplates.dll";

        const int CHUNK_DOWNLOAD_SIZE = 1024;


        /// <summary>
        /// Gets the latest version (date) of ArcDPS found
        /// </summary>
        /// <returns>Date of the latest version of ArcDps found in the page</returns>
        public static async Task<DateTime> GetArcVersion()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(ARC_DOWNLOAD_URL);

            return response.Content.Headers.LastModified.Value.Date;
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
                using (BinaryWriter bw = new BinaryWriter(File.Open(BIN_64_FOLDER + filename, FileMode.Create, FileAccess.Write)))
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


        public static void StartGW2()
        {
            Process.Start("Gw2-64.exe");
        }
    }
}
