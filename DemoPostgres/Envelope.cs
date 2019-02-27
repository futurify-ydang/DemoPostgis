using System.Runtime.Serialization;

namespace DemoPostgres
{
    [DataContract]
    public class Envelope : GeometryBase
    {
        [DataMember(EmitDefaultValue = false)] public double xmin { get; set; }

        [DataMember(EmitDefaultValue = false)] public double ymin { get; set; }

        [DataMember(EmitDefaultValue = false)] public double xmax { get; set; }

        [DataMember(EmitDefaultValue = false)] public double ymax { get; set; }

        [DataMember(EmitDefaultValue = false)] public double? zmin { get; set; }

        [DataMember(EmitDefaultValue = false)] public double? zmax { get; set; }

        [DataMember(EmitDefaultValue = false)] public double? mmin { get; set; }

        [DataMember(EmitDefaultValue = false)] public double? mmax { get; set; }
    }
}