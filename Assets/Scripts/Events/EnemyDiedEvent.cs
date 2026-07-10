public readonly struct EnemyDiedEvent
{
    public readonly int score;

    public EnemyDiedEvent(int score)
    {
        this.score = score;
    }
}