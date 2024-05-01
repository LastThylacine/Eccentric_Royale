using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 5f;

    private Vector3 _targetPosition = Vector3.zero;

    public void Init(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);

        if (transform.position == _targetPosition) Destroy(gameObject);
    }
}
