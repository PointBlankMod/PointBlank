using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PointBlank.API.Interfaces;

namespace PointBlank
{
    internal static class PBEnvironment
    {
        #region ModLoader Variables
        public static Dictionary<string, ModLoaderObject> ModLoaderObjects = new Dictionary<string, ModLoaderObject>();
        #endregion

        #region Classes
        public class ModLoaderObject
        {
            #region Properties
            public GameObject GameObject { get; private set; }
            public ILoadable LoaderObject { get; private set; }
            #endregion

            public ModLoaderObject(GameObject gameobject, ILoadable loaderobject)
            {
                GameObject = gameobject;
                LoaderObject = loaderobject;
            }
        }
        #endregion
    }
}
