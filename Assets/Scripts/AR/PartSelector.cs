using UnityEngine;

public class PartSelector : MonoBehaviour
{
    [Header("References")]
    public UIManager uiManager;
    public GeminiClient geminiClient;

    [Header("Organ Config")]
    public string organId = "brain";

    [Header("Highlight")]
    public Color highlightColor =
        new Color(1f, 0.8f, 0f, 1f);
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

    public void Deselect()
    {
        if (partRenderer != null)
            partRenderer.material.color =
                originalColor;
    }

    void RequestExplanation()
    {
        string meshName = gameObject.name;
        string displayName = meshName;
        string shortFact = "";

        if (OrganDatabaseManager.Instance != null)
        {
            displayName = OrganDatabaseManager
                .Instance
                .GetDisplayName(organId, meshName);
            shortFact = OrganDatabaseManager
                .Instance
                .GetShortFact(organId, meshName);
        }

        uiManager.ShowPanel(
            displayName, shortFact, "", true);

        StartCoroutine(
            geminiClient.GetExplanation(
                displayName,
                desc => uiManager
                    .UpdateDescription(desc),
                err => uiManager
                    .UpdateDescription(err)
            )
        );
    }
}
