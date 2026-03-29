using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TrackingManager : MonoBehaviour
{
    [Header("Show when tracking")]
    public GameObject switchButton;
    public GameObject chatButton;
    public ObserverBehaviour imageTarget;

    void Start()
    {
        SetVisible(false);
        if (imageTarget != null)
            imageTarget.OnTargetStatusChanged
                += OnStatusChanged;
    }

    void OnDestroy()
    {
        if (imageTarget != null)
            imageTarget.OnTargetStatusChanged
                -= OnStatusChanged;
    }

    void OnStatusChanged(
        ObserverBehaviour b,
        TargetStatus status)
    {
        bool tracked =
            status.Status == Status.TRACKED ||
            status.Status ==
                Status.EXTENDED_TRACKED;
        SetVisible(tracked);
    }

    void SetVisible(bool v)
    {
        switchButton?.SetActive(v);
        chatButton?.SetActive(v);
    }
}