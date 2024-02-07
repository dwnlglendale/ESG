namespace CarbonFootprint1.Data.EndPoints
{
    public class Constant
    {
        //public static string endPoint = @"http://cbgflowapi.rapidtest.com/CBG/FlowAPI/";

        public static string endPoint = config("endPoint");

        public static string config(string key)
        {
            return GetAppConfig("NEWCONFIG", key);
        }

        public static string GetAppConfig(string configName, string configKey)
        {
            string result = "";
            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
                var config = builder.Build();
                result = config[configName + ":" + configKey];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading from ReadFromAffConfigFile: error is " + ex.InnerException + " " + ex);
            }
            return result;
        }
    }

}
