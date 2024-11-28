using System.Collections;

public class TestEnemy : Enemy
{
    protected override IEnumerator MainCoroutine()
    {
        yield return null;
    }

    public override void GetDamage(int damage)
    {

    }
}
