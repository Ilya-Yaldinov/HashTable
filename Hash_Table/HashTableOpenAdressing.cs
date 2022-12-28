using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hash_Table
{
    //This implementation uses a simple hash function that calculates the remainder of the key 
    //divided by the size of the hash table. When a collision occurs, it uses open addressing to 
    //find the next available slot in the table by incrementing the index and wrapping around to 
    //the beginning of the table if necessary.
    //You can use the Put method to add a key-value pair to the hash table, and the Get 
    //method to retrieve the value for a given key.

    public class HashTableOpenAdressing
    {
        private readonly int maxSize;
        private int count;
        private float c1, c2 = 0.25f;
        private Node[] nodes;
        private HashFunc hashFunc;
        private Dictionary<int, int> hashDictionary = new Dictionary<int, int>();

        public int MaxClusterLength() => hashDictionary.Values.Max();
        public IReadOnlyCollection<Node> Nodes => nodes?.ToList()?.AsReadOnly();

        public HashTableOpenAdressing(int size = 200)
        {
            this.maxSize = size;
            nodes = new Node[maxSize];
            hashFunc = new HashFunc(maxSize);
        }

        //Функция двойного хеширования
        public int DoubleHashing(int value, int i)
        {
            return (int)Math.Abs((value + i * value) % maxSize);
        }

        //Функция линейного хеширования 
        public int LinearHashing(int key, int i)
        {
            return Math.Abs((hashFunc.GetHashByMult(key) + i) % maxSize);
        }

        //Функция квадратичного хеширования
        public int QuadraticHashing(int key, int i)
        {
            double stepHash = (hashFunc.GetHashByDiv(key) * c1 * i + c2 * i * i) % maxSize;
            return Math.Abs((int)stepHash);
        }

        /*public int MaxClusterLength ()
        {
            var max = 0;
            var current = 0;
                foreach (var item in nodes)
                {
                    if (!item.Equals(default(KeyValuePair<int, string>)))
                    {
                        current++;
                    }
                    else
                    {
                        max = Math.Max(max, current);
                        current = 0;
                    }
                }

                return Math.Max(max, current);
        }*/


        public void Insert(int key, string value)
        {
            if (count > maxSize)
            {
                Console.WriteLine($"Максимальная длинна таблицы составляет {maxSize} символов.");
                return;
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var i = 0;
            int hash = LinearHashing(key, i);

            while (nodes[hash] != null) //проверяем ячейки, до тех пор, пока не находим пустую ячейку
            {
                i++;
                hash = LinearHashing(key, i);
            }
            nodes[hash] = new Node(key, value);
            hashDictionary.Add(hash, i);
        }

        public void Delete(int key)
        {
            int i = 0;
            int hash = LinearHashing(key, i);

            while (nodes[hash] != null)
            {
                if (nodes[hash].Key == key)
                {
                    nodes[hash] = null;
                    break;
                }
                i++;
                hash = LinearHashing(key, i);
            }
        }

        public string Search(int key)
        {
            int i = 0;
            int hash = LinearHashing(key, i);

            while (i < maxSize) //проверяем все ячейки 
            {
                if (nodes[hash].Key == key)
                {
                    return nodes[hash].Value;
                }
                i++;
                hash = LinearHashing(key, i);
            }

            return null;
        }

        public void ShowHashTable()
        {
            foreach (var item in nodes)
            {
                if (item != null)
                    Console.WriteLine($"{item.Key} - {item.Value}");
            }
            Console.WriteLine();
        }

        public void GeneratingValuesAndKeys(int sizeHash = 200)
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

            for (int i = 0; i < sizeHash; i++)
            {
                Insert(keys[i], values[i]);
            }
        }
    }

}
