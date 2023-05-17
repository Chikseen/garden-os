
using System.Net;

namespace MainService.Network
{
    public class Network
    {
        public String _localeIPv4 = "";
        public String _publicIPv4 = "";

        public Network()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            // Get the IP
            string myIP = Dns.GetHostEntry(hostName).AddressList.Where(e => e.ToString().Contains("192.")).ToList()[0].ToString();

            foreach (var item in Dns.GetHostEntry(hostName).AddressList)
            {
                Console.WriteLine(item);
            }

            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            Console.WriteLine(address);


            string strHostName = System.Net.Dns.GetHostName();
            string strIp = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            Console.WriteLine(strHostName);
            Console.WriteLine(strIp);
        }
    }
}