namespace TDMSToCSV
{
    static class ReadSingleTDMSAndExportToCSV
    {
        public static void Run()
        {
            Console.WriteLine("Paste the full path to the TDMS data file");
            Console.Write("> ");
            var tdmsFileName = Console.ReadLine();
            ProcessFile.Go(tdmsFileName);

            Console.Write("All done - press any key to exit >");
            Console.ReadLine();
        }
    }
}
