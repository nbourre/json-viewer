using json_viewer.Commands;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security;
using System.Text.Json;
using WebTools;

namespace json_viewer
{
    public class MainViewModel : BaseViewModel
    {
        // Exercice avec jeton : https://the-one-api.dev/documentation#1
        
        private string url;
        private string jsonContent;
        private string endPoint;
        private string token;

        public string URL
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged();
                GetJsonCommand.RaiseCanExecuteChanged();
            }
        }

        public string EndPoint
        {
            get => endPoint;
            set
            {
                endPoint = value;
                OnPropertyChanged();
            }
        }

        public string Token { 
            get => token;
            set {
                token = value;
                OnPropertyChanged();
            }
        }

        public string JsonContent
        {
            get => jsonContent;
            set
            {
                jsonContent = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand<string> GetJsonCommand { get; set; }

        public MainViewModel()
        {
            GetJsonCommand = new DelegateCommand<string>(GetJson, CanGetJson);
            URL = "https://cat-fact.herokuapp.com";
            EndPoint = "/facts/random?animal_type=cat&amount=2";

        }

        private bool CanGetJson(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        private async void GetJson(string url)
        {
            if (EndPoint != string.Empty)
            {
                url += EndPoint;
            }

            if (Token != string.Empty && Token != null)
            {
                ApiHelper.SetAuthenticationBearer(Token.ToString());
            }

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var temp = await response.Content.ReadAsStringAsync();
                    JsonContent = PrettyJson(temp);
                }
                else
                {
                    JsonContent = response.ReasonPhrase;
                }
            }
        }

        public string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return System.Text.Json.JsonSerializer.Serialize(jsonElement, options);
        }

    }
}
