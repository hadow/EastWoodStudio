using System;

namespace EW.Mobile.Platforms.Graphics
{
    internal partial class GraphicsCapabilities
    {

        public GraphicsCapabilities(GraphicsDevice graphicsDevice)
        {

        }

        internal bool SupportsDxt1 { get; private set; }

        internal bool SupportsPvrtc { get; private set; }

        internal bool SupportsEtc1 { get; private set; }

        internal bool SupportsTextrueArrays { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        internal void Initialize(GraphicsDevice device)
        {

        }
    }
}