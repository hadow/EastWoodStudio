﻿using System;

namespace EW.Mods.Common.Traits
{

    public class SpawnMPUnitsInfo : ITraitInfo
    {
        public object Create(ActorInitializer init) { return new SpawnMPUnits(); }
    }

    public class SpawnMPUnits
    {
    }
}