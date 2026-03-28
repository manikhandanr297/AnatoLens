using UnityEngine;

public class ModelController : MonoBehaviour
{
    public float rotationSpeed = 0.3f;
    public float zoomSpeed = 0.002f;  // was 0.01f
    public float minScale = 0.05f;
    public float maxScale = 0.3f;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(Vector3.up,
                    -touch.deltaPosition.x * rotationSpeed,
                    Space.World);
                transform.Rotate(Vector3.right,
                    touch.deltaPosition.y * rotationSpeed,
                    Space.World);
            }
        }

        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);
            float prevDist = Vector2.Distance(
                t0.position - t0.deltaPosition,
                t1.position - t1.deltaPosition);
            float currDist = Vector2.Distance(
                t0.position, t1.position);
            float delta = currDist - prevDist;
            float newScale = transform.localScale.x
                           + delta * zoomSpeed;
            newScale = Mathf.Clamp(newScale, minScale, maxScale);
            transform.localScale = Vector3.one * newScale;
        }
    }

    public void ResetTransform()
    {
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one * 0.1f;
    }
}