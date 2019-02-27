using System.ComponentModel.DataAnnotations;

namespace DemoPostgres
{
    public class Layer : MetaDataBase
    {
        public Layer()
        {
            Extent = new Envelope();
        }

        public bool IsODataEnabled { get; set; }

        public bool IsOdkEnabled { get; set; }

        public string FilterExpression { get; set; }
        public string TableName { get; set; }

        [Required(ErrorMessage = "Service is required")]
        public int ServiceId { get; set; }

        public string Drawing { get; set; }
        public int VisibleRange { get; set; }

        public Envelope Extent { get; set; }

        public bool IsSupportTime { get; set; }
        public int TimeFieldId { get; set; }
        public double MinDate { get; set; }
        public double MaxDate { get; set; }

        // for feature service
        public bool IsEditable { get; set; }

        public bool HasAttachments { get; set; }
        public GeometryType? Type { get; set; }

        public int MinScale { get; set; }
        public int MaxScale { get; set; }
        public bool SignalRIsAddEventEnabled { get; set; }
        public bool SignalRIsUpdateEventEnabled { get; set; }
        public bool SignalRIsDeleteEventEnabled { get; set; }

        public bool PubNubIsAddEventEnabled { get; set; }
        public bool PubNubIsUpdateEventEnabled { get; set; }
        public bool PubNubIsDeleteEventEnabled { get; set; }

        public bool AzureIsAddEventEnabled { get; set; }
        public bool AzureIsUpdateEventEnabled { get; set; }
        public bool AzureIsDeleteEventEnabled { get; set; }
        public PopupType? PopupType { get; set; }

        /// <summary>
        ///     This value will not get by default, we only get it when need
        /// </summary>
        public string PopupContent { get; set; }

        public string RelationshipsJson { get; set; }
        public string XmlMapping { get; set; }
        public string OdkForm { get; set; }
        public bool IsVersioned { get; set; }
    }
}