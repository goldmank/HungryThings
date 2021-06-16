using System;
using UnityEngine;

namespace Game.Scripts.Audio
{
    [Serializable]
    public class MusicData
    {
        public MusicType Type;
        public AudioClip Track;
        public float MaxVolume;
    }
}