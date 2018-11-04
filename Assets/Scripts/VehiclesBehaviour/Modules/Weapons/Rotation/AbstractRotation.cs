using UnityEngine;

public abstract class AbstractRotation {

    protected void RotateToTarget(GameObject gameObject, Vector3 target) {
        Quaternion rot =
            Quaternion.LookRotation(
                gameObject.transform.position - target,
                Vector3.forward);
        gameObject.transform.rotation = Quaternion.Euler(
            0,
            0,
            rot.eulerAngles.z);
    }
}

