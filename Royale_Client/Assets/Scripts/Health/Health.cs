using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float Max { get; private set; } = 10f;
    private float _current;
    private HealthUI _healthUI;

    private void Start()
    {
        _current = Max;
        _healthUI = GetComponentInChildren<HealthUI>();

        _healthUI.UpdateHealth(Max, _current);
    }

    public void ApplyDamage(float value, GameObject damageable)
    {
        _current -= value;
        if (_current <= 0)
        {
            _current = 0;
            if (damageable.TryGetComponent<Tower>(out Tower tower)) tower.Destroy();
            else if (damageable.TryGetComponent<Unit>(out Unit unit)) unit.Destroy();
        }

        _healthUI.UpdateHealth(Max, _current);
    }
}

interface IHealth
{
    Health Health { get; }
}