using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hash_Table
{
    class HashFunc
    {
        private int maxTableSize;

        public HashFunc(int size)
        {
            maxTableSize = size;
        }

        public int GetHashByDiv(int key)
        {
            return Math.Abs(key.GetHashCode() % maxTableSize);
        }

        public int GetHashByMult(int key)
        {
            double goldenRatio = 0.618033;
            return (int)Math.Abs(maxTableSize * (key.GetHashCode() * goldenRatio % 1));
        }

        public int GetHashByMD5(int key)
        {
            MD5 md5Hash = MD5.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(key.ToString());
            byte[] hashBytes = md5Hash.ComputeHash(sourceBytes);
            int hash = BitConverter.ToInt32(hashBytes);
            return Math.Abs(hash % maxTableSize);
        }

        public int GetHashBySHA256(int key)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(key.ToString());
            byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
            int hash = BitConverter.ToInt32(hashBytes);
            return Math.Abs(hash % maxTableSize);
        }

        public int GetHashByPBKDF2(int key)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[24];
            provider.GetBytes(salt);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(key.ToString(), salt, 100000);
            return Math.Abs(pbkdf2.GetHashCode() % maxTableSize);
        }
    }
}
