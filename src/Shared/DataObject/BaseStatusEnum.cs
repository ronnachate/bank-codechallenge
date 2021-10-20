using System.ComponentModel;

namespace CodeChallenge.DataObjects
{
    public enum BaseStatusEnum
    {
        [Description("SUCCESS")]
        Success = 0,
        [Description("FAILED")]
        Failed = 10
    }
}
