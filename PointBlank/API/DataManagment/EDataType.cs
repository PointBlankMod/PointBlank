namespace PointBlank.API.DataManagment
{
    public enum EDataType
    {
        JSON,
        XML,
#if DEBUG
        CONF, // Not done yet
#endif
        UNKNOWN
    }
}
