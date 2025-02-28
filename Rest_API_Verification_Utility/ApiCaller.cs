using System.Text;
using System.Net;

public class ApiCaller
{
    public async Task<HttpResponseMessage> CallApi(string apiType, string url, string jsonData = null)
    {

        HttpResponseMessage response = null;
        try
        {
            using (var client = new HttpClient())
            {
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

                return response;
            }
        }
        catch (HttpRequestException e)
        {
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = e.Message
            };
            return response;
        }
        catch (Exception ex)
        {
            response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = ex.Message
            };
            return response;
        }
    }
}