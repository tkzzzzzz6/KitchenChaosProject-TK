using System.ComponentModel;
using UnityEngine;

public class StoveCounter : BaseCounter
{

    public enum StoveState
    {
        Idle,
        Frying,
        Burning,
    }

    [SerializeField] private FryingRecipeListSO fryingRecipeList;
    [SerializeField] private FryingRecipeListSO burningRecipeList;
    [SerializeField] private StoveCounterVisual stoveCounterVisual;
    [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private AudioSource sound;
    private FryingRecipe fryingRecipe;
    private float fryingTime;
    private StoveState stoveState = StoveState.Idle;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {//player has kitchen object
            if (!HasKitchenObject())
            {// counter is empty
                if (fryingRecipeList.TryGetFryingRecipe(
                player.GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe fryingRecipe))
                {
                    TransferKitchenObject(player, this);
                    StartFrying(fryingRecipe);
                }
                else if (burningRecipeList.TryGetFryingRecipe(
                player.GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe burningRecipe))
                {
                    TransferKitchenObject(player, this);
                    StartBurning(burningRecipe);
                }
                else
                {

                }
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
                TurnToIdle();
                TransferKitchenObject(this, player);
            }
        }
    }

    public void Update()
    {
        switch (stoveState)
        {
            case StoveState.Idle:
                break;
            case StoveState.Frying:
                fryingTime += Time.deltaTime;
                progressBarUI.UpdateProgress(fryingTime / fryingRecipe.fryingTime);
                if (fryingTime >= fryingRecipe.fryingTime)
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(fryingRecipe.output.prefab);

                    burningRecipeList.TryGetFryingRecipe(
                        GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe newFryingRecipe
                    );
                    StartBurning(newFryingRecipe);
                }
                break;
            case StoveState.Burning:
                fryingTime += Time.deltaTime;
                progressBarUI.UpdateProgress(fryingTime / fryingRecipe.fryingTime);
                if (fryingTime >= fryingRecipe.fryingTime)
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(fryingRecipe.output.prefab);
                    TurnToIdle();
                }
                break;
            default:
                break;

        }


    }

    public void StartFrying(FryingRecipe fryingRecipe)
    {
        fryingTime = 0f;
        this.fryingRecipe = fryingRecipe;
        stoveState = StoveState.Frying;
        stoveCounterVisual.ShowStoveEffect();
        sound.Play();
    }

    public void StartBurning(FryingRecipe fryingRecipe)
    {
        if (fryingRecipe == null)
        {
            Debug.LogWarning("No burning recipe for that we can't process burning!");
            TurnToIdle();
            return;
        }
        stoveCounterVisual.ShowStoveEffect();
        fryingTime = 0f;
        this.fryingRecipe = fryingRecipe;
        stoveState = StoveState.Burning;
        sound.Play();
    }

    public void TurnToIdle()
    {
        progressBarUI.Hide();
        stoveState = StoveState.Idle;
        stoveCounterVisual.HideStoveEffect();
        sound.Pause();
    }
}
