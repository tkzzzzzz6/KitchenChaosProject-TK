using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetComponent<PlateKitchenObject>(out PlateKitchenObject plateKitchenObject))
            {
                // judge if the meal is correct todo 
                OrderManager.Instance.DeliveryRecipe(plateKitchenObject);
                player.DestroyKitchenObject();
            }
        }
    }
}
