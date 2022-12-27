namespace Hash_Table;

public class Node
{
    public int Key { get; private set; }
    public string Value { get; private set; }

    public Node(int key, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        Key = key;
        Value = value;
    }

    public void SetKey(int key) => Key = key;

    public void SetValue(string value) => Value = value;

    public override string ToString()
    {
        return Key.ToString();
    }
}
