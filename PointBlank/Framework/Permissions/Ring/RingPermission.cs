using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Permissions;
using PointBlank.API.Implements;

namespace PointBlank.Framework.Permissions.Ring
{
    [Serializable]
    internal sealed class RingPermission : CodeAccessPermission, IUnrestrictedPermission
    {
        #region Variables
        private RingPermissionRing _ring;
        #endregion

        #region Properties
        public RingPermissionRing Ring
        {
            get => _ring;
            set
            {
                VerifyRing(value);
                _ring = value;
            }
        }
        #endregion

        public RingPermission(PermissionState state)
        {
            switch (state)
            {
                case PermissionState.None:
                    SetUnrestricted(false);
                    return;
                case PermissionState.Unrestricted:
                    SetUnrestricted(true);
                    return;
            }
            throw new ArgumentException("Invalid permission state!");
        }

        public RingPermission(RingPermissionRing ringFlag)
        {
            VerifyRing(ringFlag);
            _ring = ringFlag;
        }

        #region IUnrestrictedPermission Functions
        public bool IsUnrestricted() => (_ring == RingPermissionRing.Framework);
        #endregion

        #region CodeAccessPermission Functions
        public override bool IsSubsetOf(IPermission target)
        {
            if (target == null)
                return (_ring == RingPermissionRing.None);

            RingPermission ringPermissions = (RingPermission)target;

            if (ringPermissions.IsUnrestricted())
                return true;
            else if (IsUnrestricted())
                return false;
            else
                return IsAllowed(ringPermissions);
        }

        public override IPermission Intersect(IPermission target)
        {
            if (target == null)
                return null;

            RingPermission ringPermissions = (RingPermission)target;
            RingPermissionRing ringPermissionsRing = (_ring < ringPermissions.Ring ? _ring : ringPermissions.Ring);

            return ringPermissionsRing == RingPermissionRing.None ? null : new RingPermission(ringPermissionsRing);
        }

        public override IPermission Union(IPermission target)
        {
            if (target == null)
                return Copy();

            RingPermission ringPermissions = (RingPermission)target;
            RingPermissionRing ringPermissionsRing = (_ring < ringPermissions.Ring ? _ring : ringPermissions.Ring);

            return ringPermissionsRing == RingPermissionRing.None ? null : new RingPermission(ringPermissionsRing);
        }

        public override IPermission Copy() => new RingPermission(_ring);

        public override SecurityElement ToXml()
        {
            SecurityElement element = new SecurityElement("IPermission");
            Type type = GetType();
            StringBuilder sb = new StringBuilder(type.Assembly.ToString());

            sb.Replace('\"', '\'');
            element.AddAttribute("class", type.FullName + ", " + sb);
            element.AddAttribute("version", "1");
            if (!IsUnrestricted())
            {
                if (_ring != RingPermissionRing.None)
                    element.AddAttribute("Ring", Enum.GetName(typeof(RingPermission), _ring));
            }
            else
            {
                element.AddAttribute("Unrestricted", "true");
            }
            return element;
        }

        public override void FromXml(SecurityElement elem)
        {
            string eUnrestricted = elem.Attribute("Unrestricted");
            string eRing = elem.Attribute("Ring");

            if (eUnrestricted != null)
                SetUnrestricted(Convert.ToBoolean(eUnrestricted));
            if (eRing != null)
                _ring = (RingPermissionRing)Enum.Parse(typeof(RingPermissionRing), eRing);
        }
        #endregion

        #region Private Functions
        private void SetUnrestricted(bool unrestricted)
        {
            if(unrestricted)
                Ring = RingPermissionRing.Framework;
        }

        private void VerifyRing(RingPermissionRing ring)
        {
            if (ring < RingPermissionRing.None || ring > RingPermissionRing.Framework)
                throw new ArgumentException("Invalid ring!");
        }

        private bool IsAllowed(RingPermission rp)
        {
            if (_ring == RingPermissionRing.None)
                return true;
            return rp.Ring != RingPermissionRing.None && rp.Ring.HasFlag(_ring);
        }
        #endregion
    }
}
