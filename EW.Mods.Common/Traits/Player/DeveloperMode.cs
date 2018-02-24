﻿using System;
using EW.Traits;
namespace EW.Mods.Common.Traits
{
    public class DeveloperModeInfo : ITraitInfo
    {
        public object Create(ActorInitializer init) { return new DeveloperMode(); }
    }
    public class DeveloperMode:ISync,INotifyCreated
    {
        public bool Enabled { get; private set; }

        [Sync]bool pathDebug;

        public bool PathDebug { get { return Enabled && pathDebug; } }

        void INotifyCreated.Created(Actor self)
        {

        }
    }
}