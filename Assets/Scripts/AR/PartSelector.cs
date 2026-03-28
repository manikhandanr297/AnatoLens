using UnityEngine;

public class PartSelector : MonoBehaviour
{
    [Header("References")]
    public InfoPanel infoPanel;
    public GeminiClient geminiClient;

    [Header("Part Name Override")]
    [Tooltip("Leave empty to use GameObject name")]
    public string customPartName = "";

    [Header("Highlight")]
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private Renderer partRenderer;
    private static PartSelector currentlySelected;

    void Start()
    {
        partRenderer = GetComponent<Renderer>();
        if (partRenderer != null)
            originalColor =
                partRenderer.material.color;
    }

    void OnMouseDown()
    {
        HandleTap();
    }

    // Called from touch raycast
    public void HandleTap()
    {
        // Deselect previous
        if (currentlySelected != null &&
            currentlySelected != this)
        {
            currentlySelected.Deselect();
        }

        Select();
        RequestExplanation();
    }

    void Select()
    {
        currentlySelected = this;
        if (partRenderer != null)
            partRenderer.material.color =
                highlightColor;
    }

    void Deselect()
    {
        if (partRenderer != null)
            partRenderer.material.color =
                originalColor;
    }

    void RequestExplanation()
    {
        string partName = string.IsNullOrEmpty(
            customPartName)
            ? gameObject.name
            : customPartName;

        infoPanel.ShowLoading(partName);

        StartCoroutine(
            geminiClient.GetExplanation(
                partName,
                (explanation) => {
                    infoPanel.UpdateDescription(
                        explanation);
                },
                (error) => {
                    infoPanel.UpdateDescription(error);
                }
            )
        );
    }
}