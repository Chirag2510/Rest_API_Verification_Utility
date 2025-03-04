using OfficeOpenXml;

public class DotnetAPICheck
{
    public async Task DotnetAPIVerification(string baseURL, string filePath, ExcelPackage package, ExcelWorksheet worksheet)
    {
        HttpResponseMessage response = null;

        int ProductId = 0;
        int ModifiedBy = 2;

        ApiCaller apiCaller = new ApiCaller();
        UpdateExcel updateExcel = new UpdateExcel();

        Console.WriteLine("\n");

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
        
        ProductId = await updateExcel.UpdateResponseToExcel(worksheet, response, "Post", baseURL + "api/products", 1, jsonData);

        Console.WriteLine("\n");

        //GET API call by ProductId and ModifiedBy
        Console.WriteLine("Endpoint: Get " + baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);
        response = await apiCaller.CallApi("Get", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);

        await updateExcel.UpdateResponseToExcel(worksheet, response, "Get", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy, 2);

        Console.WriteLine("\n");

        //GET API call by ModifiedBy
        Console.WriteLine("Endpoint: Get " + baseURL + "api/products/?modifiedBy=" + ModifiedBy);
        response = await apiCaller.CallApi("Get", baseURL + "api/products/?modifiedBy=" + ModifiedBy);

        await updateExcel.UpdateResponseToExcel(worksheet, response, "Get", baseURL + "api/products/?modifiedBy=" + ModifiedBy, 3);
      
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

        await updateExcel.UpdateResponseToExcel(worksheet, response, "Put", baseURL + "api/products/" + ProductId, 4, jsonData);

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

        await updateExcel.UpdateResponseToExcel(worksheet, response, "Patch", baseURL + "api/products/" + ProductId, 5, jsonData);

        Console.WriteLine("\n");

        //DELETE API call
        Console.WriteLine("Endpoint: Delete " + baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);
        response = await apiCaller.CallApi("Delete", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);

        await updateExcel.UpdateResponseToExcel(worksheet, response, "Delete", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy, 6);
        
        await Task.Run(() => 
        {
            try
            {
                // Save the package
                package.SaveAs(new FileInfo(filePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError while saving the data in Excel file. Technical Error: " + ex.Message);
                throw;
            }
            
            Console.WriteLine("\nExcel file created and data added successfully!");
        });

        Console.WriteLine("\nAll the API Endpoints are verified, Please check the results in Excel file!\n");
    }
}