using System;
using System.IO;
using Android.App;
namespace EW.Xna.Platforms
{
    partial class TitleContainer
    {

        private static Stream PlatformOpenStream(string safeName)
        {
            return Application.Context.Assets.Open(safeName); 
        }

    }
}