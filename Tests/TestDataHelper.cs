using System.Collections.Generic;

namespace Tests
{
    public static class TestDataHelper
    {
        public static Dictionary<KeyData, ValueData> Generate(int itemCount)
        {
            var output = new Dictionary<KeyData, ValueData>();

            for (int i = 0; i < itemCount; i++)
            {
                var key = new KeyData(i, "Key " + i);
                var value = new ValueData(i, "Value " + i);
                output.Add(key, value);
            }

            return output;
        }
    }
}
