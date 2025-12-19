using UnityEngine;

public class BaseCounter : KitchenObjectHolder
{

    [SerializeField] private GameObject selectedCounter;
    

    public virtual void Interact(Player player)
    {
        Debug.LogWarning("Interact Method is not overridden!");
    }

    public virtual void InteractOperate(Player player)
    {
        
    }

    public void SelectCounter()
    {
        selectedCounter.SetActive(true);
    }
    public void CancelSelect()
    {
        selectedCounter.SetActive(false);
    }
}
