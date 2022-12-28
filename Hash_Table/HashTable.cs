using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace Hash_Table;

public class HashTable
{
    private readonly int maxTableSize;
    private readonly int maxSize;
    private Dictionary<int, List<Node>> nodes = null;
    private HashFunc hashFunc;

    public IReadOnlyCollection<KeyValuePair<int, List<Node>>> Nodes => nodes?.ToList()?.AsReadOnly();
    public int MaxLengthChain => nodes.Where(x => x.Value != null).Max(x => x.Value.Count);
    public int MinLengthChain => nodes.Where(x => x.Value != null).Min(x => x.Value.Count);
    public int Count { get; private set; }
    public double FillFactor => (double)(Count / maxTableSize);

    public HashTable(int maxTableSize = 1000, int maxSize = 100000)
    {
        this.maxTableSize = maxTableSize;
        nodes = new Dictionary<int, List<Node>>(maxTableSize);
        this.maxSize = maxSize;
        Count = 0;
        hashFunc = new HashFunc(maxTableSize);
    }

    public void Insert(int key, string value)
    {
        if (Count > maxSize)
        {
            Console.WriteLine($"Максимальная длинна таблицы составляет {maxTableSize} символов.");
            return;
        }
        if (string.IsNullOrEmpty(value))
        {
            Console.WriteLine("Введенное вами значение является null или пусто.");
            return;
        }

        var item = new Node(key, value);
        var hash = hashFunc.GetHashByMult(item.Key);
        List<Node> hashTableItem = null;

        if (nodes.ContainsKey(hash))
        {
            hashTableItem = nodes[hash];
            var oldElementWithKey = hashTableItem.SingleOrDefault(i => i.Key == item.Key);
            nodes[hash].Add(item);
        }
        else
        {
            hashTableItem = new List<Node> { item };
            nodes.Add(hash, hashTableItem);
        }
        Count++;
    }

    public void Delete(int key)
    {
        var hash = hashFunc.GetHashByMult(key);
        if (!nodes.ContainsKey(hash))
        {
            Console.WriteLine($"Введенный вами ключ \"{key}\" не существует.");
            return;
        }

        var hashTableItem = nodes[hash];
        var item = hashTableItem.SingleOrDefault(i => i.Key == key);

        if (item != null)
        {
            Console.WriteLine($"Элемент \"{item}\" удален.");
            hashTableItem.Remove(item);
        }
        else
        {
            Console.WriteLine($"Под ключем \"{key}\" элемент не сужествует.");
        }
        Count--;
    }

    public string Search(int key)
    {
        var hash = hashFunc.GetHashByMult(key);
        if (!nodes.ContainsKey(hash))
        {
            return $"Введенный вами ключ \"{key}\" не существует.";
        }

        var hashTableItem = nodes[hash];

        if (hashTableItem != null)
        {
            var item = hashTableItem.SingleOrDefault(i => i.Key == key);
            if (item != null)
            {
                return key + " - " + item.Value;
            }
        }
        return $"Под ключем \"{key}\" элемент не сужествует.";
    }

    public void ShowHashTable(int limit = 10)
    {
        var newNodes = nodes.Take(limit);
        foreach (var item in newNodes)
        {
            Console.WriteLine(item.Key);
            foreach (var value in item.Value)
            {
                Console.WriteLine($"\t{value.Key} - {value.Value}");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void GeneratingValuesAndKeys(int sizeHash = 100000)
    {
        var start = Enumerable.Range(0, sizeHash).ToArray();
        var randomKeys = new Random();
        var randomValues = new Random();

        var keys = new List<int>();
        var values = new List<string>();

        for (var i = 0; i < sizeHash; i++)
            keys.Add(start[i] + randomKeys.Next() + i);

        for (var j = 0; j < sizeHash; j++)
        {
            var s = "";
            for (var i = 0; i < 5; i++)
            {
                var a = (char)randomValues.Next(0, 255);
                s += a;
            }
            values.Add(s);
        }

        for(int i = 0; i < sizeHash; i++)
        {
            Insert(keys[i], values[i]);
        }
    }
}
