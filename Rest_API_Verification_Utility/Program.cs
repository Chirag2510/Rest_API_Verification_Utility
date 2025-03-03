using OfficeOpenXml;

class Program
{
    static async Task Main(string[] args)
    {

        string assignment_type = "DotNet_React";
        ExcelPackage package;
        ExcelWorksheet worksheet;
        string filePath;

        // Define the file path
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data.xlsx");

        // Create new Excel package
        package = new ExcelPackage();

        // Add a new worksheet
        worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // Add headers
        worksheet.Cells[1, 1].Value = "Assignment Type";
        worksheet.Cells[1, 2].Value = "Globant Email";
        worksheet.Cells[1, 3].Value = "Execution Date";
        worksheet.Cells[1, 4].Value = "Endpoint";
        worksheet.Cells[1, 5].Value = "Http Verb";
        worksheet.Cells[1, 6].Value = "Request Payload";
        worksheet.Cells[1, 7].Value = "Response";
        worksheet.Cells[1, 8].Value = "Response Code";
        worksheet.Cells[1, 9].Value = "Message";

        // Set the same value in the specified range
        worksheet.Cells[$"A2:A7"].Value = assignment_type;
        worksheet.Cells[$"C2:C7"].Value = DateTime.Now.ToString("yyyy-MM-dd");

        //Get the base URL of backend server from user
        Console.WriteLine("\nEnter Base URL:");
        string baseURL = Console.ReadLine();        // http://localhost:5097/ - Sample URL

        //DotNet Assignment API Verification
        DotnetAPICheck dotnetAPICheck = new DotnetAPICheck();
        await dotnetAPICheck.DotnetAPIVerification(baseURL, filePath, package, worksheet);
    
    }
}