using MapSystem;
using UnityEngine;

namespace Game {
    public class GameController : MonoBehaviour {

        [SerializeField] private MapClient _mapClient;

        private static GameController _instance;
        public static GameController Instance {
            get { return _instance; }
        }

        private void Start() {
            _instance = this;
        }

        public void SetMap() {
            
        }

        private void Update() {
            
        }

        private void OnDestroy() {
            _instance = null;
        }
    }
}