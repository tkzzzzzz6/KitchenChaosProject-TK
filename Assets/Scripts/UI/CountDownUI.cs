using System;
using TMPro;
using UnityEngine;


public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }
    private void Update()
    {
        if (GameManager.Instance.IsCountDownState())
        {
            numberText.text = Mathf.CeilToInt(GameManager.Instance.GetCountDownToStartTimer()).ToString();
        }
    }
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountDownState())
        {
            numberText.gameObject.SetActive(true);
        }
        else
        {
            numberText.gameObject.SetActive(false);
        }
    }
}
