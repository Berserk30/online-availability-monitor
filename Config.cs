namespace Manusys
{
    using System;
    using Newtonsoft.Json;

/*
Defines app configuration. Service at its startup needs the region where it runs and the db conn string.
 */
    public class Config
    {
        [JsonRequired]
        public String region {get; set;}

        [JsonRequired]
        public String dbConnectionString {get; set;}
    }
}
