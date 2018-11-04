// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
	public float offsetY;

    // Use this for initialization
    private void Start()
    {

    }


    // Update is called once per frame
    private void Update()
    {
		transform.position = new Vector3(
			transform.position.x,
			target.position.y + offsetY,
			transform.position.z);
    }
}

