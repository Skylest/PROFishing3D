using System;

public class UniRx<T>
{
    public Action OnValueChanged;

    private T varValue;
    public T Value
    {
        get => varValue;
        set
        {
            if (!varValue.Equals(value))
            {
                varValue = value;
                OnValueChanged?.Invoke();
            }
        }
    }
    public UniRx()
    { }

    public UniRx(T startValue)
    { varValue = startValue; }
}