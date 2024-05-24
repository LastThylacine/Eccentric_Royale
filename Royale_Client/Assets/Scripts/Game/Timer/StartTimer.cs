using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    [SerializeField] private GameObject _destroyedObject;
    [SerializeField] private Text _text;

    public void StartTick(int tick)
    {
        Debug.Log(tick);
        _text.text = tick.ToString();
    }

    public void Destroy()
    {
        Destroy(_destroyedObject);
    }
}
