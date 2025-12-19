using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateSO;
    [SerializeField] private float spawnRate = 3f;
    [SerializeField] private int plateCountMax = 5;

    private List<KitchenObject> plateList = new List<KitchenObject>();
    private float timer = 0f;

    public void Update()
    {
        if (plateCountMax > plateList.Count)
        {
            timer += Time.deltaTime;
        }
        if (timer >= spawnRate)
        {
            timer = 0f;
            SpawnPlate();
        }
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() == false)
        {//player does not have kitchen object
            if(plateList.Count > 0)
            {
                player.AddKitchenObject(plateList[plateList.Count - 1]);
                plateList.RemoveAt(plateList.Count - 1);
            }
        }
    }

    public void SpawnPlate()
    {
        if (plateCountMax <= plateList.Count)
        {
            timer = 0f;
            return;
        }
        KitchenObject kitchenObject = GameObject.Instantiate(plateSO.prefab, GetHoldPoint()).GetComponent<KitchenObject>();

        kitchenObject.transform.localPosition = Vector3.zero + Vector3.up * plateList.Count * 0.05f;

        plateList.Add(kitchenObject);
    }


}
