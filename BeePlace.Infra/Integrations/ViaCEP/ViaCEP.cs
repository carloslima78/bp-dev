using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace BeePlace.Infra.Integrations.ViaCEP
{
    public class ViaCEP
    {
        public static ViaCEPRoot Validate(string zip)
        {
            var viaCepRoot = new ViaCEPRoot();

            try
            {
                HttpClient httpClient = new HttpClient();
                var uri = string.Format("https://viacep.com.br/ws/{0}/json", zip.Replace("-", "").Replace(".", ""));
                HttpResponseMessage result = httpClient.GetAsync(uri).Result;
                string json = result.Content.ReadAsStringAsync().Result;
                viaCepRoot = JsonConvert.DeserializeObject<ViaCEPRoot>(json);
                return viaCepRoot;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    public class ViaCEPRoot
    {
        public string cep { get; set; }

        public string logradouro { get; set; }

        public string complemento { get; set; }

        public string bairro { get; set; }

        public string localidade { get; set; }

        public string uf { get; set; }

        public string unidade { get; set; }

        public string ibge { get; set; }

        public string gia { get; set; }
    }
}
