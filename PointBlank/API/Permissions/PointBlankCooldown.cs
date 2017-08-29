using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Permissions
{
    /// <summary>
    /// Used for managing cooldowns in PointBlank
    /// </summary>
    public class PointBlankCooldown
    {
        #region Properties
        /// <summary>
        /// The permission instance for the cooldown
        /// </summary>
        public PointBlankPermission Permission { get; private set; }

        /// <summary>
        /// The time the cooldown was set/started
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// The time the cooldown will end
        /// </summary>
        public DateTime EndTime { get; private set; }
        #endregion

        public PointBlankCooldown(PointBlankPermission permission, DateTime startTime)
        {
            // Set the variables
            Permission = permission;
            StartTime = startTime;

            // Setup the variables
            if (permission.Cooldown == null)
                EndTime = StartTime;
            else
                EndTime = startTime.AddSeconds((int)permission.Cooldown);
        }
    }
}
