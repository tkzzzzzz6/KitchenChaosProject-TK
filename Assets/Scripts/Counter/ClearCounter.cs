using UnityEngine;

public class ClearCounter : BaseCounter
{
    // [SerializeField] private ClearCounter transferTargetCounter;
    // [SerializeField] private bool testing = false;


    // private void Update()
    // {
    //     if(testing && Input.GetMouseButtonDown(0))
    //     {
    //         TransferKitchenObject(this,transferTargetCounter);
    //     }
    // }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {//player has kitchen Object

            if (player.GetKitchenObject().TryGetComponent<PlateKitchenObject>(out PlateKitchenObject plateKitchenObject))
            {// is the kitchen object is plate

                if (!HasKitchenObject())
                {// counter is empty,to transfer plate to counter
                    TransferKitchenObject(player, this);
                }
                else
                {// counter has kitchen object,to add kitchen object to plate
                    bool isSuccess = plateKitchenObject.AddKitchenObjectSO(GetKitchenObjectSO());
                    if (isSuccess)
                    {
                        DestroyKitchenObject();
                    }
                }
            }
            else
            { // the kitchen object in player hand is not plate
                if (!HasKitchenObject())
                {// counter is empty
                    TransferKitchenObject(player, this);
                }
                else
                {// counter has kitchen object
                    if (GetKitchenObject().TryGetComponent<PlateKitchenObject>(out plateKitchenObject))
                    {
                        if(plateKitchenObject.AddKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.DestroyKitchenObject();
                        }
                    }
                }
            }
        }
        else
        {//player does not have kitchen object
            if (!HasKitchenObject())
            {// the counter is empty

            }
            else
            {// the counter has kitchen object
                TransferKitchenObject(this, player);
            }
        }
    }
}
