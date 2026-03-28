using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    public ModelController modelController;
    private bool isTwoFingerGesture = false;

    void Update()
    {
        if (Input.touchCount == 0)
        {
            isTwoFingerGesture = false;
            return;
        }
        if (Input.touchCount >= 2)
        {
            isTwoFingerGesture = true;
            return;
        }
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began
                && !isTwoFingerGesture)
            {
                TrySelectPart(touch.position);
            }
        }
    }

    void TrySelectPart(Vector2 screenPos)
    {
        Ray ray = Camera.main
            .ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            PartSelector part =
                hit.collider
                .GetComponent<PartSelector>();
            if (part != null)
                part.HandleTap();
        }
    }
}