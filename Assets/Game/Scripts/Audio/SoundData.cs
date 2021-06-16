using System;
using UnityEngine;

namespace Game.Scripts.Audio
{
    [Serializable]
    public class SoundData
    {
        public SoundType Type;
        public AudioClip Track;
        public float MaxVolume;
    }
}