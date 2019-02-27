using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace DemoPostgres
{
    public abstract class SpatialReference
    {
    }

    [DataContract]
    public class WkidBasedSpatialReference : SpatialReference
    {
        public WkidBasedSpatialReference()
        {
            wkid = Constants.wkid_102100;
            latestWkid = 3857;
        }
        public WkidBasedSpatialReference(int wkid)
        {
            this.wkid = wkid;
            latestWkid = 3857;
        }

        [DataMember(EmitDefaultValue = false)]
        public int wkid
        {
            get; set;
        }
        [JsonIgnore]
        private int? _latestWkid { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? latestWkid
        {
            get
            {
                if (this.wkid == Constants.wkid_4326)
                {
                    return null;
                }
                return this._latestWkid;
            }
            set { this._latestWkid = value; }
        }

        [DataMember(EmitDefaultValue = false)] public int? vcsWkid { get; set; }

        [DataMember(EmitDefaultValue = false)] public int? latestVcsWkid { get; set; }
    }
}