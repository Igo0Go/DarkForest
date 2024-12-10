public abstract class EnemyState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void UseState();
}

public class EmptyState : EnemyState
{
    public EmptyState(Enemy enemy) : base(enemy)
    {
    }

    public override void UseState() {}
}