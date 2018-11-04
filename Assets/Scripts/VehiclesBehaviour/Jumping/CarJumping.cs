// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;

public class CarJumping : IJumpingBehaviour
{
	private readonly IHandlingBehaviour _handling;
	private readonly GameObject _playerGameObject;
	private readonly IVehicle _currentVehicle;
	private Vector3 _initialScale;
	private float _thisTimeMaxScale;

	
	public CarJumping(GameObject player)
	{
		_playerGameObject = player;
		_handling = _playerGameObject.GetComponent<IVehicle>().Handling;
		_currentVehicle = _playerGameObject.GetComponentInChildren<IVehicle>();
		_initialScale = _playerGameObject.transform.localScale;
	}
	
	public void Update()
	{
		switch (_handling.CurrentCondition)
		{
			case HandlingCondition.Jumping:
				{
					if (_playerGameObject.transform.localScale.x / _initialScale.x < _thisTimeMaxScale)
						_playerGameObject.transform.localScale += new Vector3(Time.deltaTime * 2, Time.deltaTime) * 1.5f;
					else
					{
						_handling.CurrentCondition = HandlingCondition.Falling;
					}
				}
				break;
			case HandlingCondition.Falling:
				{
					if (_playerGameObject.transform.localScale.x / _initialScale.x > 1.0f)
						_playerGameObject.transform.localScale -= new Vector3(Time.deltaTime * 2, Time.deltaTime) * 1.5f;
					else
					{
						_playerGameObject.transform.localScale = _initialScale;
						_handling.CurrentCondition = HandlingCondition.OnGround;
					}
				}
				break;
		}
	}

	public void Jump()
	{
		_thisTimeMaxScale =
			1 + _playerGameObject.GetComponent<Rigidbody2D>().velocity.y / _currentVehicle.MaxSpeed;
		_handling.CurrentCondition = HandlingCondition.Jumping;
	}
}
