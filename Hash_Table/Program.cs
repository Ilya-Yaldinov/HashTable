using System.Collections;
using System.Security.Cryptography;

namespace Hash_Table;
class Program
{
    public static void Main()
    {
        HashTableOpenAdressing hash = new();
        hash.GeneratingValuesAndKeys();
        Console.WriteLine(hash.MaxClusterLength());
    }
}
