using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Implements;

namespace PointBlank.API
{
    /// <summary>
    /// Used for easier reflection management
    /// </summary>
    public static class PointBlankReflect
    {
        #region Flags
        /// <summary>
        /// The flags for the instance field/method
        /// </summary>
        public static readonly BindingFlags INSTANCE_FLAG = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        /// <summary>
        /// The flags for the static field/method
        /// </summary>
        public static readonly BindingFlags STATIC_FLAG = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        /// <summary>
        /// The flags for the static and instance of a field/method
        /// </summary>
        public static readonly BindingFlags STATIC_INSTANCE_FLAG = INSTANCE_FLAG | STATIC_FLAG;
        #endregion

        #region Variables
        private static List<FieldInfo> Fields = new List<FieldInfo>();
        private static List<MethodInfo> Methods = new List<MethodInfo>();
        #endregion

        #region Private Functions
        private static MethodInfo FindMethod(Type type, string name, BindingFlags flags)
        {
            MethodInfo[] methods = Methods.Where(a => a.DeclaringType == type && a.Name == name).ToArray();

            foreach(MethodInfo method in methods)
            {
                if (flags.HasFlag(BindingFlags.NonPublic) && !method.IsPrivate)
                    continue;
                if (flags.HasFlag(BindingFlags.Public) && !method.IsPublic)
                    continue;
                if (flags.HasFlag(BindingFlags.Static) && !method.IsStatic)
                    continue;
                if (flags.HasFlag(BindingFlags.Instance) && method.IsStatic)
                    continue;

                return method;
            }
            return null;
        }

        private static FieldInfo FindField(Type type, string name, BindingFlags flags)
        {
            FieldInfo[] fields = Fields.Where(a => a.DeclaringType == type && a.Name == name).ToArray();

            foreach(FieldInfo field in fields)
            {
                if (flags.HasFlag(BindingFlags.NonPublic) && !field.IsPrivate)
                    continue;
                if (flags.HasFlag(BindingFlags.Public) && !field.IsPublic)
                    continue;
                if (flags.HasFlag(BindingFlags.Static) && !field.IsStatic)
                    continue;
                if (flags.HasFlag(BindingFlags.Instance) && field.IsStatic)
                    continue;

                return field;
            }
            return null;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gets a method by using reflection
        /// </summary>
        /// <param name="type">The class type where the method is located</param>
        /// <param name="name">The name of the method</param>
        /// <param name="flags">The method flags</param>
        /// <param name="save">Should the method be saved into a buffer for later use</param>
        /// <returns>The MethodInfo of the method</returns>
        public static MethodInfo GetMethod(Type type, string name, BindingFlags flags, bool save = true)
        {
            MethodInfo method = FindMethod(type, name, flags);

            if (method == null)
            {
                method = type.GetMethod(name, flags);
                if (save)
                    Methods.Add(method);
            }

            return method;
        }
        /// <summary>
        /// Gets a method by using reflection with all flags
        /// </summary>
        /// <param name="type">The class type where the method is located</param>
        /// <param name="name">The name of the method</param>
        /// <param name="save">Should the method be saved into a buffer for later use</param>
        /// <returns>The MethodInfo of the method</returns>
        public static MethodInfo GetMethod(Type type, string name, bool save = true) => GetMethod(type, name, STATIC_INSTANCE_FLAG, save);
        /// <summary>
        /// Gets a method by using reflection
        /// </summary>
        /// <typeparam name="T">The class where the method is located</typeparam>
        /// <param name="name">The name of the method</param>
        /// <param name="flags">The flags of the method</param>
        /// <param name="save">Should the method be saved into a buffer for later use</param>
        /// <returns>The MethodInfo of the method</returns>
        public static MethodInfo GetMethod<T>(string name, BindingFlags flags, bool save = true) => GetMethod(typeof(T), name, flags, save);
        /// <summary>
        /// Gets a method by using reflection with all flags
        /// </summary>
        /// <typeparam name="T">The class where the method is located</typeparam>
        /// <param name="name">The name of the method</param>
        /// <param name="flags">The flags of the method</param>
        /// <param name="save">Should the method be saved into a buffer for later use</param>
        /// <returns>The MethodInfo of the method</returns>
        public static MethodInfo GetMethod<T>(string name, bool save = true) => GetMethod<T>(name, STATIC_INSTANCE_FLAG, save);

        /// <summary>
        /// Gets a field by using reflection
        /// </summary>
        /// <param name="type">The class type where the field is located</param>
        /// <param name="name">The name of the field</param>
        /// <param name="flags">The field flags</param>
        /// <param name="save">Should the field be saved into a buffer for later use</param>
        /// <returns>The FieldInfo of the field</returns>
        public static FieldInfo GetField(Type type, string name, BindingFlags flags, bool save = true)
        {
            FieldInfo field = FindField(type, name, flags);

            if(field == null)
            {
                field = type.GetField(name, flags);
                if (save)
                    Fields.Add(field);
            }

            return field;
        }
        /// <summary>
        /// Gets a field by using reflection with all flags
        /// </summary>
        /// <param name="type">The class type where the field is located</param>
        /// <param name="name">The name of the field</param>
        /// <param name="flags">The field flags</param>
        /// <param name="save">Should the field be saved into a buffer for later use</param>
        /// <returns>The FieldInfo of the field</returns>
        public static FieldInfo GetField(Type type, string name, bool save = true) => GetField(type, name, STATIC_INSTANCE_FLAG, save);
        /// <summary>
        /// Gets a field by using reflection
        /// </summary>
        /// <typeparam name="T">The class where the field is located</typeparam>
        /// <param name="name">The name of the field</param>
        /// <param name="flags">The flags of the field</param>
        /// <param name="save">Should the field be saved into a buffer for later use</param>
        /// <returns>The FieldInfo of the field</returns>
        public static FieldInfo GetField<T>(string name, BindingFlags flags, bool save = true) => GetField(typeof(T), name, flags, save);
        /// <summary>
        /// Gets a field by using reflection with all flags
        /// </summary>
        /// <typeparam name="T">The class where the field is located</typeparam>
        /// <param name="name">The name of the field</param>
        /// <param name="flags">The flags of the field</param>
        /// <param name="save">Should the field be saved into a buffer for later use</param>
        /// <returns>The FieldInfo of the field</returns>
        public static FieldInfo GetField<T>(string name, bool save = true) => GetField<T>(name, STATIC_INSTANCE_FLAG, save);
        #endregion
    }
}
