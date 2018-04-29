// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// PistolA.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using Assets.Scripts;
using Assets.Scripts.Vehicles.Weapons.Rotation;

using UnityEngine;

public class PistolA : MonoBehaviour, IWeapon
{
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
			RaycastHit2D hit = 
				Physics2D.Raycast(transform.position, firingDirection);

			DrawBullitTrail(transform.position, firingDirection);

			if (hit.collider != null)
			{
				Instantiate(
					_hitExplosionPrefab, 
					new Vector3(hit.point.x, hit.point.y), 
					Quaternion.identity);

				var destroyableComponents =
					hit.collider.gameObject.GetComponents<IDestroyable>();

				if (destroyableComponents.Length > 0)
				{
					foreach (var component in destroyableComponents)
					{
						component.RecieveDamage(Damage, "bullit");
					}
				}
			}

			curCooldown += Cooldown;
		}
	}

	void Start()
	{
		RotationBehaviour = new NoRotation();
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

	}

	private void DrawBullitTrail(Vector3 start, Vector3 direction)
	{
		var trailEnd = start + direction;
		var trailEndObject = Instantiate(new GameObject());

		var lineRenderer = trailEndObject.AddComponent(typeof(LineRenderer)) 
			as LineRenderer;
		var destroyer = trailEndObject.AddComponent(typeof(ObjectDestroyer)) 
			as ObjectDestroyer;
		var fader = trailEndObject.AddComponent(typeof(ObjectFader))
			as ObjectFader;

		destroyer.LifeTime = 1;
		fader.TimeToFade = 1;

		lineRenderer.startWidth = 0.01f;
		lineRenderer.endWidth = 0.01f;
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

		var g = new Gradient();
		var colorKeys = new GradientColorKey[2];
		var alphaKeys = new GradientAlphaKey[2];

		colorKeys[0].color = Color.white;
		colorKeys[0].time = 0.0F;
		colorKeys[1].color = Color.white;
		colorKeys[1].time = 1.0F;
		
		alphaKeys[0].alpha = 0.8F;
		alphaKeys[0].time = 0.0F;
		alphaKeys[1].alpha = 0.0F;
		alphaKeys[1].time = 1.0F;

		g.SetKeys(colorKeys, alphaKeys);

		lineRenderer.colorGradient = g;
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, trailEnd);


	}
}
