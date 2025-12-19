using System.ComponentModel;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private ContainerCounterVisual containerCounterVisual;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) return;

        CreateKitchenObject(kitchenObjectSO.prefab);
        TransferKitchenObject(this, player);

        containerCounterVisual.PlayOpen();
    }
    
}
