using System.Runtime.Serialization;

namespace BP_OnlineDOD.Client.Helpers
{
    [DataContract(Name = "PatchObject")]
    public class PatchObject
    {
        [DataMember(Name = "op")]
        public string Op { get; set; }
        [DataMember(Name = "path")]
        public string Path { get; set; }
        [DataMember(Name = "value")]
        public object Value { get; set; }
    }
}
