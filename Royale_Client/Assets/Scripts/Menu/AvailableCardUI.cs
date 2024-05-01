using UnityEngine;
using UnityEngine.UI;

public class AvailableCardUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _lockedColor;

    private CardStateType _currentState = CardStateType.NONE;
    [SerializeField] private CardSelector _selector;
    [SerializeField] private int _id;
    #region Editor
#if UNITY_EDITOR
    [SerializeField] private Image _image;

    public void Create(CardSelector selector, Card card, int id)
    {
        _selector = selector;
        _id = id;
        _image.sprite = card.Sprite;
        _text.text = card.Name;
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
    #endregion

    public void SetState(CardStateType state)
    {
        _currentState = state;
        switch (_currentState)
        {
            case CardStateType.AVAILABLE:
                _text.color = _availableColor;
                break;
            case CardStateType.SELECTED:
                _text.color = _selectedColor;
                break;
            case CardStateType.LOCKED:
                _text.color = _lockedColor;
                break;
            default:
                break;
        }
    }

    public void Click()
    {
        switch (_currentState)
        {
            case CardStateType.AVAILABLE:
                _selector.SelectCard(_id);
                SetState(CardStateType.SELECTED);
                break;
            case CardStateType.SELECTED:
                break;
            case CardStateType.LOCKED:
                break;
            default:
                break;
        }
    }

    public enum CardStateType
    {
        NONE = 0,
        AVAILABLE = 1,
        SELECTED = 2,
        LOCKED = 3
    }
}