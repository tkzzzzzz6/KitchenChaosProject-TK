using UnityEngine;
using System.Collections.Generic;
public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    [SerializeField] private PlateCompleteVisual plateCompleteVisual;

    [SerializeField] private KitchenObjectGridUI kitchenObjectGridUI;
    private List<KitchenObjectSO> kitchenObjectSOList = new List<KitchenObjectSO>();


    public bool AddKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        plateCompleteVisual.ShowKitchenObject(kitchenObjectSO);
        kitchenObjectGridUI.ShowKitchenObjectUI(kitchenObjectSO);
        kitchenObjectSOList.Add(kitchenObjectSO);
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}

