using System.Text;
using System.Text.Json;
using OfficeOpenXml;

public class UpdateExcel
{
    public async Task<int> UpdateResponseToExcel(ExcelWorksheet worksheet, HttpResponseMessage response, string apiType, string url, int row_number, string jsonData = null)
    {
        var api_type = apiType.ToUpper();
        int productId = 0;

        var userEmail = "";
        if (response.Headers.TryGetValues("X-User-Email", out var emailValues))
        {
            userEmail = emailValues.FirstOrDefault();
            Console.WriteLine("X-User-Email: " + userEmail);
        }
        else
        {
            Console.WriteLine("Not getting the email");
        }

        var responseData = "";
        var message = "";
        var productIds = "";
        if (response.IsSuccessStatusCode)
        {
            responseData = await response.Content.ReadAsStringAsync();
            
            using(JsonDocument doc = JsonDocument.Parse(responseData)) 
            {
                JsonElement root = doc.RootElement;

                // Check if the root is an array
                if (root.ValueKind == JsonValueKind.Array)
                {
                    // Loop through each element in the array
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        productIds += element.GetProperty("productId") + ", ";
                    }

                    // Remove extra comma from end
                    productIds = productIds.Substring(0, productIds.Length - 2);

                } 
                else
                {
                    // Access ProductID and ProductName directly
                    productId = root.GetProperty("productId").GetInt32();
                    var productName = root.GetProperty("productName");
                }
                
                message =  api_type + " API Endpoint is verified successfully.";

                if (api_type == "POST") 
                {
                    message += "\nProduct with ProductID: " +productId + " is added.";
                }

                if (api_type == "GET" && productIds != "") 
                {
                    message += "\nProducts with ProductID: " +productIds+ " are retrieved.";
                } 
                else if (api_type == "GET") 
                {
                    message += "\nProduct with ProductID: " +productId+ " is retrieved.";
                }

                if (api_type == "PUT" || api_type == "PATCH") 
                {
                    message += "\nProduct with ProductID: " +productId + " is updated.";
                }

                if (api_type == "DELETE") 
                {
                    message += "\nProduct with ProductID: " +productId + " is deleted.";
                }

            }
        } else {
            message =  api_type + " API Endpoint verification failed.";
            message += "\nTechnical Error: " + response.ReasonPhrase; 
        }

        row_number = row_number + 1;
        
        await Task.Run( () => 
        {
            worksheet.Cells[row_number, 2].Value = userEmail;
            worksheet.Cells[row_number, 4].Value = url;
            worksheet.Cells[row_number, 5].Value = api_type;
            worksheet.Cells[row_number, 6].Value = jsonData;
            worksheet.Cells[row_number, 7].Value = responseData;
            worksheet.Cells[row_number, 8].Value = (int)response.StatusCode;
            worksheet.Cells[row_number, 9].Value = message;
        });
    
        Console.WriteLine("Excel file created and data added successfully!");

        return productId;
    }
}
