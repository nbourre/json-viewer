using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
