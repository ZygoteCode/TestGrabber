using System.Text;
using Newtonsoft.Json.Linq;
using LegitHttpClient;
using System;

public class IPGrabber
{
    public static string GetIPAddress()
    {
        try
        {
            HttpClient client = new HttpClient();
            client.ConnectTo("ip4.seeip.org", true, 443);

            HttpRequest request = new HttpRequest();
            request.URI = "/";
            request.Method = HttpMethod.GET;
            request.Version = HttpVersion.HTTP_11;

            request.Headers.Add(new HttpHeader() { Name = "Host", Value = "ip4.seeip.org" });

            return BodyUtils.GetCleanIP(Encoding.UTF8.GetString(client.Send(request).Body));
        }
        catch
        {
            return "";
        }
    }

    public static string GetGeoIP1(string ip)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.ConnectTo("ip-api.com");

            HttpRequest request = new HttpRequest();
            request.URI = "/json/" + ip;
            request.Method = HttpMethod.GET;
            request.Version = HttpVersion.HTTP_10;

            request.Headers.Add(new HttpHeader() { Name = "Host", Value = "ip-api.com" });

            string body = BodyUtils.GetCleanJSON(Encoding.UTF8.GetString(client.Send(request).Body));
            dynamic jss = JObject.Parse(body);

            return "IP Address: " + ip + "\r\n" + 
                "Country: " + jss.country + "\r\n" +
                "Country Code: " + jss.countryCode + "\r\n" +
                "Region: " + jss.regionName + "\r\n" +
                "City: " + jss.city + "\r\n" +
                "ZIP (Postal/Post Code): " + jss.zip + "\r\n" +
                "Latitude: " + jss.lat + "\r\n" +
                "Longitude: " + jss.lon + "\r\n" +
                "Timezone: " + jss.timezone + "\r\n" +
                "ISP (Internet Service Provider): " + jss.isp;
        }
        catch
        {
            return "";
        }
    }

    public static string GetGeoIP2(string ip)
    {
        try
        {
            HttpClient client = new HttpClient();
            client.ConnectTo("proxycheck.io");

            HttpRequest request = new HttpRequest();
            request.URI = "/v2/" + ip + "?vpn=1&asn=1";
            request.Method = HttpMethod.GET;
            request.Version = HttpVersion.HTTP_10;

            request.Headers.Add(new HttpHeader() { Name = "Host", Value = "proxycheck.io" });

            string body = BodyUtils.GetCleanJSON(Encoding.UTF8.GetString(client.Send(request).Body));
            dynamic jss = JObject.Parse(body)[ip];

            return "IP Address: " + ip + "\r\n" +
                "ASN (Autonomous System Number): " + jss.asn + "\r\n" +
                "Provider: " + jss.provider + "\r\n" +
                "Organisation: " + jss.organisation + "\r\n" +
                "Continent: " + jss.continent + "\r\n" +
                "Country: " + jss.country + "\r\n" +
                "ISO Code: " + jss.isocode + "\r\n" +
                "Region: " + jss.region + "\r\n" +
                "Region Code: " + jss.regioncode + "\r\n" +
                "Timezone: " + jss.timezone + "\r\n" +
                "City: " + jss.city + "\r\n" +
                "ZIP (Postal/Post Code): " + jss.postcode + "\r\n" +
                "Latitude: " + jss.latitude + "\r\n" +
                "Longitude: " + jss.longitude + "\r\n" +
                "Using Proxy/VPN: " + jss.proxy + "\r\n" +
                "Type: " + jss.type;
        }
        catch
        {
            return "";
        }
    }
}