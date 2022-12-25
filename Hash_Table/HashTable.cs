namespace Hash_Table;

public class HashTable
{
    private readonly int maxSize;

    private Dictionary<int, List<Node>> nodes = null;
    public IReadOnlyCollection<KeyValuePair<int, List<Node>>> Nodes => nodes?.ToList()?.AsReadOnly();

    public HashTable(int maxSize)
    {
        this.maxSize = maxSize;
        nodes = new Dictionary<int, List<Node>>(maxSize);
    }

    public void Insert(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (key.Length > maxSize)
        {
            throw new ArgumentException($"Максимальная длинна ключа составляет {maxSize} символов.", nameof(key));
        }
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        var item = new Node(key, value);
        var hash = GetHash(item.Key);
        List<Node> hashTableItem = null;

        if (nodes.ContainsKey(hash))
        {
            hashTableItem = nodes[hash];
            var oldElementWithKey = hashTableItem.SingleOrDefault(i => i.Key == item.Key);
            if (oldElementWithKey != null)
            {
                throw new ArgumentException($"Хеш-таблица уже содержит элемент с ключом {key}. Ключ должен быть уникален.", nameof(key));
            }

            nodes[hash].Add(item);
        }
        else
        {
            hashTableItem = new List<Node> { item };
            nodes.Add(hash, hashTableItem);
        }
    }

    public void Delete(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (key.Length > maxSize)
        {
            throw new ArgumentException($"Максимальная длинна ключа составляет {maxSize} символов.", nameof(key));
        }

        var hash = GetHash(key);
        if (!nodes.ContainsKey(hash))
        {
            return;
        }

        var hashTableItem = nodes[hash];
        var item = hashTableItem.SingleOrDefault(i => i.Key == key);

        if (item != null)
        {
            hashTableItem.Remove(item);
        }
    }
    public string Search(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (key.Length > maxSize)
        {
            throw new ArgumentException($"Максимальная длинна ключа составляет {maxSize} символов.", nameof(key));
        }

        var hash = GetHash(key);

        if (!nodes.ContainsKey(hash))
        {
            return null;
        }

        var hashTableItem = nodes[hash];

        if (hashTableItem != null)
        {
            var item = hashTableItem.SingleOrDefault(i => i.Key == key);

            if (item != null)
            {
                return item.Value;
            }
        }

        return null;
    }
    private int GetHash(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (value.Length > maxSize)
        {
            throw new ArgumentException($"Максимальная длинна ключа составляет {maxSize} символов.", nameof(value));
        }

        var hash = value.Length;
        return hash;
    }

    public void ShowHashTable()
    {
        foreach (var item in nodes)
        {
            Console.WriteLine(item.Key);
            foreach (var value in item.Value)
            {
                Console.WriteLine($"\t{value.Key} - {value.Value}");
            }
        }
        Console.WriteLine();
    }
}
