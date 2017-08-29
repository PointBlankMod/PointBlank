using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API.Commands;
using PointBlank.API.Services;
using PointBlank.API.Permissions;
using PointBlank.API.DataManagment;

namespace PointBlank.Services.PermissionManager
{
    internal class PermissionManager : PointBlankService
    {
        #region Properties
        public static string PermissionsPath => PointBlankServer.ConfigurationsPath + "/Permissions";

        public UniversalData UniPermissionsConfig { get; private set; }

        public JsonData PermissionsConfig { get; private set; }

        public static List<PointBlankPermission> Permissions { get; private set; }

        public override int LaunchIndex => 0;
        #endregion

        public override void Load()
        {
            // Set the variables
            Permissions = new List<PointBlankPermission>();
            UniPermissionsConfig = new UniversalData(PermissionsPath);
            PermissionsConfig = UniPermissionsConfig.GetData(EDataType.JSON) as JsonData;
        }

        public override void Unload() => Save();

        #region Public Functions
        public void Save()
        {
            foreach(PointBlankPermission permission in Permissions)
            {
                if (PermissionsConfig.Document[permission.Permission] == null)
                    PermissionsConfig.Document.Add(permission.Permission, permission.Cooldown);
                else
                    PermissionsConfig.Document[permission.Permission] = permission.Cooldown;
            }
            UniPermissionsConfig.Save();
        }

        public void AddPermission(PointBlankPermission permission)
        {
            Permissions.Add(permission);
            Save();
        }

        public void RemovePermission(PointBlankPermission permission)
        {
            Permissions.Remove(permission);
            Save();
        }
        #endregion
    }
}
