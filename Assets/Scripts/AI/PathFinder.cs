// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// ObjectAhead.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

namespace Assets.Scripts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public class PathFinder : IPathFinder
	{
		private float _viewDistance = 10.0f;
		private float _collisionDistance;
		private float _lookSideWaysDistance = 3;
		private Vector2 _collisionPoint;
		private readonly List<Path> _availablePaths;

		public PathFinder()
		{
			_availablePaths = new List<Path>();
		}

		/// <summary>
		/// Finds handling direction to avoid an obstacle 
		/// </summary>
		/// <param name="gameObject">Moving object</param>
		/// <returns>Handling direction as axis value</returns>
		public float FindWay(GameObject gameObject)
		{
			// Draw view distance
			Debug.DrawRay(
				gameObject.transform.position,
				Vector2.up * _viewDistance,
				Color.blue,
				0);

			_availablePaths.Clear();
			float result = 0f;

			if (ObjectAhead(
				gameObject,
				_viewDistance,
				string.Empty,
				out _collisionPoint,
				out _collisionDistance))
			{
				var path = new Path();

				// First step only from player to first obstacle
				path.Add(new PathStep { StepDirection = 0, StepLength = 0 });
				_availablePaths.Add(path);

				CheckStep(_collisionPoint, ref path);

				var goodPaths =
					_availablePaths.Where(p => p.Length >= _viewDistance)
					.ToArray();

				if (goodPaths.Any())
					result = goodPaths.FirstOrDefault()[1].StepDirection;
			}

			return result;
		}

		/// <summary>
		/// Check if any obstacle is ahead of point
		/// </summary>
		/// <param name="startPoint"></param>
		/// <param name="distance"></param>
		/// <param name="objectTag"></param>
		/// <param name="collisionPoint"></param>
		/// <param name="collisionDistance"></param>
		/// <returns></returns>
		public static bool ObjectAhead(
			Vector2 startPoint,
			float distance,
			string objectTag,
			out Vector2 collisionPoint,
			out float collisionDistance)
		{
			var hit = Physics2D.Raycast(startPoint, Vector2.up, distance);

			Debug.DrawRay(
				startPoint,
				Vector2.up * distance,
				Color.white,
				0);

			if (hit.collider != null && (hit.collider.tag == objectTag || string.IsNullOrEmpty(objectTag)))
			{
				collisionPoint = hit.point;
				collisionDistance = hit.distance;
				return true;
			}
			else
			{
				collisionPoint = Vector2.zero;
				collisionDistance = 0f;
				return false;
			}
		}

		/// <summary>
		/// Check if any obstacle is ahead of gameobject
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="distance"></param>
		/// <param name="objectTag"></param>
		/// <param name="collisionPoint"></param>
		/// <param name="collisionDistance"></param>
		/// <returns></returns>
		public static bool ObjectAhead(
			GameObject owner,
			float distance,
			string objectTag,
			out Vector2 collisionPoint,
			out float collisionDistance)
		{
			var texture = owner.GetComponent<SpriteRenderer>().sprite;
			var raycastPoint1 = owner.transform.position
			                    + Vector3.left * texture.bounds.size.x / 2 
								+ Vector3.left * 0.1f	// Make route a little bit wider
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

			if (leftRayCollided || rightRayCollided)
			{
				// Both collided
				if (hitLeft.collider != null && hitRight.collider != null)
				{
					// Collided with the same obstacle
					if (Math.Abs(hitLeft.distance - hitRight.distance) < 1.28)
					{
						collisionPoint = Vector2.Lerp(
							hitLeft.point,
							hitRight.point,
							0.5f);

						collisionDistance = hitLeft.distance;
					}
					else
					{
						if (hitLeft.distance < hitRight.distance)
						{
							collisionPoint = hitRight.point;
							collisionDistance = hitRight.distance;
						}
						else
						{
							collisionPoint = hitLeft.point;
							collisionDistance = hitLeft.distance;
						}
					}
				}

				// Left collided only
				else if (hitLeft.collider != null)
				{
					collisionPoint = hitLeft.point;
					collisionDistance = hitLeft.distance;
				}

				// Right collided only
				else
				{
					collisionPoint = hitRight.point;
					collisionDistance = hitRight.distance;
				}

				// Draw debug crosshair
				Debug.DrawLine(
					collisionPoint + new Vector2(-0.5f, 0.5f),
					collisionPoint + new Vector2(0.5f, -0.5f),
					Color.green);
				Debug.DrawLine(
					collisionPoint + new Vector2(-0.5f, -0.5f),
					collisionPoint + new Vector2(0.5f, 0.5f),
					Color.green);

				return true;
			}

			collisionPoint = Vector2.zero;
			collisionDistance = 0.0f;

			return false;
		}

		public static NearLinesInfo IsNearLinesFree(
			Vector2 point,
			float distance,
			string objectTag)
		{
			var result = new NearLinesInfo();

			var hitLeft = Physics2D.Raycast(
				point,
				Vector2.left,
				distance);

			Debug.DrawRay(point, Vector2.left * distance, Color.white, 0);

			var hitRight = Physics2D.Raycast(
				point,
				Vector2.right,
				distance);

			Debug.DrawRay(
				point,
				Vector2.right * distance,
				Color.white,
				0);

			result.LeftLineFree =
				!(hitLeft.collider != null
				  && (hitLeft.collider.tag == objectTag || string.IsNullOrEmpty(objectTag)));
			result.RightLineFree =
				!(hitRight.collider != null
				  && (hitRight.collider.tag == objectTag || string.IsNullOrEmpty(objectTag)));

			return result;
		}

		public void CheckStep(
			Vector2 turnPoint,
			ref Path currentPath)
		{
			var lastStep = currentPath.Steps.LastOrDefault();
			lastStep.StepLength = _collisionDistance;
			Vector2 nextPoint;
			float nextStepLength;

			var nearLines = IsNearLinesFree(
				turnPoint,
				_lookSideWaysDistance,
				string.Empty);

			// No paths available - cannot make any turn
			if (!nearLines.LeftLineFree && !nearLines.RightLineFree)
			{
				_availablePaths.Remove(currentPath);
			}
			else if (nearLines.LeftLineFree && nearLines.RightLineFree)
			{
				// Both paths are free

				// Make new path to left
				var newPath = (Path)currentPath.Clone();
				_availablePaths.Add(newPath);

				var leftPoint = turnPoint + Vector2.left * _lookSideWaysDistance;

				Debug.DrawRay(
					leftPoint,
					Vector2.up * (_viewDistance - newPath.Length),
					Color.white,
					0);

				if (ObjectAhead(
					leftPoint,
					_viewDistance - newPath.Length,
					"Building",
					out nextPoint,
					out nextStepLength))
				{
					// if there are further obstacles
					newPath.Add(
						new PathStep
							{
								StepDirection = 1,
								StepLength = nextStepLength
							});
					CheckStep(nextPoint, ref newPath);
				}
				else
				{
					newPath.Add(
						new PathStep
							{
								StepDirection = 1,
								StepLength =
									_viewDistance
									- newPath.Length
							});
				}

				// For path to right use current path
				var rightPoint = turnPoint + Vector2.right * _lookSideWaysDistance;

				Debug.DrawRay(
					rightPoint,
					Vector2.up * (_viewDistance - currentPath.Length),
					Color.white,
					0);

				if (ObjectAhead(
					rightPoint,
					_viewDistance - currentPath.Length,
					string.Empty,
					out nextPoint,
					out nextStepLength))
				{
					currentPath.Add(
						new PathStep
							{
								StepDirection = -1,
								StepLength = nextStepLength
							});
					CheckStep(rightPoint, ref currentPath);
				}
				else
				{
					currentPath.Add(
						new PathStep
							{
								StepDirection = -1,
								StepLength =
									_viewDistance
									- newPath.Length
							});
				}
			}

			// Only one path is free
			else
			{
				if (nearLines.LeftLineFree)
				{
					var leftPoint = turnPoint + Vector2.left * _lookSideWaysDistance;

					Debug.DrawRay(
						leftPoint,
						Vector2.up * (_viewDistance - currentPath.Length),
						Color.white,
						0);

					if (ObjectAhead(
						leftPoint,
						_viewDistance - currentPath.Length,
						string.Empty,
						out nextPoint,
						out nextStepLength))
					{
						currentPath.Add(
							new PathStep
								{
									StepDirection = 1,
									StepLength = nextStepLength
								});
						CheckStep(leftPoint, ref currentPath);
					}
					else
					{
						currentPath.Add(
							new PathStep
								{
									StepDirection = 1,
									StepLength =
										_viewDistance
										- currentPath.Length
								});
					}
				}
				else
				{
					var rightPoint = turnPoint + Vector2.right * _lookSideWaysDistance;

					Debug.DrawRay(
						rightPoint,
						Vector2.up
						* (_viewDistance - currentPath.Length),
						Color.white,
						0);

					if (ObjectAhead(
						rightPoint,
						_viewDistance - currentPath.Length,
						string.Empty,
						out nextPoint,
						out nextStepLength))
					{
						currentPath.Add(
							new PathStep
								{
									StepDirection = -1,
									StepLength = nextStepLength
								});
						CheckStep(rightPoint, ref currentPath);
					}
					else
					{
						currentPath.Add(
							new PathStep
								{
									StepDirection = -1,
									StepLength =
										_viewDistance
										- currentPath.Length
								});
					}
				}
			}
		}

		public struct NearLinesInfo
		{
			public bool LeftLineFree { get; set; }

			public bool RightLineFree { get; set; }
		}


	}
}