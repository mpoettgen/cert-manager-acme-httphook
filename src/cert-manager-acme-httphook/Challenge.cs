using k8s;
using k8s.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CertManager.Acme.HttpHook
{
    public class Challenge : CustomResource<ChallengeSpec, ChallengeStatus>
    {
        public override string ToString()
        {
            string display = "Challenge ";
            if (Metadata != null)
                display += $"Metadata: ({Metadata?.Name}, ResourceVersion: {Metadata?.ResourceVersion}) ";
            if (Spec != null)
                display += $"Spec: (DnsName: {Spec?.DnsName}, Type: {Spec?.Type}, Key: {Spec?.Key}, Token: {Spec?.Token}) ";
            if (Status != null)
                display += $"Status: (State: {Status.State}, Reason: {Status.Reason})";
            return display;
        }
    }

    public class ChallengeSpec
    {

        [JsonProperty(PropertyName = "authzURL")]
        public string AuthzURL { get; set; }

        [JsonProperty(PropertyName = "dnsName")]
        public string DnsName { get; set; }

        [JsonProperty(PropertyName = "issuerRef")]
        public IssuerReference IssuerRef { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "solver")]
        public Solver Solver { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "wildcard")]
        public bool? Wildcard { get; set; }
    }

    public class IssuerReference
    {
        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class Solver
    {
        [JsonProperty(PropertyName = "dns01")]
        public object DnsSolver { get; set; }

        [JsonProperty(PropertyName = "http01")]
        public object HttpSolver { get; set; }

        [JsonProperty(PropertyName = "selector")]
        public object Selector { get; set; }
    }

    public class ChallengeStatus
    {
        [JsonProperty(PropertyName = "presented")]
        public bool? Presented { get; set; }

        [JsonProperty(PropertyName = "processing")]
        public bool? Processing { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public ChallengeState State { get; set; }
    }

    public enum ChallengeState
    {
        Valid,
        Ready,
        Pending,
        Processing,
        Invalid,
        Expired,
        Errored
    }
}