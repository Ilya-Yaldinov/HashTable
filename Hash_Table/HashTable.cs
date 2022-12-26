using System.Security.Cryptography;
using System.Text;

namespace Hash_Table;

public class HashTable
{
    private readonly int maxTableSize;
    private readonly int maxSize;
    private Dictionary<int, List<Node>> nodes = null;

    public IReadOnlyCollection<KeyValuePair<int, List<Node>>> Nodes => nodes?.ToList()?.AsReadOnly();
    public int MaxLengthChain => nodes.Where(x => x.Value != null).Max(x => x.Value.Count);
    public int MinLengthChain => nodes.Where(x => x.Value != null).Min(x => x.Value.Count);
    public int Count { get; private set; }
    public double FillFactor => (double)Count/maxTableSize;



    public HashTable(int maxTableSize = 1000, int maxSize = 100000)
    {
        this.maxTableSize = maxTableSize;
        nodes = new Dictionary<int, List<Node>>(maxTableSize);
        this.maxSize = maxSize;
        Count = 0;
    }

    public void Insert(int key, string value)
    {
        if (Count > maxSize)
        {
            Console.WriteLine($"Максимальная длинна таблицы составляет {maxTableSize} символов.");
        }
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        var item = new Node(key, value);
        var hash = GetHashByDiv(item.Key);
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
        if (Count > maxSize)
        {
            Console.WriteLine($"Максимальная длинна таблицы составляет {maxTableSize} символов.");
        }

        var hash = GetHashByDiv(key);
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
        Count--;
    }
    public string Search(int key)
    {
        if (Count > maxSize)
        {
            Console.WriteLine($"Максимальная длинна таблицы составляет {maxTableSize} символов.");
        }

        var hash = GetHashByDiv(key);

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

    private int GetHashByDiv(int key)
    {
        return Math.Abs(key.GetHashCode() % maxTableSize);
    }

    /*private int GetHashByMD5(int key)
    {
        var md5Hash = MD5.Create();
        var sourceBytes = Encoding.UTF8.GetBytes(key.ToString());
        var hashBytes = md5Hash.ComputeHash(sourceBytes);
        var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        return hash;
    }*/
}
