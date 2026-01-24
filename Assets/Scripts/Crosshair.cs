using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color interactColor = Color.black;

    public void SetInteract(bool canIntercat)
    {
        crosshairImage.color = canIntercat ? interactColor : normalColor;
    }

    

}
