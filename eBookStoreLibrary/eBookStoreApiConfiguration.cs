using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eBookStoreLibrary
{
    public static class eBookStoreApiConfiguration
    {
        #region Private Members to get Configuration
        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.api.json", true, true);
            return builder.Build();
        }
        #endregion

        public static string ConnectionString => GetConfiguration().GetConnectionString("eStoreDb");

        public static string DefaultAccount => JsonSerializer.Serialize(new
        {
            EmailAddress = GetConfiguration()["Account:DefaultAccount:Email"],
            Password = GetConfiguration()["Account:DefaultAccount:Password"]
        });

    }
}
