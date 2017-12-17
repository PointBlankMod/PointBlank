using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PointBlank.API.Logging
{
    public static class PointBlankLogging
    {
        static PointBlankLogging()
        {
            if (File.Exists(PointBlankInfo.Previous_Log))
                File.Delete(PointBlankInfo.Previous_Log);
            if (File.Exists(PointBlankInfo.Current_Log))
                File.Move(PointBlankInfo.Current_Log, PointBlankInfo.Previous_Log);
            File.Create(PointBlankInfo.Current_Log);
        }

        #region Private Functions
        private static string GetAsm()
        {
            StackTrace stack = new StackTrace(false);
            string asm = "";

            try
            {
                asm = stack.FrameCount > 0 ? stack.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name : "Not Found";

                if (stack.FrameCount > 1 && (asm == "PointBlank" || asm == "Assembly-CSharp" || asm == "UnityEngine"))
                    asm = stack.GetFrame(2).GetMethod().DeclaringType.Assembly.GetName().Name;
                if (asm == "Assembly-CSharp" || asm == "UnityEngine")
                    asm = "Game";
            }
            catch (Exception ex)
            {
                asm = "Mono";
            }
            return asm;
        }
        #endregion

        #region Public Functions
        public static void Log(object log, bool inConsole = true, ConsoleColor color = ConsoleColor.White, string prefix = "[LOG]")
        {

        }
        #endregion
    }
}
