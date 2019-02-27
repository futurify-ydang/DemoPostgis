using System.ComponentModel;

namespace DemoPostgres
{
    public enum PopupType
    {
        [Description("None")] esriServerHTMLPopupTypeNone,
        [Description("URL")] esriServerHTMLPopupTypeAsURL,
        [Description("HTML Text")] esriServerHTMLPopupTypeAsHTMLText
    }
}