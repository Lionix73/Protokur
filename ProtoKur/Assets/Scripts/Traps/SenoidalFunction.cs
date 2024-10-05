using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenoidalFunction : MonoBehaviour
{
    public float frequency = 1f; // Frequency of the sine wave
    public float amplitude = 1f; // Amplitude of the sine wave
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPosition + new Vector3(0, y, 0);
    }
}