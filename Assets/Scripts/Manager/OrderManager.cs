using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance {get;private set;}
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    [SerializeField] private RecipeListSO recipeSOList;
    [SerializeField] private int maxOrderCount = 5;
    [SerializeField] private float orderRate = 2f;
    
    private List<RecipeSO>  orderRecipeSOList = new List<RecipeSO>();
    private float orderTimer = 0f;

    private bool isStartOrder = false;
    private int orderCount = 0;
    private int successDeliveryCount = 0;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }
    private void GameManager_OnStateChanged(object sender,EventArgs e)
    {
        if(GameManager.Instance.IsGamePlayingState())
        {
            StartSwanOrder();
        }
    }
    private void Update()
    {
        if(isStartOrder)
        {
            OrderUpdate();    
        }
    }
    private void OrderUpdate()
    {
        orderTimer += Time.deltaTime;
        if(orderTimer >= orderRate)
        {
            orderTimer = 0f;
            OrderANewRecipe();
        }
    }
    private void OrderANewRecipe()
    {
        if(orderCount >= maxOrderCount)return;
        ++orderCount;
        int index = UnityEngine.Random.Range(0,recipeSOList.recipeSOList.Count);
        orderRecipeSOList.Add(recipeSOList.recipeSOList[index]);
        OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
    }
    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        RecipeSO matchedRecipe = null;
        foreach(RecipeSO recipe in orderRecipeSOList)
        {
            if(IsCorrect(recipe,plateKitchenObject))
            {
                matchedRecipe = recipe;
                break;
            }
        }
        if(matchedRecipe == null)
        {
            OnRecipeFailed?.Invoke(this,EventArgs.Empty);
            Debug.Log("No matched recipe!");
            return;
        }
        else
        {
            orderRecipeSOList.Remove(matchedRecipe);
            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            ++successDeliveryCount;
            Debug.Log("Recipe delivered!");
        }
    }
    private bool IsCorrect(RecipeSO recipe,PlateKitchenObject plateKitchenObject)
    {
        List<KitchenObjectSO> list1 = recipe.KitchenObjectSOList();
        List<KitchenObjectSO> list2 = plateKitchenObject.GetKitchenObjectSOList();

        if(list1.Count != list2.Count)return false;
        foreach(KitchenObjectSO kitchenObjectSO in list1)
        {
            if(list2.Contains(kitchenObjectSO) == false)return false;
        }
        return true;
    }

    public List<RecipeSO> GetOrderList()
    {
        return orderRecipeSOList;
    }

    public void StartSwanOrder()
    {
        isStartOrder = true;
    }

    public int GetSuccessDeliveryCount()
    {
        return successDeliveryCount;
    }

}
