using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private const string SOUNDMANAGER_VOLUME = "SouundManagerVolume";
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private float volume = 5f;

    private void Awake()
    {
        Instance = this;
        LoadVolume();
    }

    private void Start()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnRecipeCompleted += OrderManager_OnRecipeCompleted;
            OrderManager.Instance.OnRecipeFailed += OrderManager_OnRecipeFailed;
        }
        CuttingCounter.OnCut += CuttingCounter_OnCut;
        KitchenObjectHolder.OnDrop += KitchenObjectHolder_OnDrop;
        KitchenObjectHolder.OnPickup += KitchenObjectHolder_Pickup;
        TrashCounter.OnObjectTrashed += TrashCounter_OnObjectTrashed;

    }


    private void OnDestroy()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnRecipeCompleted -= OrderManager_OnRecipeCompleted;
            OrderManager.Instance.OnRecipeFailed -= OrderManager_OnRecipeFailed;
        }
        CuttingCounter.OnCut -= CuttingCounter_OnCut;
        KitchenObjectHolder.OnDrop -= KitchenObjectHolder_OnDrop;
        KitchenObjectHolder.OnPickup -= KitchenObjectHolder_Pickup;
    }
    private void KitchenObjectHolder_Pickup(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup);
    }

    private void KitchenObjectHolder_OnDrop(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectDrop);
    }
    private void CuttingCounter_OnCut(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.chop);
    }

    private void OrderManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverSuccess);
    }

    private void OrderManager_OnRecipeFailed(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverFail);
    }

    private void PlaySound(AudioClip[] clips, float volumeMutiplayer = 1f)
    {
        PlaySound(clips, Camera.main.transform.position, volumeMutiplayer);
    }

    private void PlaySound(AudioClip[] clips, Vector3 position, float volumeMutipler = 1f)
    {
        if (volume == 0)
        {
            return;
        }
        int index = UnityEngine.Random.Range(0, clips.Length);
        // Apply global volume scaling so UI volume control affects all sfx
        float scaledVolume = volumeMutipler * (volume / 10.0f);
        AudioSource.PlayClipAtPoint(clips[index], position, scaledVolume);
    }
    private void TrashCounter_OnObjectTrashed(object sender, EventArgs e)
    {
        print("trashed");
        PlaySound(audioClipRefsSO.trash);
    }
    public void PlayStepSound(float volumeMutiplayer = 1f)
    {
        PlaySound(audioClipRefsSO.footstep, volumeMutiplayer * (volume / 10.0f));
    }
    public void ChangeVolume()
    {
        volume++;
        if (volume > 10)
        {
            volume = 0;
        }
        SaveVolume();
    }
    public float GetVolume()
    {
        return volume;
    }
    private void SaveVolume()
    {
        PlayerPrefs.SetInt(SOUNDMANAGER_VOLUME, (int)volume);
    }
    private void LoadVolume()
    {
        volume = PlayerPrefs.GetInt(SOUNDMANAGER_VOLUME, (int)volume);
    }
}
