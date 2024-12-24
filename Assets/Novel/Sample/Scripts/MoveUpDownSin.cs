using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDownSin : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private float frequency;
    private Vector2 startPosition;
    private float timer;

    private void Start()
    {
        startPosition = transform.localPosition;
    }
    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.y = startPosition.y + size * Mathf.Sin(2f * Mathf.PI * frequency * timer);
        transform.localPosition = pos;
        timer += Time.deltaTime;
    }
}
