using System.Collections.Generic;
using Assets.Scripts.Basics;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.AI {
    class EnemiesController : MbSingleton<EnemiesController> {

        [SerializeField] private GameObject _enemiesHolder;

        [SerializeField] private GameObject _player;

        [SerializeField] private List<GameObject> _enemiesPrefabs;


        private List<GameObject> _enemies = new List<GameObject>();
        
        public List<IDestroyable> Enemies {
            get {
                return _enemiesPrefabs.Select(w => w.GetComponent<IDestroyable>()).ToList();
            }
        }

        private float timeToSpawn = 0;
        private float spawnCooldown = 1.5f;


        private void Start() {
            
        }


        private void Update() {

            if (timeToSpawn <= 0 && _enemies.Count < 10) {
                var enemy = SpawnEnemy();
                timeToSpawn = spawnCooldown;
                return;
            }

            timeToSpawn -= Time.deltaTime;
        }

        private GameObject SpawnEnemy() {
            var spawnPos = new Vector3(Random.Range(2, 15), _player.transform.position.y - 10, 0);
            var spawnRot = Quaternion.identity;
            var enemy = Instantiate(_enemiesPrefabs[0], spawnPos, spawnRot, _enemiesHolder.transform);
            return enemy;
        }


    }
}


