using System.Runtime.Serialization;

namespace DemoPostgres
{
    public abstract class GeometryBase
    {
        [DataMember(EmitDefaultValue = false)]
        public WkidBasedSpatialReference spatialReference { get; set; } =
            new WkidBasedSpatialReference { wkid = Constants.wkid_102100, latestWkid = 3857 };
    }
}