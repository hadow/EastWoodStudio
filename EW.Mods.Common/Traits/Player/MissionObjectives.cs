﻿using System;
using System.Collections.Generic;


namespace EW.Mods.Common.Traits
{
    public class MissionObjectives
    {
    }



    public class ObjectivesPanelInfo : ITraitInfo
    {
        public object Create(ActorInitializer init) { return new ObjectivesPanel(); }
    }

    public class ObjectivesPanel
    {

    }

}