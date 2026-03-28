using UnityEngine;

public class PartSelector : MonoBehaviour
{
    public InfoPanel infoPanel;
    public GeminiClient geminiClient;
    public string customPartName = "";

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

    public void HandleTap()
    {
        if (currentlySelected != null
            && currentlySelected != this)
            currentlySelected.Deselect();

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
                desc => infoPanel
                    .UpdateDescription(desc),
                err => infoPanel
                    .UpdateDescription(err)
            )
        );
    }
}