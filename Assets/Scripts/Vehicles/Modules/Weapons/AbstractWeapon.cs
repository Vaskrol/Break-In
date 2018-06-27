using UnityEngine;


public abstract class AbstractWeapon : MonoBehaviour {

    public int Level;

    public GameObject GameObject { get { return gameObject; } }

    private Color _trailColor = Color.black;

    protected void SetTrailColor(Color color) {
        _trailColor = color;
    }

    protected void DrawBullitTrail(Vector3 start, Vector3 direction) {
        var trailEnd = start + direction;
        var trailEndObject = new GameObject();

        trailEndObject.name = "Trail_end";

        var lineRenderer = trailEndObject.AddComponent(typeof(LineRenderer))
            as LineRenderer;
        var destroyer = trailEndObject.AddComponent(typeof(ObjectDestroyer))
            as ObjectDestroyer;
        var fader = trailEndObject.AddComponent(typeof(ObjectFader))
            as ObjectFader;

        destroyer.LifeTime = 1;
        fader.TimeToFade = 1;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var g = new Gradient();
        var colorKeys = new GradientColorKey[2];
        var alphaKeys = new GradientAlphaKey[2];

        colorKeys[0].color = _trailColor;
        colorKeys[0].time = 0.0F;
        colorKeys[1].color = _trailColor;
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

