using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.NetworkInformation;
using System.Text;

namespace MUtility
{
    public class Network
    {
        public static bool Ping()
        {
            return Ping("www.google.com");
        }

        private static bool Ping(string url)
        {
            Contract.Requires(url == null, "Err: Wrong URL.");

            Ping ping = new Ping();

            PingReply reply = ping.Send(url, 3000);

            if (reply.Status == IPStatus.Success) { return true; }
            else { return false; }
        }
    }
}
