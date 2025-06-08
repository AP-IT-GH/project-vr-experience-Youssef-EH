using UnityEngine;

public class FloatingUIElement : MonoBehaviour
{
    public float amplitude = 20f;      // How high to float (in pixels)
    public float frequency = 1f;       // Speed of floating

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = initialPos + new Vector3(0, yOffset, 0);
    }
}