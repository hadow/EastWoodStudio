﻿using System;
using EW.Traits;
namespace EW.Mods.Common.Traits
{

    public class TransformsInfo : ITraitInfo
    {
        public virtual object Create(ActorInitializer init) { return new Transforms(); }
    }

    public class Transforms
    {
    }
}