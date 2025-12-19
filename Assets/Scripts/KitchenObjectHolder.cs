using System;
using System.Xml.Serialization;
using UnityEngine;

public class KitchenObjectHolder : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    public static event EventHandler OnDrop;
    public static event EventHandler OnPickup;

    private KitchenObject kitchenObject;

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObject.GetKitchenObjectSO();
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        bool hadObjectBefore = this.kitchenObject != null;
        this.kitchenObject = kitchenObject;
        // If holder gained an object (was empty before, now has one)
        if (!hadObjectBefore && this.kitchenObject != null)
        {
            if (this is Player)
            {
                OnPickup?.Invoke(this, EventArgs.Empty);
            }
            else // assume counters or other holders -> object was dropped/placed
            {
                OnDrop?.Invoke(this, EventArgs.Empty);
            }
        }

        if (this.kitchenObject != null)
        {
            kitchenObject.transform.localPosition = Vector3.zero;
        }
    }

    public Transform GetHoldPoint()
    {
        return holdPoint;
    }

    public void TransferKitchenObject(KitchenObjectHolder sourceHolder, KitchenObjectHolder targetHolder)
    {
        if (sourceHolder.GetKitchenObject() == null)
        {
            Debug.LogWarning("Source counter has no kitchen object to transfer.");
            return;
        }
        if (targetHolder.GetKitchenObject() != null)
        {
            Debug.LogWarning("Target counter already has a kitchen object.");
            return;
        }
        targetHolder.AddKitchenObject(sourceHolder.GetKitchenObject());
        sourceHolder.ClearKitchenObject();
    }

    public void AddKitchenObject(KitchenObject kitchenObject)
    {
        kitchenObject.transform.SetParent(holdPoint);
        SetKitchenObject(kitchenObject);
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }
    public void DestroyKitchenObject()
    {
        Destroy(kitchenObject.gameObject);
        ClearKitchenObject();
    }
    public void CreateKitchenObject(GameObject kitchenObjectPrefab)
    {
        KitchenObject kitchenObject = GameObject.Instantiate(kitchenObjectPrefab, GetHoldPoint()).GetComponent<KitchenObject>();
        SetKitchenObject(kitchenObject);
    }

    public static void ClearStaticDate()
    {
        OnDrop = null;
        OnPickup = null;
    }


}
