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

            var uvaChannel = UVAChannelBuilder.Build(tdmsFileName);
            UVAChannelToCSV.Export(uvaChannel, UVAChannelExportMode.Raw, rawCSVFileName);
            UVAChannelToCSV.Export(uvaChannel, UVAChannelExportMode.NSamplesPerSecond, subSampledCSVFileName);
            UVAChannelToCSV.Export(uvaChannel, UVAChannelExportMode.Summary, summaryCSVFileName);

            Console.WriteLine($"Files exported to {outDir}\n");
        }
    }
}
