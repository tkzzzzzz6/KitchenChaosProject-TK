using System;
using NUnit.Framework;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private CuttingRecipeListSO cuttingRecipeList;
    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private CuttingCounterVisual cuttingCounterVisual;

    public static event EventHandler OnCut;
    private int cuttingCount = 0;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {//player has kitchen object
            if (!HasKitchenObject())
            {// counter is empty
                TransferKitchenObject(player, this);
                cuttingCount = 0;
            }
            else
            {// counter has kitchen object

            }
        }
        else
        {//player does not have kitchen object
            if (!HasKitchenObject())
            {// counter is empty

            }
            else
            {// counter has kitchen object
                TransferKitchenObject(this, player);
                progressBarUI.Hide();
            }

        }
    }

    public override void InteractOperate(Player player)
    {
        if (HasKitchenObject())
        {
            if (cuttingRecipeList.TryGetCuttingRecipe(GetKitchenObject().GetKitchenObjectSO(), out CuttingRecipe cuttingRecipe))
            {
                Cut();
                progressBarUI.UpdateProgress((float)cuttingCount / cuttingRecipe.cuttingCountMax);

                if (cuttingCount == cuttingRecipe.cuttingCountMax)
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(cuttingRecipe.output.prefab);
                }
            }

        }
    }

    public void Cut()
    {
        OnCut?.Invoke(this, EventArgs.Empty);
        ++cuttingCount;
        cuttingCounterVisual.PlayCut();
    }
    public static void ClearStaticDate()
    {
        OnCut = null;
    }
}
