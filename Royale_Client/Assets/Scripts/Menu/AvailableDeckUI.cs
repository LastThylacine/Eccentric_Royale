using System.Collections.Generic;
using UnityEngine;

public class AvailableDeckUI : MonoBehaviour
{
    [SerializeField] private CardSelector _selector;
    [SerializeField] private List<AvailableCardUI> _availableCardUI = new List<AvailableCardUI>();

    #region Editor
#if UNITY_EDITOR
    [SerializeField] private Transform _availableCardParent;
    [SerializeField] private AvailableCardUI _availableCardUIPrefab;

    public void SetAllCardsCount(Card[] cards)
    {
        for (int i = 0; i < _availableCardUI.Count; i++)
        {
            GameObject gameObject = _availableCardUI[i].gameObject;
            UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(gameObject);
        }

        _availableCardUI.Clear();

        for (int i = 1; i < cards.Length; i++)
        {
            AvailableCardUI card = Instantiate(_availableCardUIPrefab, _availableCardParent);
            card.Create(_selector, cards[i], i);
            _availableCardUI.Add(card);
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
    #endregion

    public void UpdateCardsList(IReadOnlyList<Card> available, IReadOnlyList<Card> selected)
    {
        for (int i = 0; i < _availableCardUI.Count; i++)
        {
            _availableCardUI[i].SetState(AvailableCardUI.CardStateType.LOCKED);
        }

        for (int i = 0; i < available.Count; i++)
        {
            _availableCardUI[available[i].ID - 1].SetState(AvailableCardUI.CardStateType.AVAILABLE);
        }

        for (int i = 0; i < selected.Count; i++)
        {
            _availableCardUI[selected[i].ID - 1].SetState(AvailableCardUI.CardStateType.SELECTED);
        }
    }
}
