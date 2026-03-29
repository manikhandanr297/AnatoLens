using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    public ModelController modelController;
    private bool twoFinger = false;
    private Vector2 touchStart;
    private float tapTimer;

    void Update()
    {
        if (Input.touchCount == 0)
        {
            twoFinger = false;
            return;
        }
        if (Input.touchCount >= 2)
        {
            twoFinger = true;
            return;
        }
        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
        {
            touchStart = t.position;
            tapTimer = 0f;
        }
        if (t.phase == TouchPhase.Moved ||
            t.phase == TouchPhase.Stationary)
            tapTimer += Time.deltaTime;

        if (t.phase == TouchPhase.Ended &&
            !twoFinger && tapTimer < 0.25f &&
            Vector2.Distance(
                t.position, touchStart) < 25f)
            TrySelect(t.position);
    }

    void TrySelect(Vector2 pos)
    {
        Ray ray = Camera.main
            .ScreenPointToRay(pos);
        if (Physics.Raycast(
            ray, out RaycastHit hit, 100f))
        {
            PartSelector ps = hit.collider
                .GetComponent<PartSelector>();
            if (ps != null) ps.HandleTap();
        }
    }
}