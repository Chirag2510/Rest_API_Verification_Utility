using OfficeOpenXml;

public class DotnetAPICheck
{

    private string _assignment_type;
    private ExcelPackage _package;
    private ExcelWorksheet _worksheet;
    private string _filePath;

    public DotnetAPICheck(string assignment_type)
    {
        _assignment_type = assignment_type;
        InitializeSheetsService();
    }

    private void InitializeSheetsService() 
    {

        // Define the file path
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data.xlsx");

        // Create new Excel package
        _package = new ExcelPackage();

        // Add a new worksheet
        _worksheet = _package.Workbook.Worksheets.Add("Sheet1");

        // Add headers
        _worksheet.Cells[1, 1].Value = "Assignment Type";
        _worksheet.Cells[1, 2].Value = "Globant Email";
        _worksheet.Cells[1, 3].Value = "Execution Date";
        _worksheet.Cells[1, 4].Value = "Endpoint";
        _worksheet.Cells[1, 5].Value = "Http Verb";
        _worksheet.Cells[1, 6].Value = "Request Payload";
        _worksheet.Cells[1, 7].Value = "Response";
        _worksheet.Cells[1, 8].Value = "Response Code";
        _worksheet.Cells[1, 9].Value = "Message";

        // Set the same value in the specified range
        _worksheet.Cells[$"A2:A7"].Value = _assignment_type;
        _worksheet.Cells[$"C2:C7"].Value = DateTime.Now.ToString("yyyy-MM-dd");

    }

    public async Task DotnetAPIVerification()
    {
        HttpResponseMessage response = null;

        Console.WriteLine("\nEnter Base URL:");
        string baseURL = Console.ReadLine();        // http://localhost:5097/ - Sample URL
        //string baseURL = "http://localhost:5097/";

        int ProductId = 0;
        int ModifiedBy = 2;

        Console.WriteLine("\nAPI Check Utility\n");

        ApiCaller apiCaller = new ApiCaller();
        UpdateExcel updateExcel = new UpdateExcel();

        //POST API call
        var postData = new { 
            ProductName = "Galaxy S25",
            Color = "Grey",
            CategoryId = 1,
            Price = 100000, 
            QuantityInStock = 10,
            BrandId = 2,
            ProductDescription = "Samsung Galaxy S25",
            ModifiedBy = ModifiedBy
        };
        var jsonData = System.Text.Json.JsonSerializer.Serialize(postData);

        Console.WriteLine("Endpoint: Post " + baseURL + "api/products");
        response = await apiCaller.CallApi("Post", baseURL + "api/products", jsonData);
        
        ProductId = await updateExcel.UpdateResponseToExcel(_worksheet, response, "Post", baseURL + "api/products", 1, jsonData);

        Console.WriteLine("\n");

        //GET API call by ProductId and ModifiedBy
        Console.WriteLine("Endpoint: Get " + baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);
        response = await apiCaller.CallApi("Get", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);

        await updateExcel.UpdateResponseToExcel(_worksheet, response, "Get", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy, 2);

        Console.WriteLine("\n");

        //GET API call by ModifiedBy
        Console.WriteLine("Endpoint: Get " + baseURL + "api/products/?modifiedBy=" + ModifiedBy);
        response = await apiCaller.CallApi("Get", baseURL + "api/products/?modifiedBy=" + ModifiedBy);

        await updateExcel.UpdateResponseToExcel(_worksheet, response, "Get", baseURL + "api/products/?modifiedBy=" + ModifiedBy, 3);
      
        Console.WriteLine("\n");
       
        //PUT API call
        Console.WriteLine("Endpoint: Put " + baseURL + "api/products/" + ProductId);

        var putData = new {
            ProductId = ProductId,
            ProductName = "Updated Galaxy S25",
            Color = "Black",
            CategoryId = 1,
            Price = 100000,
            QuantityInStock = 10,
            BrandId = 2,
            ProductDescription = "Updated Samsung Galaxy S25",
            ModifiedBy = ModifiedBy
        };

        jsonData = System.Text.Json.JsonSerializer.Serialize(putData);
        response = await apiCaller.CallApi("Put", baseURL + "api/products/" + ProductId, jsonData);

        await updateExcel.UpdateResponseToExcel(_worksheet, response, "Put", baseURL + "api/products/" + ProductId, 4, jsonData);

        Console.WriteLine("\n");

        //PATCH API call
        Console.WriteLine("Endpoint: Patch " + baseURL + "api/products/" + ProductId);

        var patchData = new object[]
        {
            new
            {
                op = "replace",
                path = "/ProductDescription",
                value = "Samsung Galaxy S25 updated by Patch"
            },
            new
            {
                op = "replace",
                path = "/ModifiedBy",
                value = ModifiedBy
            }
        };

        jsonData = System.Text.Json.JsonSerializer.Serialize(patchData);

        response = await apiCaller.CallApi("Patch", baseURL + "api/products/" + ProductId, jsonData);

        await updateExcel.UpdateResponseToExcel(_worksheet, response, "Patch", baseURL + "api/products/" + ProductId, 5, jsonData);

        Console.WriteLine("\n");

        //DELETE API call
        Console.WriteLine("Endpoint: Delete " + baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);
        response = await apiCaller.CallApi("Delete", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);

        await updateExcel.UpdateResponseToExcel(_worksheet, response, "Delete", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy, 6);
        
        Console.WriteLine("\nAll the API Endpoints are verified, Please check the results in Excel file!\n");

        await Task.Run(() => 
        {
            // Save the package
            _package.SaveAs(new FileInfo(_filePath));
        });
    }
}