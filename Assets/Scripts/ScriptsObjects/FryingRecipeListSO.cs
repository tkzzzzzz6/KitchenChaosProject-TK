using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FryingRecipe
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTime;

}

[CreateAssetMenu()]
public class FryingRecipeListSO : ScriptableObject
{
    public List<FryingRecipe> list;

    public bool TryGetFryingRecipe(KitchenObjectSO input, out FryingRecipe fryingRecipe)
    {
        foreach (FryingRecipe recipe in list)
        {
            if (recipe.input == input)
            {
                fryingRecipe = recipe;
                return true;
            }
        }
        fryingRecipe = null;
        return false;
    }
}
