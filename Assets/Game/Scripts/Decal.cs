using UnityEngine;

public class Decal : MonoBehaviour
{
    [Range(0,60)]
    [Tooltip("0 - не будет удаляться")]
    public float lifeTime = 1;

    private void Start()
    {
        if(lifeTime > 0)
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
