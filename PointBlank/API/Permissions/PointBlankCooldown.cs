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

        /// <summary>
        /// The target object
        /// </summary>
        public object Target { get; private set; }

        /// <summary>
        /// Is the cooldown expired already
        /// </summary>
        public bool IsExpired => ((EndTime - DateTime.Now).TotalMilliseconds <= 0);
        #endregion

        /// <summary>
        /// Creates a cooldown instance using the provided information
        /// </summary>
        /// <param name="permission">The permission the cooldown is applied to</param>
        /// <param name="startTime">The cooldown start time</param>
        /// <param name="target">The target object the cooldown is attached to</param>
        public PointBlankCooldown(PointBlankPermission permission, DateTime startTime, object target)
        {
            // Set the variables
            Permission = permission;
            StartTime = startTime;
            Target = target;

            // Setup the variables
            if (permission.Cooldown == null)
                EndTime = StartTime;
            else
                EndTime = startTime.AddSeconds((int)permission.Cooldown);
        }
    }
}
