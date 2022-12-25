using System.Collections;

namespace Hash_Table;
class Program
{
    public static void Main()
    {
        var hashTable = new HashTable(10000);
        hashTable.Insert("Little Prince", "I never wished you any sort of harm; but you wanted me to tame you...");
        hashTable.Insert("Fox", "And now here is my secret, a very simple secret: It is only with the heart that one can see rightly; what is essential is invisible to the eye.");
        hashTable.Insert("Rose", "Well, I must endure the presence of two or three caterpillars if I wish to become acquainted with the butterflies.");
        hashTable.Insert("King", "He did not know how the world is simplified for kings. To them, all men are subjects.");
        hashTable.Insert("Kin", "He did not know how.");

        hashTable.ShowHashTable();
        Console.ReadLine();
        
        hashTable.Delete("King");
        hashTable.ShowHashTable();
        Console.ReadLine();

        var text = hashTable.Search("Little Prince");
        Console.WriteLine(text);
        Console.ReadLine();
    }
}
