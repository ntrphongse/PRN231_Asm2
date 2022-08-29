using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eBookStoreLibrary
{
    public class ODataResponse<T> where T : class
    {
        public T value { get; set; }
    }
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }

    public static class eBookStoreClientUtils
    {
        private static readonly HttpClient client = new HttpClient();

        private static JsonSerializerOptions DateTimeConverter
        {
            get
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new DateTimeConverter());
                return options;
            }
        }

        public static async Task<HttpResponseMessage> ApiRequest(eBookStoreHttpMethod method, string apiUrl, object bodyExtra = null)
        {
            switch (method)
            {
                case eBookStoreHttpMethod.GET:
                    return await client.GetAsync(apiUrl);
                case eBookStoreHttpMethod.POST:
                    return await client.PostAsJsonAsync(apiUrl, bodyExtra, DateTimeConverter);
                case eBookStoreHttpMethod.PUT:
                    return await client.PutAsJsonAsync(apiUrl, bodyExtra, DateTimeConverter);
                case eBookStoreHttpMethod.DELETE:
                    return await client.DeleteAsync(apiUrl);
                default:
                    return null;
            }
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            var claims = user.Claims;
            var role = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
            if (role.Value.Equals("ADMIN"))
            {
                return true;
            }
            return false;
        }

        public static async Task<T> ReadODataAsAsync<T>(this HttpResponseMessage responseMessage) where T : class
        {
            ODataResponse<T> temp = await responseMessage.Content.ReadAsAsync<ODataResponse<T>>();
            return temp.value;
        }
    }
}
