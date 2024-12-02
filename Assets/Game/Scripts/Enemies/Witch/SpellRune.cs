using System.Collections;
using UnityEngine;

public class SpellRune : MonoBehaviour
{
    [SerializeField, Min(0.1f)]
    private float radius = 1;

    [HideInInspector]
    public GameObject bullet;
    [HideInInspector]
    public float shootDelay = 1;
    [HideInInspector]
    public float spellSpeed = 1;
    [HideInInspector]
    public float spellLifeTime = 1;
    [HideInInspector]
    public int damage = 1;
    [SerializeField, Min(1)]
    private float countPerTime = 1;

    public void Activate()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if(bullet != null)
            {
                for (int i = 0; i < countPerTime; i++)
                {
                    LaunchSpell(GetRandomSpawnPoint(), transform.forward);
                }
                yield return new WaitForSeconds(shootDelay);
            }
            else
            {
                yield return null;
            }
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float x = Random.Range(-radius, radius);
        float y = Random.Range(-radius, radius);
        return transform.position + transform.right * x + transform.up * y;
    }

    private void LaunchSpell(Vector3 spawnPoint, Vector3 direction)
    {
        MagicBullet currentBullet = Instantiate(bullet, spawnPoint, Quaternion.identity).
            GetComponent<MagicBullet>();
        currentBullet.transform.parent = null;
        currentBullet.transform.forward = direction;
        currentBullet.LaunchBullet(spellSpeed, spellLifeTime, damage, true);
    }
}
