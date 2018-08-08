using System.Runtime.Serialization;

namespace Api.Models
{
    [DataContract]
    public class NsResponse
    {
        [DataMember]
        public string Host { get; private set; }
        [DataMember]
        public string A { get; private set; }
        [DataMember]
        public string AAAA { get; private set; }
        [DataMember]
        public string NS { get; private set; }
        [DataMember]
        public string MX { get; private set; }
        [DataMember]
        public string PTR { get; private set; }
        [DataMember]
        public string SOA { get; private set; }
        [DataMember]
        public string SRV { get; private set; }
        [DataMember]
        public string TXT { get; private set; }
        [DataMember]
        public string Title { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public string Keywords { get; private set; }

        public NsResponse(string host, string a, string aaaa, string ns, string mx, string ptr, string soa, string srv, string txt, string title, string description, string keywords)
        {
            this.Host = host;
            this.A = a;
            this.AAAA = aaaa;
            this.NS = ns;
            this.MX = mx;
            this.PTR = ptr;
            this.SOA = soa;
            this.SRV = srv;
            this.TXT = txt;
            this.Title = title;
            this.Description = description;
            this.Keywords = keywords;
        }
    }
}
