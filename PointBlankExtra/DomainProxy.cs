using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PointBlankExtra
{
    [Obsolete("This class should not be used as part of the PointBlank API!")]
    public class DomainProxy : MarshalByRefObject // This class is used for cross domain interaction
    {
        #region Variables
        private AppDomain _PointBlankDomain = null;
        #endregion

        #region Properties
        public AppDomain ProxyDomain { get { return AppDomain.CurrentDomain; } } // Returns the domain the proxy is in
        public AppDomain PointBlankDomain // Returns the pointblank domain
        {
            get
            {
                return _PointBlankDomain;
            }
            set
            {
                if (_PointBlankDomain == null)
                    _PointBlankDomain = value;
            }
        }
        #endregion

        public DomainProxy()
        {
            ProxyDomain.AssemblyResolve += new ResolveEventHandler(OnResolve); // Add the event
        }

        #region Functions
        public Assembly LoadAssembly(string assemblyPath) // Loads assemblies we want
        {
            try
            {
                return Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Events
        protected Assembly OnResolve(object sender, ResolveEventArgs args) // The resolve event
        {
            Console.WriteLine("Resolve! " + args.Name);
            try
            {
                AssemblyName name = new AssemblyName(args.Name);
                Assembly asm;

                asm = ProxyDomain.GetAssemblies().First(a => AssemblyName.ReferenceMatchesDefinition(name, a.GetName())); // Check if the dll is already loaded
                if (asm != null)
                    return asm;

                asm = PointBlankDomain.GetAssemblies().First(a => AssemblyName.ReferenceMatchesDefinition(name, a.GetName())); // Check if the dll is in pointblank
                if (asm != null)
                    return asm;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
