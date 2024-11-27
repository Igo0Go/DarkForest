using UnityEngine;

public class Decal : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField]
    [Tooltip("0 - не будет удаляться")]
    private float lifeTime = 1;

    private void Start()
    {
        if(lifeTime > 0)
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
