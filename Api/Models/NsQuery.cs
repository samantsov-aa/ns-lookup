using DnsClient;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Api.Models
{
    public class NsQuery
    {
        public static NsResponse Query(string host, List<string> queries)
        {
            var queriesIncase = queries.Select(t => t.ToLower()).ToList();
            var lookup = new LookupClient(new IPAddress(0x08080808));
            lookup.Retries = 3;
            lookup.Timeout = TimeSpan.FromSeconds(3);
            string a = null;
            string aaaa = null;
            string ns = null;
            string mx = null;
            string ptr = null;
            string soa = null;
            string srv = null;
            string txt = null;
            string title = null;
            string desc = null;
            string keywords = null;
            string content = null;
            if (queriesIncase.Any(t => t == "title" || t == "keywords" || t == "description"))
            {
                content = getContent(host);
            }
            if (queriesIncase.Contains("a")) a = getARecords(Query(lookup, host, QueryType.A));
            if (queriesIncase.Contains("aaaa")) aaaa = getAAAARecords(Query(lookup, host, QueryType.AAAA));
            if (queriesIncase.Contains("ns")) ns = getNSRecords(Query(lookup, host, QueryType.NS));
            if (queriesIncase.Contains("mx")) mx = getMXRecords(Query(lookup, host, QueryType.MX));
            if (queriesIncase.Contains("ptr")) ptr = getPtrRecords(Query(lookup, host, QueryType.PTR));
            if (queriesIncase.Contains("soa")) soa = getSoaRecords(Query(lookup, host, QueryType.SOA));
            if (queriesIncase.Contains("srv")) srv = getSrvRecords(Query(lookup, host, QueryType.SRV));
            if (queriesIncase.Contains("txt")) txt = getTxtRecords(Query(lookup, host, QueryType.TXT));
            if (queriesIncase.Contains("title")) title = getTitle(content);
            if (queriesIncase.Contains("keywords")) keywords = getKeywords(content);
            if (queriesIncase.Contains("description")) desc = getDescription(content);
            return new NsResponse(host, a, aaaa, ns, mx, ptr, soa, srv, txt, title, desc, keywords);
        }

        private static IDnsQueryResponse Query(ILookupClient lookup, string host, QueryType t)
        {
            return lookup.Query(host, t);
        }

        private static string getContent(string host)
        {
            var content = "";
            try
            {
                content = readHtml("https://" + host);
            }
            catch
            {
                try
                {
                    content = readHtml("http://" + host);
                }
                catch
                {
                }
            }
            return content;
        }

        private static string readHtml(string host)
        {
            char[] buf = new char[5000];
            WebRequest request = WebRequest.Create(host);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                sr.ReadBlock(buf, 0, 5000);
            }
            return new string(buf);
        }

        private static string getARecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.ARecords().Select(a => a.Address.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }

        }

        private static string getMXRecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.MxRecords().Select(a => a.Exchange.Original.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getAAAARecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.AaaaRecords().Select(a => a.Address.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getNSRecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.NsRecords().Select(a => a.NSDName.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getPtrRecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.PtrRecords().Select(a => a.PtrDomainName.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getSoaRecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.SoaRecords().Select(a => a.MName.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getSrvRecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.SrvRecords().Select(a => a.Target.ToString()));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getTxtRecords(IDnsQueryResponse response)
        {
            try
            {
                return string.Join("\n", response.Answers.TxtRecords().Select(a => String.Join("\n", String.Join("\n", a.EscapedText))));
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        private static string getTitle(string content)
        {
            HtmlDocument htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(content);
            return htmlSnippet?.DocumentNode?.SelectSingleNode("//title")?.InnerText?.Trim() ?? String.Empty;
        }

        private static string getDescription(string content)
        {
            var res = String.Empty;
            try
            {
                HtmlDocument htmlSnippet = new HtmlDocument();
                htmlSnippet.LoadHtml(content);
                var meta =
                    htmlSnippet?.DocumentNode?.SelectNodes("//meta")?
                        .Where(m => m.Attributes?["name"]?.Value.ToLowerInvariant() == "description");

                if (meta.Any())
                {
                    res = meta.First().Attributes["content"]?.Value ?? String.Empty;
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }

        private static string getKeywords(string content)
        {
            var res = String.Empty;
            try
            {
                HtmlDocument htmlSnippet = new HtmlDocument();
                htmlSnippet.LoadHtml(content);
                var meta =
                    htmlSnippet?.DocumentNode?.SelectNodes("//meta")?
                        .Where(m => m.Attributes?["name"]?.Value.ToLowerInvariant() == "keywords");

                if (meta.Any())
                {
                    res = meta.First().Attributes["content"]?.Value ?? String.Empty;
                }
            }
            catch (Exception ex)
            {
            }
            return res;
        }
    }
}
