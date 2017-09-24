namespace PointBlank.API.Commands
{
    /// <summary>
    /// Error codes for executing a command
    /// </summary>
    public enum ECommandRunError
    {
        None,
        CommandNotExist,
        NoPermission,
        ServerRunning,
        ServerLoading,
        NotConsole,
        NotPlayer,
        ArgumentCount,
        Cooldown,
        NoExecute,
        Exception
    }
}
