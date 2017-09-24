using System.Threading;
using PointBlank.API.Interfaces;
using PointBlank.API.Extension;
using UnityEngine;

namespace PointBlank.Framework
{
    internal class ExtensionManager : MonoBehaviour, ILoadable
    {
        #region Variables
<<<<<<< HEAD
        private bool _Running = true;
        #endregion

        #region Properties
        public Thread tFrameworkTicker { get; private set; }
        public Thread tAPITicker { get; private set; }
=======
        private bool _running = true;
        #endregion

        #region Properties
        public Thread TFrameworkTicker { get; private set; }
        public Thread TApiTicker { get; private set; }
>>>>>>> master
        #endregion

        public void Load()
        {
            // Setup the variables
<<<<<<< HEAD
            tFrameworkTicker = new Thread(new ThreadStart(FrameworkTick));
            tAPITicker = new Thread(new ThreadStart(APITick));

            // Run the code
            tFrameworkTicker.Start();
            tAPITicker.Start();
=======
            TFrameworkTicker = new Thread(new ThreadStart(FrameworkTick));
            TApiTicker = new Thread(new ThreadStart(ApiTick));

            // Run the code
            TFrameworkTicker.Start();
            TApiTicker.Start();
>>>>>>> master
        }

        public void Unload()
        {
            // Set the variables
<<<<<<< HEAD
            _Running = false;

            // Run the code
            tFrameworkTicker.Abort();
            tAPITicker.Abort();
=======
            _running = false;

            // Run the code
            TFrameworkTicker.Abort();
            TApiTicker.Abort();
>>>>>>> master
        }

        #region Thread Functions
        private void FrameworkTick()
        {
<<<<<<< HEAD
            while (_Running)
                ExtensionEvents.RunFrameworkTick();
        }

        private void APITick()
        {
            while (_Running)
                ExtensionEvents.RunAPITick();
=======
            while (_running)
                ExtensionEvents.RunFrameworkTick();
        }

        private void ApiTick()
        {
            while (_running)
                ExtensionEvents.RunApiTick();
>>>>>>> master
        }
        #endregion
    }
}
