using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;
using GenericStoreApp.Models;
using System.Security.Policy;

namespace GenericStoreApp.Service
{
    public class FakeStoreApi
    {
        public static async Task<List<FakeProduct>?> GetAllFakeProductsApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("https://fakestoreapi.com/products/");
                    if (response != null)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<FakeProduct>>(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}