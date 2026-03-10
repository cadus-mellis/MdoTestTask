using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SqlVersionService.Contracts.Requests;
using SqlVersionService.Contracts.Responses;

namespace SqlVersionService.Desktop.Services
{
    public sealed class SqlVersionServiceClient
    {
        private readonly HttpClient _httpClient;

        public SqlVersionServiceClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL is required.", nameof(baseUrl));

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(new Uri(baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/"), "api/sql/")
            };
        }

        public async Task<OpenConnectionResponse> OpenConnectionAsync(string connectionString)
        {
            var request = new OpenConnectionRequest
            {
                ConnectionString = connectionString
            };

            return await PostAsync<OpenConnectionRequest, OpenConnectionResponse>(
                "connection/open",
                request);
        }

        public async Task<GetVersionResponse> GetVersionAsync(Guid connectionId)
        {
            var request = new GetVersionRequest
            {
                ConnectionId = connectionId
            };

            return await PostAsync<GetVersionRequest, GetVersionResponse>(
                "version",
                request);
        }

        public async Task<CloseConnectionResponse> CloseConnectionAsync(Guid connectionId)
        {
            var request = new CloseConnectionRequest
            {
                ConnectionId = connectionId
            };

            return await PostAsync<CloseConnectionRequest, CloseConnectionResponse>(
                "connection/close",
                request);
        }

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ProblemDetailsResponse problem = null;

                    try
                    {
                        problem = JsonConvert.DeserializeObject<ProblemDetailsResponse>(responseContent);
                    }
                    catch
                    {
                    }

                    var message = problem?.Detail ?? responseContent ?? "Request failed.";
                    throw new InvalidOperationException(message);
                }

                var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                return result;
            }
        }
    }
}