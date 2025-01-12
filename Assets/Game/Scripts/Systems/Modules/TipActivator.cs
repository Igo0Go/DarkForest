using UnityEngine;

public class TipActivator : MonoBehaviour
{
    [SerializeField]
    private TipItem item;

    public void ActivateTip()
    {
        FindObjectOfType<TipPanel>().ShowTip(item);
        Destroy(gameObject);
    }
}
