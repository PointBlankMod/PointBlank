﻿namespace PointBlank.API.Extension.Loader
{
    /// <summary>
    /// Makes a class loadable
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Called when a class is loading
        /// </summary>
        void Load();

        /// <summary>
        /// Called when a class is unloading
        /// </summary>
        void Unload();
    }
}