using System;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Data;
using System.Timers;
using System.Collections.Generic;
using System.Threading;

using Timer = System.Timers.Timer;

namespace Manusys
{
    /*
    Class is responsible for regularly pinging configured web sites, recording HTTP ret code
    and measuring latency.
     */
    public class Program
    {
        private static Config config;

        public static void Main(string[] args)
        {
            String json;
            using (StreamReader r = new StreamReader("config.json"))
            {
                json = r.ReadToEnd();
            }

            config = JsonConvert.DeserializeObject<Config>(json);

            Console.WriteLine(config.dbConnectionString);

            var list = new List<String>();
            using(var conn = new MySqlConnection())
            {
                conn.ConnectionString = config.dbConnectionString;
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM config", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string)reader[1]);
                    Console.WriteLine("{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);
                }
            }
            
            var aTimer = new Timer(60000);
            aTimer.Elapsed += (source, e) => OnTimedEvent(list);

            // Hook up the Elapsed event for the timer. 
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            var evt = new ManualResetEvent(false);

            Console.CancelKeyPress += (source, e) => { Console.WriteLine("Ctrl-C"); evt.Set(); };

            WaitHandle.WaitAll(new [] {evt});
        }

        /*
        Callback for the timer tick.
        */
        private static async void OnTimedEvent(List<string> urls)
        {
            var now = DateTime.UtcNow;
            var sw = new Stopwatch();
            using(var client = new HttpClient())
            {
                for(int i = 0; i < urls.Count; ++ i)
                {
                    sw.Start();
                    var response = await client.GetAsync(urls[i]);
                    sw.Stop();
                
                    using(var conn = new MySqlConnection())
                    {
                        conn.ConnectionString = config.dbConnectionString;
                        conn.Open();

                        var SQLstr = String.Format("INSERT INTO data (Region, Url, TimestampUtc, LatencyInMsec, HttpStatusCode)" +
                        " VALUES (\"{0}\", \"{1}\", \"{2:yyyy-MM-dd HH:mm:ss}\", \"{3}\", \"{4}\")",
                        config.region, urls[i], now, sw.ElapsedMilliseconds, (int)response.StatusCode);

                        MySqlCommand cmd = new MySqlCommand(SQLstr, conn);
                        cmd.ExecuteNonQuery();
                    
                    }
                }
            }
        }
    }
}
