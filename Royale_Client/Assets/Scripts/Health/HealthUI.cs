using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private Text _valueText;
    [SerializeField] private float _defaultWidth;

    private void OnValidate()
    {
        _defaultWidth = _filledImage.sizeDelta.x;
    }

    public void UpdateHealth(float max, float current)
    {
        float percent = current / max;
        _filledImage.sizeDelta = new Vector2(_defaultWidth * percent, _filledImage.sizeDelta.y);

        _valueText.text = current.ToString();
    }

    private void LateUpdate()
    {
        Vector3 newAngles = transform.eulerAngles;
        newAngles.y = 0;

        transform.eulerAngles = newAngles;
    }
}
