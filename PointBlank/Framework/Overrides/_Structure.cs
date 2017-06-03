using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Detour;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.Framework.Overrides
{
    internal class _Structure
    {
        public void askDamage(Vector3 pos , float StructureDamage , float Radius)
        {
           DamageTool.explode(pos, Radius, EDeathCause.ANIMAL, CSteamID.Nil, 0, 0, 0, 0, StructureDamage, 0, 0, 0, EExplosionDamageType.CONVENTIONAL, Radius, false);
        }
    }
}
