using System;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities.Encoders;

namespace PointBlank.API.Security
{
    public class PBSHA3Digest : Sha3Digest
    {
        public Int32 Bits;
        
        internal PBSHA3Digest(int bits) : base(bits)
        {
            Bits = bits;
        }
        
        internal Int32 DoUnevenFinal(Byte[] output, Int32 outOff, Byte partialByte, Int32 partialBits) => DoFinal(output, outOff, partialByte, partialBits);

        public Byte[] CreateDigest(Byte[] Input)
        {
            Int32 partialBits = Bits % 8;
            
            byte[] output = new byte[GetDigestSize()];
            
            if (partialBits == 0)
            {
                BlockUpdate(Input, 0, Input.Length);
                DoFinal(output, 0);
            }
            else
            {
                BlockUpdate(Input, 0, Input.Length - 1);
                DoUnevenFinal(output, 0, Input[Input.Length - 1], partialBits);
            }
            return output;
        }
        
        public Byte[] CreateDigest(String Input) => CreateDigest(Encoding.UTF8.GetBytes(Input));

        public String CreateStringDigest(Byte[] Input) => Hex.ToHexString(CreateDigest(Input)).ToUpper();

        public String CreateStringDigest(String Input) => Hex.ToHexString(CreateDigest(Input)).ToUpper();
    }
}