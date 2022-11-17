

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public enum SourceDisplayType
{
    [Description("Not Display")]
    NotDisplay = 1,
    [Description("Display")]
    Display = 2,
    [Description("Display Set Switch")]
    DisplaySetSwitch = 3,
    [Description("Display Set Switch Parameters")]
    DisplayAndSetSwitchParameters = 4,
    [Description("Display Set Switch Parameters Channels Info")]
    DisplayAndSetSwitchParametersChannelsInfo = 5
}
