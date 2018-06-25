// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// PistolA.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016
using Assets.Scripts.Vehicles.Weapons.Rotation;
using UnityEngine;

public class PistolA : AbstractWeapon, IWeapon {

    public float Cooldown { get; set; }

	public float Damage { get; set; }    

    public IRotationBehaviour RotationBehaviour { get; set; }

    private float curCooldown = 0;

	private float spreading = 10f;

	private GameObject _hitExplosionPrefab;

	public void Fire(Vector2 aim)
	{
		var spread = Random.Range(-spreading / 2, spreading / 2);
		Vector3 firingDirection = Quaternion.AngleAxis(spread, Vector3.back) * transform.up * 20;

		if (curCooldown == 0.0f)
		{
            curCooldown = Cooldown;

            RaycastHit2D hit = 
				Physics2D.Raycast(transform.position, firingDirection);

			DrawBullitTrail(transform.position, firingDirection);

			if (hit.collider != null)
			{
				Instantiate(
					_hitExplosionPrefab, 
					new Vector3(hit.point.x, hit.point.y), 
					Quaternion.identity);


                var vController = hit.collider.gameObject.GetComponent<VehicleController>();
                if (vController == null)
                    return;

                var vehicle = vController.CurrentVehicle;
                vehicle.RecieveDamage(Damage, DamageType.Bullit);
            }
		}
	}

	void Start()
	{
		RotationBehaviour = new NoRotation();
        Level = 1;
        Cooldown = 0.5f;
		Damage = 25f;

		_hitExplosionPrefab = (GameObject)
			Resources.Load(
				"Prefabs/ParticleSystems/BulletExplosionA",
				typeof(GameObject));
	}

	void Update()
	{
		if (curCooldown > 0)
			curCooldown -= Time.deltaTime;
		else
		{
			curCooldown = 0;
		}
	}

	public void UpdateRotation()
	{
        // Not rotated weapon
	}
}
