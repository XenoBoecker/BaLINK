
namespace Custom
{
    [System.Serializable]
    public class ToggleableField<T>
    {
        public T Value;
        public bool IsActive;

        public ToggleableField(T value)
        {
            Value = value;
            IsActive = false;
        }
    }
}

