using Newtonsoft.Json;
using System.Text;

namespace RestaurantAPI.IntegrationTests.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent ToJsonHttpContent(this object obj)
        {
            var jsonModel = JsonConvert.SerializeObject(obj);

            var httpContent = new StringContent(jsonModel, UnicodeEncoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
