using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Detour;

namespace PointBlank.Services.DetourManager
{
    internal class DetourWrapper
    {
        #region Properties
        public MethodInfo Original { get; private set; }
        public MethodInfo Modified { get; private set; }

        public IntPtr ptrOriginal { get; private set; }
        public IntPtr ptrModified { get; private set; }

        public RedirectionHelper.OffsetBackup OffsetBackup { get; private set; }
        public DetourAttribute Attribute { get; private set; }

        public bool Detoured { get; private set; }
        public object Instance { get; private set; }
        public bool Local { get; private set; }
        #endregion

        public DetourWrapper(MethodInfo original, MethodInfo modified, DetourAttribute attribute, object instance = null)
        {
            // Set the variables
            Original = original;
            Modified = modified;
            Instance = instance;
            Attribute = attribute;
            Local = (Modified.DeclaringType.Assembly == Assembly.GetExecutingAssembly());

            RuntimeHelpers.PrepareMethod(original.MethodHandle);
            RuntimeHelpers.PrepareMethod(modified.MethodHandle);
            ptrOriginal = Original.MethodHandle.GetFunctionPointer();
            ptrModified = Modified.MethodHandle.GetFunctionPointer();

            OffsetBackup = new RedirectionHelper.OffsetBackup(ptrOriginal);
            Detoured = false;
        }

        #region Public Functions
        public bool Detour()
        {
            if (Detoured)
                return true;
            bool result = RedirectionHelper.DetourFunction(ptrOriginal, ptrModified);

            if(result)
                Detoured = true;

            return result;
        }

        public bool Revert()
        {
            if (!Detoured)
                return false;
            bool result = RedirectionHelper.RevertDetour(OffsetBackup);

            if(result)
                Detoured = false;

            return result;
        }

        public object CallOriginal(object[] args, object instance = null)
        {
            Revert();
            object result = null;
            try
            {
                result = Original.Invoke(instance ?? Instance, args);
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error when attempting to run original method!", ex);
            }

            Detour();
            return result;
        }
        #endregion
    }
}
