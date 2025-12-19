
using UnityEngine;
using UnityEngine.UI;

public class KitchenObjectIconUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void Show(Sprite iconSprite)
    {
        gameObject.SetActive(true);
        iconImage.sprite = iconSprite;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
