using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position = Player.Instance.transform.position;
    }
}
