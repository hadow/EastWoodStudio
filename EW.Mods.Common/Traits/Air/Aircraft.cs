﻿using System;
using System.Collections.Generic;

namespace EW.Mods.Common.Traits
{

    public class AircraftInfo : ITraitInfo
    {
        public object Create(ActorInitializer init)
        {
            return new Aircraft();
        }
    }

    public class Aircraft
    {
    }
}