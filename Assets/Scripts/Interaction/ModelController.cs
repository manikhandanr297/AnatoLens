using UnityEngine;

public class ModelController : MonoBehaviour
{
    public float rotationSpeed = 0.25f;
    public float zoomSpeed = 0.002f;
    public float minScale = 0.001f;
    public float maxScale = 0.05f;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                transform.Rotate(Vector3.up,
                    -t.deltaPosition.x *
                    rotationSpeed, Space.World);
                transform.Rotate(Vector3.right,
                    t.deltaPosition.y *
                    rotationSpeed, Space.World);
            }
        }
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);
            float prev = Vector2.Distance(
                t0.position - t0.deltaPosition,
                t1.position - t1.deltaPosition);
            float curr = Vector2.Distance(
                t0.position, t1.position);
            float s = transform.localScale.x +
                (curr - prev) * zoomSpeed;
            s = Mathf.Clamp(s, minScale, maxScale);
            transform.localScale = Vector3.one * s;
        }
    }
}