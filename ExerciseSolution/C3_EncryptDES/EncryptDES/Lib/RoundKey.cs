//
// Created by Jalter on 10/17/2023.
//


namespace EncryptDES.Lib;

public class RoundKey
{
    private readonly List<string> roundKeys = new();

    // Generate 16 sub-keys, each of which is 48-bits long from the original 64-bits key
    public RoundKey(string key)
    {
        if (key.Length != 64) throw new ArgumentException("The key must be 64-bits long.");
        // 1. Permute the key using PC-1 table
        string permutedKey = Utilities.Permute(key, Tables.Pc1);
        // 2. Split the permuted key into two halves
        string left = permutedKey.Substring(0, 28);
        string right = permutedKey.Substring(28, 28);
        // 3. Generate 16 sub-keys
        for (int i = 0; i < 16; i++)
        {
            // 3.1. Shift the two halves
            left = Shift(left, Tables.Shift[i]);
            right = Shift(right, Tables.Shift[i]);
            // 3.2. Merge the two halves
            string merged = left + right;
            // 3.3. Permute the merged key using PC-2 table
            string subKey = Utilities.Permute(merged, Tables.Pc2);
            // 3.4. Add the sub-key to the list
            roundKeys.Add(subKey);
        }
    }

    public string GetKey(int index)
    {
        return roundKeys[index];
    }

    // Shift the key by the given number of bits
    private static string Shift(string key, int shift)
    {
        return string.Concat(key.AsSpan(shift), key.AsSpan(0, shift));
    }
    
    // print the sub-keys
    public void Print()
    {
        for (int i = 0; i < roundKeys.Count; i++) Console.WriteLine($"K{i + 1} = {roundKeys[i]}");
    }
}
