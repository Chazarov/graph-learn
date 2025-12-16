using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisualTools : MonoBehaviour
{
    private Toggle toggle;

    [SerializeField] private TextMeshProUGUI targetText;
    private Color baseColor;
    [SerializeField] private Color isActiveColor;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        
        if (toggle == null)
        {
            Debug.LogWarning($"[{nameof(ToggleVisualTools)}] Toggle component not found!");
            return;
        }

        baseColor = targetText.color;

        UpdateVisual(toggle.isOn);

        toggle.onValueChanged.AddListener(UpdateVisual);
    }

    private void OnDestroy()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(UpdateVisual);
        }
    }

    private void UpdateVisual(bool isOn)
    {
        if (targetText != null)
        {
            targetText.color = isOn ? isActiveColor : baseColor;
        }
    }
}
