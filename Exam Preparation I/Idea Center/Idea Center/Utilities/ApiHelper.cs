using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace Idea_Center.Utilities
{
    public class ApiHelper
    {
        protected RestClient client;
        protected const string baseURL = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84";
        protected const string email = "testemail@gtest.bg";
        protected const string password = "123123";
        protected string lastIdeaId = "";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            string jwtToken = GetJwtToken(email, password);

            var options = new RestClientOptions(baseURL)
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };

            client = new RestClient(options);
        }

        protected string GetJwtToken(string email, string password)
        {
            var authenClient = new RestClient(baseURL);
            var request = new RestRequest("/api/User/Authentication");
            request.AddJsonBody(new
            {
                email,
                password
            });
            var response = authenClient.Execute(request, Method.Post);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token = result.GetProperty("accessToken").GetString();
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("Token is null or empty");
                }
                return token;
            }
            else
            {
                throw new InvalidOperationException($"Authentication failed:  {response.StatusCode}");
            }
        }
        protected string GenerateIdeaTitle()
        {
            return $"TestIdea{DateTime.Now.Ticks}";
        }
    }
}
