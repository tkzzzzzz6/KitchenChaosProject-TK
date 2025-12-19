
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressImage;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateProgress(float progress)
    {
        Show();
        progressImage.fillAmount = progress;

        if (progress == 1f)
        {
            Invoke("Hide", 0.5f);
        }
    }
}
