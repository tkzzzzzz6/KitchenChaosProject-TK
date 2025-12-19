using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject uiParent;
    [SerializeField] private TextMeshProUGUI upKeyText;
    [SerializeField] private TextMeshProUGUI downKeyText;
    [SerializeField] private TextMeshProUGUI rightKeyText;
    [SerializeField] private TextMeshProUGUI leftKeyText;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI operateKeyText;
    [SerializeField] private TextMeshProUGUI pauseKeyText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += Gamemanager_OnStateChanged;
        Show();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= Gamemanager_OnStateChanged;
        }
    }

    private void Gamemanager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsWaitingToStartState())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        UpdateVisual();
        uiParent.SetActive(true);
    }
    private void Hide()
    {
        uiParent.SetActive(false);
    }
    private void UpdateVisual()
    {
        upKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Up);
        downKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Down);
        leftKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Left);
        rightKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Right);
        interactKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Interact);
        operateKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Operate);
        pauseKeyText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Pause);
    }
}
