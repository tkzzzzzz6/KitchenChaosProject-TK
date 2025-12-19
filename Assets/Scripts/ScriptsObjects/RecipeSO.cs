using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> kitchenObjectSOList;

    public List<KitchenObjectSO> KitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }

}
