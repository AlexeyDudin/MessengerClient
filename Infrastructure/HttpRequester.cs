using Domain.Dtos;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Infrastructure
{
    public static class HttpRequester
    {
        public static async Task<T> SendRequestAsync<T>(string url, HttpRequestType method, string loginDto = null, params object[] parameters) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                if (loginDto != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginDto);
                }
                var resultConnectionString = url;
                HttpResponseMessage response;
                switch (method)
                {
                    case HttpRequestType.GET:
                        foreach (var p in parameters)
                        {
                            if (p is string)
                            {
                                resultConnectionString += "/" + p;
                            }
                            else
                            {
                                resultConnectionString += "/" + JsonConvert.SerializeObject(p);
                            }
                        }
                        response = await client.GetAsync(resultConnectionString);
                        
                        break;
                    case HttpRequestType.POST:
                        JsonContent content = JsonContent.Create(parameters);
                        response = await client.PostAsync(url, content);
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<HttpResultDto>(content);
                        Type listType = typeof(T);
                        if (listType == typeof(string))
                        {
                            return res.value.ToString() as T;
                        }
                        return JsonConvert.DeserializeObject<T>(res.value.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                    throw new Exception($"Ошибка запроса по адресу {resultConnectionString}, код {response.StatusCode}");
            }
        }
    }
}
