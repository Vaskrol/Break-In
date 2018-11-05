using System;

namespace Configs.Maps {
    
    [Serializable]
    public class MapConfig {
        [SerializeFi] public int Width;
        [Serializable] public int Length;
    }
}