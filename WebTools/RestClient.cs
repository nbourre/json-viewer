using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebTools
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class RestClient
    {
        public string EndPoint { get; set; }
        public httpVerb HttpMethod { get; set; }

        public RestClient()
        {
            EndPoint = string.Empty;
            HttpMethod = httpVerb.GET;
        }

        public async Task<HttpResponseMessage> MakeRequestAsync()
        {
            if (EndPoint == string.Empty)
                throw new ArgumentException("Il n'y a pas de EndPoint!!");

            using (var responseValue = await ApiHelper.ApiClient.GetAsync(EndPoint))
            {
                if (!responseValue.IsSuccessStatusCode)
                {
                    throw new ApplicationException(responseValue.ReasonPhrase);
                }

                return responseValue;
            }
        }

    }
}
