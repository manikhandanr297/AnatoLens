using UnityEngine;

public class PartSelector : MonoBehaviour
{
    [Header("References")]
    public InfoPanel infoPanel;
    public GeminiClient geminiClient;

    [Header("Organ Config")]
    public string organId = "brain";

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
        // Get the mesh name from this GameObject
        string meshName = gameObject.name;

        // Look up human-readable name from database
        string displayName = meshName;
        string shortFact = "";

        if (OrganDatabaseManager.Instance != null)
        {
            displayName =
                OrganDatabaseManager.Instance
                .GetDisplayName(organId, meshName);
            shortFact =
                OrganDatabaseManager.Instance
                .GetShortFact(organId, meshName);
        }

        // Show loading with display name
        infoPanel.ShowLoading(displayName);

        // Show short fact immediately while AI loads
        if (!string.IsNullOrEmpty(shortFact))
            infoPanel.UpdateDescription(
                shortFact + "\n\nLoading full explanation...");

        // Request AI explanation
        StartCoroutine(
            geminiClient.GetExplanation(
                displayName,
                desc => infoPanel
                    .UpdateDescription(desc),
                err => infoPanel
                    .UpdateDescription(err)
            )
        );
    }
}