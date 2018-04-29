// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// StupidPathFinder.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts
{
	using System;
	using UnityEngine;

	public class StupidPathFinder : IPathFinder
	{
		private float _viewDistance = 10.0f;

		public StupidPathFinder()
		{

		}

		/// <summary>
		/// Finds handling direction to avoid an obstacle 
		/// </summary>
		/// <param name="gameObject">Moving object</param>
		/// <returns>Handling direction as axis value</returns>
		public float FindWay(GameObject gameObject)
		{
			float result = 0f, collisionLeftDistance, collisionRightDistance;

			if (ObjectAhead(
				gameObject,
				_viewDistance,
				string.Empty,
				out collisionLeftDistance,
				out collisionRightDistance))
			{
				if (collisionRightDistance < 0) return 1;
				if (collisionLeftDistance < 0) return -1;

				return collisionRightDistance > collisionLeftDistance
					       ? 1.0f
					       : -1.0f;
			}

			return result;
		}

		/// <summary>
		/// Check if any obstacle is ahead of gameobject
		/// and return distanse between left and right
		/// sides of gameObject to an obstacle
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="distance"></param>
		/// <param name="objectTag"></param>
		/// <param name="leftDistance"></param>
		/// <param name="rightDistance"></param>
		/// <returns>Value of the horizontal input axis</returns>
		private static bool ObjectAhead(
			GameObject owner,
			float distance,
			string objectTag,
			out float leftDistance,
			out float rightDistance)
		{
			leftDistance = 0;
			rightDistance = 0;

			var texture = owner.GetComponent<SpriteRenderer>().sprite;
			var raycastPoint1 = owner.transform.position
								+ Vector3.left * texture.bounds.size.x / 2
								+ Vector3.left * 0.1f   // Make route a little bit wider
								+ Vector3.up * texture.bounds.size.y / 2;

			var raycastPoint2 = owner.transform.position
								+ Vector3.right * texture.bounds.size.x / 2
								+ Vector3.right * 0.1f  // Make route a little bit wider
								+ Vector3.up * texture.bounds.size.y / 2;

			var hitLeft = Physics2D.Raycast(
				raycastPoint1,
				Vector2.up,
				distance);

			Debug.DrawRay(
				raycastPoint1,
				Vector2.up * distance,
				Color.green,
				0);

			var hitRight = Physics2D.Raycast(
				raycastPoint2,
				Vector2.up,
				distance);

			Debug.DrawRay(
				raycastPoint2,
				Vector2.up * distance,
				Color.green,
				0);

			// At least one ray collided with obstacle
			bool leftRayCollided = hitLeft.collider != null
				&& (hitLeft.collider.tag == objectTag
				|| string.IsNullOrEmpty(objectTag));
			bool rightRayCollided = hitRight.collider != null
				&& (hitRight.collider.tag == objectTag
				|| string.IsNullOrEmpty(objectTag));

			if (!leftRayCollided && !rightRayCollided) return false;

			leftDistance = leftRayCollided ? hitLeft.distance : -1;
			rightDistance = rightRayCollided ? hitRight.distance : -1;
			
			return true;
		}

		// TODO : Remove from here somewhere
		private static void DrawDebugCrosshair(
			Vector2 point,
			Color? color = null)
		{
			Debug.DrawLine(
				point + new Vector2(-0.5f, 0.5f),
				point + new Vector2(0.5f, -0.5f),
				color ?? Color.white);
			Debug.DrawLine(
				point + new Vector2(-0.5f, -0.5f),
				point + new Vector2(0.5f, 0.5f),
				color ?? Color.white);
		}
	}
}