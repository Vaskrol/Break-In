using UnityEngine;


public abstract class AbstractWeapon : MonoBehaviour {

    public int Level;

    public GameObject GameObject { get { return gameObject; } }
}

