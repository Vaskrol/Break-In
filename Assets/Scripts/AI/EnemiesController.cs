using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Basics;
using UnityEngine;

namespace AI {
    class EnemiesController : MbSingleton<EnemiesController> {

        [SerializeField] private GameObject _enemiesHolder;

        [SerializeField] private GameObject _player;

        [SerializeField] private List<GameObject> _enemiesPrefabs;


        private readonly List<GameObject> _enemies = new List<GameObject>();
        
        public List<IDestroyable> Enemies {
            get {
                return _enemiesPrefabs.Select(w => w.GetComponent<IDestroyable>()).ToList();
            }
        }

        private float _timeToSpawn = 0;
        private const float SpawnCooldown = 1.5f;

        private void Update() {

            if (_timeToSpawn <= 0 && _enemies.Count < 10) {
                var enemy = SpawnEnemy();
                _enemies.Add(enemy);
                _timeToSpawn = SpawnCooldown;
                return;
            }

            _timeToSpawn -= Time.deltaTime;
        }

        private GameObject SpawnEnemy() {
            var spawnPos = new Vector3(Random.Range(2, 15), _player.transform.position.y - 10, 0);
            var spawnRot = Quaternion.identity;
            var enemy = Instantiate(_enemiesPrefabs[0], spawnPos, spawnRot, _enemiesHolder.transform);
            
            return enemy;
        }


    }
}


