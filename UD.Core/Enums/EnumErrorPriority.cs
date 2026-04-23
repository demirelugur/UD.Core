namespace UD.Core.Enums
{
    public enum EnumErrorPriority : byte
    {
        low = 1,
        normal,
        high,
        catastrophicFailure = 255
    }
}