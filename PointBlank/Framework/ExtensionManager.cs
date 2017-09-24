using System.Threading;
using PointBlank.API.Interfaces;
using PointBlank.API.Extension;
using UnityEngine;

namespace PointBlank.Framework
{
    internal class ExtensionManager : MonoBehaviour, ILoadable
    {
        #region Variables
        private bool _running = true;
        #endregion

        #region Properties
        public Thread TFrameworkTicker { get; private set; }
        public Thread TApiTicker { get; private set; }
        #endregion

        public void Load()
        {
            // Setup the variables
            TFrameworkTicker = new Thread(new ThreadStart(FrameworkTick));
            TApiTicker = new Thread(new ThreadStart(ApiTick));

            // Run the code
            TFrameworkTicker.Start();
            TApiTicker.Start();
        }

        public void Unload()
        {
            // Set the variables
            _running = false;

            // Run the code
            TFrameworkTicker.Abort();
            TApiTicker.Abort();
        }

        #region Thread Functions
        private void FrameworkTick()
        {
            while (_running)
                ExtensionEvents.RunFrameworkTick();
        }

        private void ApiTick()
        {
            while (_running)
                ExtensionEvents.RunApiTick();
        }
        #endregion
    }
}
