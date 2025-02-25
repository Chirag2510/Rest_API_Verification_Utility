using System.Text;

public class ApiCaller
{
    public async Task<string> CallApi(string apiType, string url, string jsonData = null)
    {
        try
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = null;
                var api_type = apiType.ToUpper();

                if (api_type == "POST" || api_type == "PUT" || api_type == "PATCH")
                {
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    if (api_type == "POST")
                    {
                        response = await client.PostAsync(url, content);
                    }
                    else if (api_type == "PUT")
                    {
                        response = await client.PutAsync(url, content);
                    }
                    else if (api_type == "PATCH")
                    {
                        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
                        response = await client.SendAsync(request);
                    }
                }
                else if (api_type == "GET")
                {
                    response = await client.GetAsync(url);
                }
                else if (api_type == "DELETE")
                {
                    response = await client.DeleteAsync(url);
                }
                else
                {
                    return "Invalid API Type";
                }

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "API Failed" + " API_Type:" + api_type + " Endpoint:" + url + " ResponseCode:" + response.StatusCode;
                }
                
            }
        }
        catch (Exception ex)
        {
            return "API Failed" + " API_Type:" + apiType + " Endpoint:" + url + " Exception:" + ex.Message;
        }
    }
}