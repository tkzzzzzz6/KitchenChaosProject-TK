
using UnityEngine;
using UnityEngine.UI;

public class KitchenObjectGridUI : MonoBehaviour
{
    [SerializeField] private KitchenObjectIconUI iconTemplate;
    private void Start()
    {
        iconTemplate.Hide();
    }
    public void ShowKitchenObjectUI(KitchenObjectSO kitchenObjectSO)
    {
        KitchenObjectIconUI newIconUI = GameObject.Instantiate(iconTemplate,transform);
        newIconUI.Show(kitchenObjectSO.sprite);
    }
}
