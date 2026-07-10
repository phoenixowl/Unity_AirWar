public readonly struct GameOverEvent
{
    public readonly bool won;

    public GameOverEvent(bool won)
    {
        this.won = won;
    }
}