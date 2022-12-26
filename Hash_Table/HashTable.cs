using System.Buffers.Text;
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
            return;
        }
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        var item = new Node(key, value);
        var hash = GetHashByMult(item.Key);
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
        var hash = GetHashByMult(key);
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
        var hash = GetHashByMult(key);
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

    private int GetHashByMult(int key)
    {
        double goldenRatio = 0.618033;
        return (int)Math.Abs(maxTableSize * (key.GetHashCode() * goldenRatio % 1));
    }

    private int GetHashByMD5(int key)
    {
        MD5 md5Hash = MD5.Create();
        byte[] sourceBytes = Encoding.UTF8.GetBytes(key.ToString());
        byte[] hashBytes = md5Hash.ComputeHash(sourceBytes);
        int hash = BitConverter.ToInt32(hashBytes);
        return Math.Abs(hash % maxTableSize);
    }

    private int GetHashBySHA256(int key)
    {
        SHA256 sha256Hash = SHA256.Create();
        byte[] sourceBytes = Encoding.UTF8.GetBytes(key.ToString());
        byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
        int hash = BitConverter.ToInt32(hashBytes);
        return Math.Abs(hash % maxTableSize);
    }

    private int GetHashByPBKDF2(int key)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        byte[] salt = new byte[24];
        provider.GetBytes(salt);

        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(key.ToString(), salt, 100000);
        return Math.Abs(pbkdf2.GetHashCode() % maxTableSize);
    }
}
