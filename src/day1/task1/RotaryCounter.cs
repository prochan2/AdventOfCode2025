internal class RotaryCounter
{
    public long Count { get; private set; }

    public RotaryCounter(long count)
    {
        Count = count;
    }

    public void Increment(long value)
    {
        var newCount = Count + (value % 100);
        
        Count = newCount switch
        {
            > 99 => newCount - 100,
            < 0 => newCount + 100,
            _ => newCount
        };
    }
}
