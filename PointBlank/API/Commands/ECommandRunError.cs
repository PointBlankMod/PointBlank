using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Commands
{
    /// <summary>
    /// Error codes for executing a command
    /// </summary>
    public enum ECommandRunError
    {
        NONE,
        COMMAND_NOT_EXIST,
        NO_PERMISSION,
        SERVER_RUNNING,
        SERVER_LOADING,
        NOT_CONSOLE,
        NOT_PLAYER,
        ARGUMENT_COUNT,
        COOLDOWN,
        NO_EXECUTE,
        EXCEPTION
    }
}
