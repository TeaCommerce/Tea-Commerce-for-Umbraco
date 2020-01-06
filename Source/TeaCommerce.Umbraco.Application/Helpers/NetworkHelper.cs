using System;

namespace TeaCommerce.Umbraco.Application.Helpers
{
    internal class NetworkHelper
    {
        public static string MachineName
        {
            get
            {
                try
                {
                    return Environment.MachineName;
                }
                catch
                {
                    try
                    {
                        return System.Net.Dns.GetHostName();
                    }
                    catch
                    {
                        //if we get here it means we cannot access the machine name
                        throw new ApplicationException("Cannot resolve the current machine name either by Environment.MachineName or by Dns.GetHostname()");
                    }
                }
            }
        }
    }
}