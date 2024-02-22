
using System.Text;

namespace TDMSToCSV
{
    static class ProcessFile
    {
        public static void Go(string tdmsFileName)
        {
            tdmsFileName = tdmsFileName.Trim('"').Trim();
            
            if (!File.Exists(tdmsFileName)) 
            { 
                Console.WriteLine($"File not found: {tdmsFileName}");
                return; 
            }

            Console.WriteLine($"Processing {tdmsFileName}");

            var fileNamePart = Path.GetFileNameWithoutExtension(tdmsFileName);
            var now = DateTime.Now;
            var outDir = Path.Combine(Path.GetDirectoryName(tdmsFileName), $"{now:yyyy-MM-dd HH-mm-ss} {fileNamePart} extraction");
            Directory.CreateDirectory(outDir);
            var rawCSVFileName = Path.Combine(outDir, "raw.csv");
            var subSampledCSVFileName = Path.Combine(outDir, "Sub-sampled.csv");
            var summaryCSVFileName = Path.Combine(outDir, "Summary.csv");

            (var uvaChannel, var properties) = UVAChannelBuilder.Build(tdmsFileName);
            UVAChannelToCSV.Export(uvaChannel, UVAChannelExportMode.Raw, rawCSVFileName);
            UVAChannelToCSV.Export(uvaChannel, UVAChannelExportMode.NSamplesPerSecond, subSampledCSVFileName);
            UVAChannelToCSV.Export(uvaChannel, UVAChannelExportMode.Summary, summaryCSVFileName);
            AppendPropertiesToCSV(properties, summaryCSVFileName);

            Console.WriteLine($"Files exported to {outDir}\n");
        }

        private static void AppendPropertiesToCSV(IDictionary<string, object> properties, string summaryCSVFileName)
        {
            var csv = new StringBuilder();
            csv.AppendLine($"");

            foreach ( var property in properties )
            {
                csv.AppendLine($"{property.Key}, {property.Value}");                
            }

            File.AppendAllText(summaryCSVFileName, csv.ToString());
        }
    }
}
