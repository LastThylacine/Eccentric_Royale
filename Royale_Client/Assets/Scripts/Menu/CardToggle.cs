using UnityEngine;

public class CardToggle : MonoBehaviour
{
    [SerializeField] private CardSelector _selector;
    [SerializeField] private int _index = 0;

    public void Click(bool value)
    {
        if (!value) return;

        _selector.SetSelectToggleIndex(_index);
    }
}