//using System;
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using Neo;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

namespace RandLib
{
    [DisplayName("RandLib")]
    //[ManifestExtra("Author", "Hecate2")]
    //[ManifestExtra("Email", "developer@neo.org")]
    [ManifestExtra("Description", "github.com/Hecate2/RandLib")]
    public class RandLib : SmartContract
    {
        const byte OWNER_KEY = 0x77;
        public void _deploy(object data, bool update)
        {
            if (!update)
            {
                UInt160 sender = ((Transaction)Runtime.ScriptContainer).Sender;
                Storage.Put(Storage.CurrentContext, new byte[] { OWNER_KEY }, sender);
            }
        }
        public static void Update(ByteString nefFile, string manifest)
        {
            ExecutionEngine.Assert(Runtime.CheckWitness((UInt160)Storage.Get(Storage.CurrentReadOnlyContext, new byte[] { OWNER_KEY })), "No witness");
            ContractManagement.Update(nefFile, manifest);
        }

        public static BigInteger Uint128() => Runtime.GetRandom();  // 128-bit unsigned
        
        /// <summary>
        /// LITTLE-ENDIAN!
        /// </summary>
        /// <param name="count">How many bits you want</param>
        /// <returns></returns>
        public static ByteString Bits(int count)
        {
            ByteString result = ByteString.Empty;
            for (; count > 128; count -= 128)
                result = result.Concat((ByteString)Runtime.GetRandom());
            if (count > 0)
                result = ((ByteString)(Runtime.GetRandom() >> (128-count))).Concat(result);
            return result;
        }
        
        // [start, stop)
        public static BigInteger Range(BigInteger start, BigInteger stop) => Runtime.GetRandom() % (stop - start) + start;

        public static object Choice(object[] seq) => seq[(int)Range(0, seq.Length)];

        public static object[] Shuffle(object[] seq)
        {
            object tmp;
            int len = seq.Length;
            for (int i=0; i<len; ++i)
            {
                int index = (int)Range(i, len);
                tmp = seq[i]; seq[i] = seq[index]; seq[index] = tmp;
            }
            return seq;
        }

        public static object[] RandChoices(object[] seq, int count)
        {
            object[] result = new object[count];
            object tmp;
            int len = seq.Length;
            for (int i = 0; i < count; ++i)
            {
                int index = (int)Range(i, len);
                tmp = seq[index]; seq[index] = seq[i]; seq[i] = tmp;
                result[i] = tmp;
            }
            return result;
        }
    }
}
