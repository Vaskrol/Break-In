using System;
using System.Linq;
using Tools;
using UnityEngine;
using Vehicles.Machines;

namespace VehiclesBehaviour.Handling.PlayerHandling {

    public class ArcadeCarPhysics : IHandlingBehaviour {

        public GameObject        HandlingObject   { get; set; }
        public Vector3           CurrentVelocity  { get; private set; }
        public HandlingCondition CurrentCondition { get; set; }

        private readonly VehicleBase _currentVehicle;
        private float _h;

        public ArcadeCarPhysics(GameObject player, VehicleBase currentVehicle) {
            HandlingObject  = player;
            _currentVehicle = currentVehicle;
            SetSpecifications();
            CurrentCondition = HandlingCondition.OnGround;
        }

        public void Update() {
            ProcessInput();
            ProcessPhysics();
        }
        
        private void ProcessPhysics() {
            HandlingObject.transform.Translate(Vector3.up * 0.1f);
            
            if (Math.Abs(_h) < 0.01f)
                return;
            
            HandlingObject.transform.Translate(Vector3.left * _h * _currentVehicle.Performance.Steering);
        }

        private void ProcessInput() {
            var userHandlingPos = Camera.main.ScreenToWorldPoint(InputTool.InputPosition).x - HandlingObject.transform.position.x;
            _h = - Mathf.Clamp(userHandlingPos, -1, 1);
        }

        public void InstallEquipment() {
            foreach (var eqSlot in _currentVehicle.Slots.OfType<EquipmentSlot>()) {
                eqSlot.Equipment.Install(_currentVehicle);
            }

            SetSpecifications();
        }

        private void SetSpecifications() {
            HandlingObject.GetComponent<Rigidbody2D>().mass
                = _currentVehicle.Performance.Mass;
        }
    }
}
