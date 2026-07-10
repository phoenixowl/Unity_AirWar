public readonly struct GamePauseEvent
{
    public readonly bool pause;

    public GamePauseEvent(bool pause)
    {
        this.pause = pause;
    }
}