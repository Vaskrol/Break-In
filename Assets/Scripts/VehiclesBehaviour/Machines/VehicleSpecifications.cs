using UnityEngine;
namespace Vehicles.Machines {
    public class VehicleSpecifications {
        
        public float Acceleration { get; set; }

        public float Braking { get; set; }

        public float MaxSpeed { get; set; }

        public float Mass { get; set; }

        public float Steering { get; set; }

        public float UserAccelerating { get; set; }

        public float HealthPoints { get; set; }

        public float MaxHealthPoints { get; set; }

        public Vector2 CenterOfMass { get; set; }
    }
}