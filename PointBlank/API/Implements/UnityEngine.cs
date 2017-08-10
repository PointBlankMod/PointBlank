using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlank.API.Implements
{
    public static class UnityEngine
    {
        /// <summary>
        /// Duplicates a Vector3 instance
        /// </summary>
        /// <param name="vector3">The Vector3 instance to duplicate</param>
        /// <returns>The duplicated instance of the Vector3 instance specified</returns>
        public static Vector3 Duplicate(this Vector3 vector3) => new Vector3(vector3.x, vector3.y, vector3.z);
    }
}
