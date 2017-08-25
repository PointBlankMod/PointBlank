using System.Threading;
using PointBlank.API.Interfaces;
using PointBlank.API.Extension;
using UnityEngine;

namespace PointBlank.Framework
{
    internal class ExtensionManager : MonoBehaviour, ILoadable
    {
        #region Variables
        private bool _Running = true;
        #endregion

        #region Properties
        public Thread tFrameworkTicker { get; private set; }
        public Thread tAPITicker { get; private set; }
        #endregion

        public void Load()
        {
            // Setup the variables
            tFrameworkTicker = new Thread(new ThreadStart(FrameworkTick));
            tAPITicker = new Thread(new ThreadStart(APITick));

            // Run the code
            tFrameworkTicker.Start();
            tAPITicker.Start();
        }

        public void Unload()
        {
            // Set the variables
            _Running = false;

            // Run the code
            tFrameworkTicker.Abort();
            tAPITicker.Abort();
        }

        #region Thread Functions
        private void FrameworkTick()
        {
            while (_Running)
                ExtensionEvents.RunFrameworkTick();
        }

        private void APITick()
        {
            while (_Running)
                ExtensionEvents.RunAPITick();
        }
        #endregion
    }
}
