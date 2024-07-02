using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Todo.Services
{
    public class Gravatar
    {
        public static string GetHash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(emailAddress.Trim().ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToLowerInvariant();
            }
        }

        public async Task<UserWithGravatar> GetGravatarProfile(string emailAddress)
        {
            var user = new UserWithGravatar { Email = emailAddress };
            var hash = GetHash(emailAddress);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gravatar.com/");
                var response = await client.GetAsync($"{hash}.json");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic gravatarData = JsonConvert.DeserializeObject(json);

                    if (gravatarData != null && gravatarData.entry != null)
                    {
                        user.Name = gravatarData.entry[0].displayName;
                        user.AvatarUrl = gravatarData.entry[0].thumbnailUrl;
                    }
                }
                else
                {
                    throw new ApplicationException("The Gravity API is temporarily unavailable");
                }
            }

            return user;
        }
    }
}