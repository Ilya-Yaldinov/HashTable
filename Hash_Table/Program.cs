using System.Collections;
using System.Security.Cryptography;

namespace Hash_Table;
class Program
{
    public static void Main()
    {
        HashTableOpenAdressing hash = new();
        hash.GeneratingValuesAndKeys();
        hash.ShowHashTable();

        //StreamWriter sw = new StreamWriter("mult.csv");
        //sw.WriteLine($"Fill Factor; {hash.FillFactor}");
        //sw.WriteLine($"Max Chain; {hash.MaxLengthChain}");
        //sw.WriteLine($"Min Chain; {hash.MinLengthChain}");
        //sw.WriteLine();
        //foreach(var node in hash.Nodes)
        //{
        //    sw.WriteLine($"{node.Value.Count}");
        //}

        //sw.Close();
    }
}
