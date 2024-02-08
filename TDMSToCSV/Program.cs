Console.WriteLine("1. Process single TDMS file and export to CSV");
Console.WriteLine("2. Process multiple TDMS files and export to CSV");

var option = Console.ReadLine();

if (option == "1")
{
    TDMSToCSV.ReadSingleTDMSAndExportToCSV.Run();
}
else if(option == "2")
{
    TDMSToCSV.ReadFolderOfTDMSAndExportToCSV.Run();
}
else
{
    Console.WriteLine("Invalid option");
}
