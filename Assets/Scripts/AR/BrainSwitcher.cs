using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BrainSwitcher : MonoBehaviour
{
    [Header("Models")]
    public GameObject brainSurface;
    public GameObject brainCrossSection;

    [Header("UI")]
    public TMP_Text switchButtonText;

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
        brainSurface.SetActive(true);
        brainCrossSection.SetActive(false);
        if (switchButtonText != null)
            switchButtonText.text = "Cross Section";
    }

    void ShowCrossSection()
    {
        showingSurface = false;
        brainSurface.SetActive(false);
        brainCrossSection.SetActive(true);
        if (switchButtonText != null)
            switchButtonText.text = "Surface View";
    }
}