using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    private Animator anim;
    [SerializeField] private Player player;
    private const string IS_WALKING = "isWalking";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool(IS_WALKING,player.IsWalking);
    }


}
