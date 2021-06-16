using UnityEngine.Events;

namespace IO.Infra.Scripts.Events
{
    [System.Serializable]
    public class GameEvent : UnityEvent<EventParams>
    {
    }
}