class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("\nEnter Base URL:");
        string baseURL = Console.ReadLine();        // http://localhost:5097/ - Sample URL

        int ProductId = 0;
        int ModifiedBy = 2;

        Console.WriteLine("\nAPI Check Utility\n");

        ApiCaller apiCaller = new ApiCaller();

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
        var apiResponse = await apiCaller.CallApi("Post", baseURL + "api/products", jsonData);

        Console.WriteLine("API Response:");
        Console.WriteLine(apiResponse);

        if (apiResponse != "0" && !apiResponse.Contains("API Failed"))
        {
            ProductId = Convert.ToInt32(apiResponse);
            Console.WriteLine("Product added successfully!");
        }
        else
        {
            Console.WriteLine("Error while adding product!");
            return;
        }

        Console.WriteLine("\n");

        //GET API call by ProductId and ModifiedBy
        Console.WriteLine("Endpoint: Get " + baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);
        apiResponse = await apiCaller.CallApi("Get", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);

        Console.WriteLine("API Response:");
        Console.WriteLine(apiResponse);

        if (apiResponse != "0" && !apiResponse.Contains("API Failed"))
        {
            Console.WriteLine("Product fetched successfully!");
        }
        else
        {
            Console.WriteLine("Error while fetching product by ProductId!");
            return;
        }

        Console.WriteLine("\n");

        //GET API call by ModifiedBy
        Console.WriteLine("Endpoint: Get " + baseURL + "api/products/?modifiedBy=" + ModifiedBy);
        apiResponse = await apiCaller.CallApi("Get", baseURL + "api/products/?modifiedBy=" + ModifiedBy);

        Console.WriteLine("API Response:");
        Console.WriteLine(apiResponse);

        if (apiResponse != "0" && !apiResponse.Contains("API Failed"))
        {
            Console.WriteLine("Products fetched successfully!");
        }
        else
        {
            Console.WriteLine("Error while fetching product by Email!");
            return;
        }
      
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
        apiResponse = await apiCaller.CallApi("Put", baseURL + "api/products/" + ProductId, jsonData);

        Console.WriteLine("API Response:");
        Console.WriteLine(apiResponse);

        if (apiResponse != "0" && !apiResponse.Contains("API Failed"))
        {
            Console.WriteLine("Product updated successfully!");
        }
        else
        {
            Console.WriteLine("Error while updating product!");
            return;
        }

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

        apiResponse = await apiCaller.CallApi("Patch", baseURL + "api/products/" + ProductId, jsonData);

        Console.WriteLine("API Response:");
        Console.WriteLine(apiResponse);

        if (apiResponse != "0" && !apiResponse.Contains("API Failed"))
        {
            Console.WriteLine("Product updated successfully!");
        }
        else
        {
            Console.WriteLine("Error while updating product!");
            return;
        }

        Console.WriteLine("\n");

        //DELETE API call
        Console.WriteLine("Endpoint: Delete " + baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);
        apiResponse = await apiCaller.CallApi("Delete", baseURL + "api/products/" + ProductId + "?modifiedBy=" + ModifiedBy);

        Console.WriteLine("API Response:");
        Console.WriteLine(apiResponse);

        if (apiResponse.ToLower() == "true" && !apiResponse.Contains("API Failed"))
        {
            Console.WriteLine("Product deleted successfully!");
        }
        else
        {
            Console.WriteLine("Error while deleting product!");
            return;
        }
        
        Console.WriteLine("\nAll the API Endpoints verified successfully!\n");
    }
}