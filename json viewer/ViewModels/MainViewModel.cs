using json_viewer.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using WebTools;

namespace json_viewer
{
    public class MainViewModel : BaseViewModel
    {
        private string url;
        private string jsonContent;

        public string URL { 
            get => url;
            set { 
                url = value;
                OnPropertyChanged();
                GetJsonCommand.RaiseCanExecuteChanged();
            }
        }

        public string JsonContent
        {
            get => jsonContent;
            set { 
                jsonContent = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand<string> GetJsonCommand { get; set; }

        public MainViewModel()
        {
            GetJsonCommand = new DelegateCommand<string>(GetJson, CanGetJson);
        }

        private bool CanGetJson(string url)
        {  
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        private async void GetJson(string url)
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var temp = await response.Content.ReadAsStringAsync();
                    JsonContent = PrettyJson(temp);
                } else
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

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }

    }
}
