using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObjectSO : KitchenObjectSO
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
}
