using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidsComponent : MonoBehaviour {

    [SerializeField] private Transform[] _skiddingWheels = null; // set from editor

    [SerializeField] private GameObject _handlingObject= null; // set from editor

    [SerializeField] private GameObject _skidPrefab = null;

    // Use this for initialization
    void Start () {
        var vehicleRb = _handlingObject.GetComponent<Rigidbody2D>();
        var speed = vehicleRb.velocity;

        foreach (var wheel in _skiddingWheels) {
            var trail = Instantiate(_skidPrefab.gameObject);
            trail.transform.SetParent(wheel, true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		var angle = _handlingObject.transform.rotation.eulerAngles.z;

        if (Mathf.Abs(angle) < 0.1f)
            return;

        // Turn off skids when we are going straight
    }
}
