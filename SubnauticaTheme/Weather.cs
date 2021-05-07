using RestSharp;
using System.Threading.Tasks;

namespace SubnauticaTheme
{
    public class Weather
    {

        private readonly RestClient client;
        private readonly string station;

        public double temperature;
        public double pressure;
        public double wind;
        public double humidity;

        public Weather(string station)
        {
            client = new RestClient("https://api.weather.gov");
            this.station = station;
        }


        public async Task Update()
        {
            var request = new RestRequest("/stations/" + station + "/observations/latest");
            var response = await client.ExecuteGetAsync<ObservationsDto>(request).ConfigureAwait(false);

            if (response.Data.Properties != null)
            {
                var properties = response.Data.Properties;
                temperature = properties.Temperature.Value ?? 0;
                pressure = (properties.SeaLevelPressure.Value ?? properties.BarometricPressure.Value ?? 0) / 100.0;
                wind = properties.WindSpeed.Value ?? 0;
                humidity = properties.RelativeHumidity.Value ?? 0;
            }
        }

    }
}
