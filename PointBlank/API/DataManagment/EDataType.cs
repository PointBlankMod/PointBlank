namespace PointBlank.API.DataManagment
{
    public enum EDataType
    {
        Json,
        Xml,
#if DEBUG
        Conf, // Not done yet
#endif
        Unknown
    }
}
