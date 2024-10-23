using Newtonsoft.Json;
using PicMe.App.Core.Entities;
using PicMe.App.Core.Interfaces.Repositories;
using PicMe.App.Core.Interfaces.Services;
using System.Net.Http.Headers;

namespace PicMe.App.Infra.Repositories
{
    public class OneRosterRepository : IOneRosterRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ISecureStorageService _secureStorageService;
        private readonly IJsonService _jsonService;
        private readonly IStorageService _storageService;


        public OneRosterRepository(HttpClient httpClient, ISecureStorageService secureStorageService, IJsonService jsonService, IStorageService storageService)
        {
            _httpClient = httpClient;
            _secureStorageService = secureStorageService;
            _jsonService = jsonService;
            _storageService = storageService;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = await _secureStorageService.GetAsync("ClientId");
            var clientSecret = await _secureStorageService.GetAsync("ClientSecret");
            var school = (await _secureStorageService.GetAsync("SchoolName"))?.Trim();

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret) || string.IsNullOrWhiteSpace(school))
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"1 van de credentials is leeg, vraag uw IT dienst de gegevens bij te werken", "OK");
                return null;
            }

            try
            {
                var requestBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                });

                var response = await _httpClient.PostAsync($"https://{school}.smartschool.be/ims/oneroster/token", requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Verkeerde credentials, (FOUT:{response.StatusCode}) Contacteer uw IT dienst om deze bij te werken", "OK");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);

                return tokenResponse.AccessToken;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Een onverwachte fout is opgetreden, foutcode:{ex}, Contacteer uw IT dienst om dit op te lossen", "OK");
                return null;
            }
        }

        public async Task<List<StudentInfo>> GetAllEnrollmentsAsync()
        {
            var school = await _secureStorageService.GetAsync("SchoolName");
            var token = await GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(school))
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Token is leeg, Contacteer uw IT dienst om deze bij te werken", "OK");
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"https://{school}.smartschool.be/ims/oneroster/v1p1/enrollments");

            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Verkeerde credentials, Contacteer uw IT dienst om deze bij te werken", "OK");
                return null;
            }

            var studentsJson = await response.Content.ReadAsStringAsync();
            var studentsInfo = await _jsonService.ExtractStudentInfoAsync(studentsJson);


            return studentsInfo;
        }
    }
}
