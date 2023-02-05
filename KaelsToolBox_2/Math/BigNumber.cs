using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Math
{
    public struct BigNumber
    {
        public BigNumber()
        {
        }

        public List<bool> BinaryValue { get; private set; } = new();
        public string Value { get => GetConvertedValue(); private set => Value = value; }

        private string GetConvertedValue()
        {
            string combined = string.Empty;
            int count = 0;
            foreach (bool b in BinaryValue)
            {
                combined += b ? 1 : 0;
                count++;
                if (count % 4 == 0) combined += " ";
            }

            char[] charArray = combined.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public void Add(BigNumber number)
        {
            for (int i = 0; i < number.BinaryValue.Count; i++) if (number.BinaryValue[i]) AddNumberAtPos(i);
        }
        public void AddNumberAtPos(int pos)
        {
            int curentCount = BinaryValue.Count;

            if (curentCount == pos) BinaryValue.Add(true);
            else if (curentCount < pos)
            {
                for (int i = 0; i < pos - curentCount; i++) BinaryValue.Add(false);
                BinaryValue.Add(true);
            }
            else if (BinaryValue[pos]) { BinaryValue[pos] = false; AddNumberAtPos(pos + 1); }
            else BinaryValue[pos] = true;
        }
    }
}
