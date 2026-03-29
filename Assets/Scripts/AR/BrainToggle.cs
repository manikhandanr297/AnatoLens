using UnityEngine;
using TMPro;

public class BrainToggle : MonoBehaviour
{
    public GameObject brainSurface;
    public GameObject brainCrossSection;
    public TMP_Text buttonText;
    private bool showSurface = true;

    void Start()
    {
        brainSurface?.SetActive(true);
        brainCrossSection?.SetActive(false);
        UpdateLabel();
    }

    public void Toggle()
    {
        showSurface = !showSurface;
        brainSurface?.SetActive(showSurface);
        brainCrossSection?
            .SetActive(!showSurface);
        UpdateLabel();
    }

    void UpdateLabel()
    {
        if (buttonText == null) return;
        buttonText.text = showSurface
            ? "Cross Section"
            : "Surface View";
    }
}