
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {

    [SerializeField] private Image _barBackground; 
    [SerializeField] private Image _bar;

    public void SetPercent(float value) {
        var height = _bar.rectTransform.rect.height;
        var width = _barBackground.rectTransform.rect.width * value / 100f;
        _bar.rectTransform.sizeDelta = new Vector2(width, height);
    }
}
