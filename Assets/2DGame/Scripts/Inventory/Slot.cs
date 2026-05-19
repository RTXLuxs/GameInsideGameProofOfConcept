using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject currentItem;

    private static readonly Color HighlightColor = new Color(0.6f, 1f, 0.65f);
    private static readonly Color DefaultColor = new Color(0xAC / 255f, 0x73 / 255f, 0x58 / 255f);

    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void SetHighlight(bool highlighted)
    {
        if (background != null)
            background.color = highlighted ? HighlightColor : DefaultColor;
    }
}
