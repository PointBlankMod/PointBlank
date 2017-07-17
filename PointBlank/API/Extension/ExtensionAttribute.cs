using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Extension
{
    /// <summary>
    /// Used for defining extension for the PointBlank project. Must be added to the assembly of any extension that implements a modding API to any unity game
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ExtensionAttribute : Attribute
    {
    }
}
