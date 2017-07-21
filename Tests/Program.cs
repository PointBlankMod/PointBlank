using System;

namespace Tests
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter something to create a digest of:");
            String Input = Console.ReadLine();
            
            Console.WriteLine("Enter the algorithm to digest with");
            String Algorithm = Console.ReadLine();

            if (!Int32.TryParse(Algorithm, out Int32 IntAlgorithm))
                throw new Exception("Algorithm must be an integer.");

            PBSHA3Digest Sha3Digest = new PBSHA3Digest(IntAlgorithm);

            String Digest = Sha3Digest.CreateStringDigest(Input);
            
            Console.WriteLine("Digest:");
            Console.WriteLine(Digest);
            Console.ReadKey();
        }
    }
}