using System;
using System.ComponentModel;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnObjectTrashed;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.DestroyKitchenObject();
            OnObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
    public new static void ClearStaticDate()
    {
        OnObjectTrashed = null;
    }

}
