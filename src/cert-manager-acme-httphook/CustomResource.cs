using System.Collections.Generic;
using k8s;
using k8s.Models;
using Newtonsoft.Json;

namespace CertManager.Acme.HttpHook
{
    public class CustomResource : KubernetesObject
    {
        [JsonProperty(PropertyName = "metadata")]
        public V1ObjectMeta Metadata { get; set; }
    }

    public abstract class CustomResource<TSpec, TStatus> : CustomResource
    {
        [JsonProperty(PropertyName = "spec")]
        public TSpec Spec { get; set; }

        [JsonProperty(PropertyName = "status")]
        public TStatus Status { get; set; }
    }

    public class CustomResourceList<T> : KubernetesObject
        where T : CustomResource
    {
        public V1ListMeta Metadata { get; set; }
        public List<T> Items { get; set; }
    }
}