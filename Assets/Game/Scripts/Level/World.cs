using UnityEngine;

namespace Game.Scripts.Level
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        public Vector3 SpawnPoint => _spawnPoint.position;
    }
}