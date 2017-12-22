namespace PointBlank.API.Extension.Loader
{
    /// <summary>
    /// Extending this class will allow you to load your class using the InternalLoader or let the mod loader load it automatically
    /// </summary>
    public abstract class InternalObject : ILoadable
    {
        #region Abstract Functions
        /// <summary>
        /// Called when the InternalObject is being loaded
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Called when the InternalObject is being unloaded
        /// </summary>
        public abstract void Unload();
        #endregion
    }
}
