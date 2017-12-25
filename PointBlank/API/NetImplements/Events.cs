using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Logging;

namespace PointBlank.API.NetImplements
{
    /// <summary>
    /// The .NET extension for events
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Easily runs any event from anywhere in the assembly. It also checks for any issues before running said event
        /// </summary>
        /// <param name="del">The event to run</param>
        /// <param name="paramaters">The parameters of the event</param>
        public static void RunEvent(this Delegate del, params object[] paramaters)
        {
            if (del == null)
                return;
            Delegate[] list = del.GetInvocationList();
            if (list.Length < 1)
                return;

            foreach (Delegate d in list)
            {
                try
                {
                    d.DynamicInvoke(paramaters);
                }
                catch (Exception ex)
                {
                    PointBlankLogging.LogError("Error invoking delegate in event: " + d.Method.Name, ex, false, false);
                }
            }
        }
    }
}
