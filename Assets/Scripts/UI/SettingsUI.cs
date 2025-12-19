using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; }
    [SerializeField] private GameObject uiParent;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundButtonText;
    [SerializeField] private TextMeshProUGUI musicButtonText;

    [SerializeField] private Button upKeyButton;
    [SerializeField] private Button downKeyButton;
    [SerializeField] private Button leftKeyButton;
    [SerializeField] private Button rightKeyButton;
    [SerializeField] private Button interactKeyButton;
    [SerializeField] private Button operateKeyButton;
    [SerializeField] private Button pauseKeyButton;

    [SerializeField] private TextMeshProUGUI upKeyButtonText;
    [SerializeField] private TextMeshProUGUI downKeyButtonText;
    [SerializeField] private TextMeshProUGUI leftKeyButtonText;
    [SerializeField] private TextMeshProUGUI rightKeyButtonText;
    [SerializeField] private TextMeshProUGUI interactKeyButtonText;
    [SerializeField] private TextMeshProUGUI operateKeyButtonText;
    [SerializeField] private TextMeshProUGUI pauseKeyButtonText;

    [SerializeField] private GameObject reBindingHint;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
        UpdateVisual();
        soundButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        upKeyButton.onClick.AddListener(() =>{ReBinding(GameInput.BindingType.Up);});
        downKeyButton.onClick.AddListener(() =>{ReBinding(GameInput.BindingType.Down);});
        leftKeyButton.onClick.AddListener(() =>{ReBinding(GameInput.BindingType.Left);});
        rightKeyButton.onClick.AddListener(() =>{ReBinding(GameInput.BindingType.Right);});
        interactKeyButton.onClick.AddListener(() =>{ReBinding(GameInput.BindingType.Interact);});
        operateKeyButton.onClick.AddListener(() => {ReBinding(GameInput.BindingType.Operate);});
        pauseKeyButton.onClick.AddListener(() =>{ReBinding(GameInput.BindingType.Pause);});
    }
    private void ReBinding(GameInput.BindingType bindingType)
    {
        reBindingHint.SetActive(true);
        GameInput.Instance.ReBanding(bindingType, () =>
        {
            reBindingHint.SetActive(false);
            UpdateVisual();
        });
    }

    public void Show()
    {
        uiParent.SetActive(true);
    }
    private void Hide()
    {
        uiParent.SetActive(false);
    }
    private void UpdateVisual()
    {
        soundButtonText.text = "音效大小:" + Mathf.CeilToInt(SoundManager.Instance.GetVolume());
        musicButtonText.text = "音乐大小:" + Mathf.CeilToInt(MusicManager.Instance.GetVolume());

        upKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Up);
        downKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Down);
        leftKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Left);
        rightKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Right);
        interactKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Interact);
        operateKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Operate);
        pauseKeyButtonText.text = GameInput.Instance.GetBindingDisPlayerString(GameInput.BindingType.Pause);
    }
}
