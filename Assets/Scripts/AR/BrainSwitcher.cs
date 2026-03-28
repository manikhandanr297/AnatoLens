using UnityEngine;
using TMPro;

public class BrainSwitcher : MonoBehaviour
{
    [Header("Models")]
    public GameObject brainSurface;
    public GameObject brainCrossSection;

    [Header("UI")]
    public TMP_Text switchButtonText;

    [Header("Labels")]
    public string surfaceLabel = "Cross Section";
    public string crossSectionLabel = "Surface View";

    private bool showingSurface = true;

    void Start()
    {
        ShowSurface();
    }

    public void ToggleModel()
    {
        if (showingSurface)
            ShowCrossSection();
        else
            ShowSurface();
    }

    void ShowSurface()
    {
        showingSurface = true;
        if (brainSurface != null)
            brainSurface.SetActive(true);
        if (brainCrossSection != null)
            brainCrossSection.SetActive(false);
        if (switchButtonText != null)
            switchButtonText.text = surfaceLabel;
    }

    void ShowCrossSection()
    {
        showingSurface = false;
        if (brainSurface != null)
            brainSurface.SetActive(false);
        if (brainCrossSection != null)
            brainCrossSection.SetActive(true);
        if (switchButtonText != null)
            switchButtonText.text =
                crossSectionLabel;
    }
}