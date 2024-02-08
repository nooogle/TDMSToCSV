namespace TDMSToCSV
{
    static class ReadFolderOfTDMSAndExportToCSV
    {
        public static void Run()
        {
            Console.WriteLine("Paste the full path to the TDMS data file folder");
            Console.Write("> ");
            var fullPath = Console.ReadLine().Trim('"').Trim();
            if(!Directory.Exists(fullPath)) { return; }

            foreach(var tdmsFileName in Directory.EnumerateFiles(fullPath, "*.tdms"))
            {
                ProcessFile.Go(tdmsFileName);
            }

            Console.Write("All done - press any key to exit >");
            Console.ReadLine();
        }
    }
}
