using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace WhistleAPI
{
    /// <summary>
    /// Attempt to implement the "unofficial api" described here: http://jared.wuntu.org/whistle-dog-activity-monitor-undocumented-api/
    /// Also https://github.com/oehokie/node-whistle/blob/master/index.js
    /// </summary>
    public class Client 
    {
        private string token;
        /// <summary>
        /// will default set to first dog in your list or you can set explicitly
        /// </summary>
        private string Id { get; set; }
        /// <summary>
        /// will default set to first dog in your list or you can set explicitly
        /// </summary>
        private string DeviceId { get; set; }

        public Client( string password, string email )
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("User-Agent", "WhistleApp/102 (iPhone; iOS 7.0.4; Scale/2.00)");
            string payload = "{\"password\":\"" + password + "\",\"email\":\"" + email + "\",\"app_id\":\"com.whistle.WhistleApp\"}";
            string response = webClient.UploadString("https://app.whistle.com/api/tokens.json", payload );
            Dictionary<string, string> login = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            this.token = login["token"];

            this.GetDogs();
        }

        /// <summary>
        /// Generic common authenticated request; cast out as appropriate for specific requests
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private T AuthenticatedRequest<T>( string url )
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json");
            webClient.Headers.Add("User-Agent", "WhistleApp/102 (iPhone; iOS 7.0.4; Scale/2.00)");
            webClient.Headers.Add("X-Whistle-AuthToken", this.token);

            string response = webClient.DownloadString(url);
            return JsonConvert.DeserializeObject<T>(response);
        }

        public Dictionary<string, object> GetUsers()
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/users/");
        }

        /// <summary>
        /// Must call this on initialization or later. Get data for each dog listed. Some of that data is further structured, so we return object
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetDogs()
        {
            var response = AuthenticatedRequest<List< Dictionary<string, object>>>("https://app.whistle.com/api/dogs.json"); 
            this.Id = response[0]["id"] as string;
            this.DeviceId = response[0]["device_id"] as string;
            return response;
        }

        /// <summary>
        /// Get status of Whistle Device
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDevice()
        {
            return AuthenticatedRequest<Dictionary<string, string>>("https://app.whistle.com/api/devices/" + this.DeviceId + ".json");
        }

        /// <summary>
        /// Returns minutes active and activity goal
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetDailies(int days = 30)
        {
            return AuthenticatedRequest<List<Dictionary<string, string>>>("https://app.whistle.com/api/dogs/" + this.Id + "/dailies?count=" + days); 
        }

        /// <summary>
        /// More detailed. The dailyId is returned in GetDailies
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetDetailedDaily(string dailyId)
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/dogs/" + this.Id + "/dailies/" + dailyId);
        }

        /// <summary>
        /// DEPRECATED? Gets trend from date to present day
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetTrends(string fromDate = "2015-08-30") //'2014-10-24')
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/dogs/" + this.Id + "/trends?start_date="+ fromDate); 
        }

        /// <summary>
        /// DEPRECATED ; Get metrics for a particular day
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetMetrics( string forDate = "2015-09-01") //'2014-10-24')
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/dogs/" + this.Id + "/metrics?start_date=" + forDate + "&id=" + this.Id   );
        }

        /// <summary>
        /// Timeline id is in event data obtained from GetDetailedDaily
        /// </summary>
        /// <param name="timelineId"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetTimeline(string timelineId)
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/timeline/" +  timelineId);
        }

        /// <summary>
        /// ? Types are note, food, medication
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetHighlights( string type = "note") //'note or food or medication',
        {
            return AuthenticatedRequest<List<Dictionary<string, object>>>("https://app.whistle.com/api/dogs/" + this.Id + "/highlights?type=" + type);
        }


        public List<Dictionary<string, object>> GetUserPresent()
        {
            return AuthenticatedRequest<List<Dictionary<string, object>>>("https://app.whistle.com/api/dogs/" + this.Id + "/stats/users_present");
        }

        public Dictionary<string, object> GetGoals()
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/dogs/" + this.Id + "/stats/goals");
        }

        /// <summary>
        /// Returns average minutes active/rest vs similar dogs active/rest
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetAverages()
        {
            return AuthenticatedRequest<Dictionary<string, object>>("https://app.whistle.com/api/dogs/" + this.Id + "/stats/averages");
        }

        /// <summary>
        /// Returns minutes active and minutes rest
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetDailyTotals(string fromDate = "2015-08-30") //'2014-10-24')
        {
            return AuthenticatedRequest<List<Dictionary<string, string>>>("https://app.whistle.com/api/dogs/" + this.Id + "/stats/daily_totals/?start_time=" + fromDate);
        }
    }

}
