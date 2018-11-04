// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// PistolA.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016
using UnityEngine;
using Vehicles.Machines;

public class E_PistolA : AbstractWeapon, IWeapon {

    [SerializeField] private GameObject _hitExplosionPrefab;

    public float Cooldown { get; set; }

    public float Damage { get; set; }

    public IRotationBehaviour RotationBehaviour { get; set; }    

    private float curCooldown = 0;

    private float spreading = 10f;

    public void Fire(Vector2 aim) {
        var spread = Random.Range(-spreading / 2f, spreading / 2f);
        var spreadAngle = Quaternion.AngleAxis(spread, Vector3.back);
        Vector3 firingDirection = spreadAngle * transform.up * 20;

        if (curCooldown == 0.0f) {
            curCooldown = Cooldown;

            RaycastHit2D hit =
                Physics2D.Raycast(transform.position, firingDirection);

            DrawBullitTrail(transform.position, firingDirection);

            if (hit.collider != null) {
                var targetPoint = new Vector2(hit.point.x, hit.point.y);

                var hitExplosion = Instantiate(
                    _hitExplosionPrefab,
                    targetPoint,
                    Quaternion.identity)
                    .AddComponent<ObjectDestroyer>();
                hitExplosion.name = "Hit Explosion";
                DebugDrawer.DrawCross(targetPoint, Color.red);
                

                var vController = hit.collider.gameObject.GetComponent<VehicleController>();
                if (vController == null)
                    return;

                var vehicle = vController.CurrentVehicle;
                vehicle.ReceiveDamage(Damage, DamageType.Bullit);
            }
        }
    }

    void Start() {
        RotationBehaviour = new E_FullRotation();
        Level = 1;
        Cooldown = 2f;
        Damage = 5f;
        SetTrailColor(Color.red);
    }

    void Update() {
        if (curCooldown > 0)
            curCooldown -= Time.deltaTime;
        else {
            curCooldown = 0;
        }
    }

    public void UpdateRotation() {
        RotationBehaviour.PerformRotation(gameObject);
    }
}
