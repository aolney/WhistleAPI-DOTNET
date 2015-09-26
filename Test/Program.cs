using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WhistleAPI.Client("yourpassword", "yourusername");

            var averages = client.GetAverages();
            var dailies = client.GetDailies();
            var totals = client.GetDailyTotals();
            var dog = client.GetDogs();
            var status = client.GetDevice();
            var goals = client.GetGoals();
            var highlights = client.GetHighlights();
            var timeline = client.GetTimeline(""); //needs a real id
            var present = client.GetUserPresent();
            var users = client.GetUsers();

            //example usage for data analysis
            foreach( var daily in dailies )
            {
                string dayId = daily["day_number"];
                var detailedDaily = client.GetDetailedDaily(dayId);
            }
        }
    }
}
