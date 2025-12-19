using UnityEngine;

public class ClearStaticData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrashCounter.ClearStaticDate();
        CuttingCounter.ClearStaticDate();
        KitchenObjectHolder.ClearStaticDate();
    }

}
