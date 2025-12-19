using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dotText;
    private float dotRate = 0.5f;
    private void Start()
    {
        StartCoroutine(DotAnimation());
    }
    IEnumerator DotAnimation()
    {
        dotText.text = "";
        while (true)
        {
            dotText.text += ".";
            yield return new WaitForSecondsRealtime(dotRate);
            if (dotText.text.Length > 6)
            {
                dotText.text = "";
            }
        }
    }
}
