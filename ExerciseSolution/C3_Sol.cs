// Exercise in Chapter 3: With input value is 0011101110000000, and 5 keys:

using ConsoleApp;

BlockCipher blockCipher = new();
Console.WriteLine(blockCipher.EncryptLoop(3,1,1));

public class BlockCipher
{
    private const string INPUT = "0011101110000000";

    private readonly List<string> keys = new()
    {
        "0011110010010100", "1010100101001101", "1001010011010110", "0100110101100011", "1101011000111111"
    };

    private readonly Dictionary<int, int> permutationTable = new()
    {
        { 0, 0 }, { 1, 4 }, { 2, 8 }, { 3, 12 }, { 4, 1 }, { 5, 5 }, { 6, 9 }, { 7, 13 }, { 8, 2 }, { 9, 6 },
        { 10, 10 }, { 11, 14 }, { 12, 3 }, { 13, 7 }, { 14, 11 }, { 15, 15 }
    };

    private readonly Dictionary<int, int> substitutionTable = new()
    {
        { 0, 14 }, { 1, 4 }, { 2, 13 }, { 3, 1 }, { 4, 2 }, { 5, 15 }, { 6, 11 }, { 7, 8 }, { 8, 3 }, { 9, 10 },
        { 10, 6 }, { 11, 12 }, { 12, 5 }, { 13, 9 }, { 14, 0 }, { 15, 7 }
    };

    public string EncryptLoop1(int loop)
    {
        string encrypt = INPUT;
        for (int i = 0; i < loop; i++)
        {
            // XOR the input string with the key
            string xor = "";
            for (int j = 0; j < encrypt.Length; j++) xor += encrypt[j] == keys[i][j] ? "0" : "1";
            // Substitute each 4 characters of the result by XOR with the substitution table int convert to 4 bit binary
            string sub = "";
            for (int j = 0; j < xor.Length; j += 4)
            {
                string temp = "";
                for (int k = 0; k < 4; k++) temp += xor[j + k];
                sub += Convert.ToString(substitutionTable[Convert.ToInt32(temp, 2)], 2).PadLeft(4, '0');
            }
            // Permute the result of the substitution with the permutation table
            string permute = "";
            for (int j = 0; j < sub.Length; j++) permute += sub[permutationTable[j]];
            encrypt = permute;
        }
        return encrypt;
    }
    
    public string Encrypt(string input, string key, bool isSubstitution = true, bool isPermutation = true)
    {
        string encrypt = input;
        // XOR the input string with the key
        string xor = "";
        for (int j = 0; j < encrypt.Length; j++) xor += encrypt[j] == key[j] ? "0" : "1";
        encrypt = xor;
        string sub = "";
        if (isSubstitution)
        {
            for (int j = 0; j < xor.Length; j += 4)
            {
                string temp = "";
                for (int k = 0; k < 4; k++) temp += xor[j + k];
                sub += Convert.ToString(substitutionTable[Convert.ToInt32(temp, 2)], 2).PadLeft(4, '0');
            }
            encrypt = sub;
        }
        string permute = "";
        if (isPermutation)
        {
            for (int j = 0; j < sub.Length; j++) permute += sub[permutationTable[j]];
            encrypt = permute;
        }

        return encrypt;
    }
    
    public string EncryptLoop(int loop, int loopWithNoPermutation, int loopWithNoPermutationAndSubstitution)
    {
        string encrypt = INPUT;
        int keyIndex = 0;
        for (int i = 0; i < loop; i++)
        {
            encrypt = Encrypt(encrypt, keys[keyIndex]);
            keyIndex++;
        }
        for (int i = 0; i < loopWithNoPermutation; i++)
        {
            encrypt = Encrypt(encrypt, keys[keyIndex], true, false);
            keyIndex++;
        }
        for (int i = 0; i < loopWithNoPermutationAndSubstitution; i++)
        {
            encrypt = Encrypt(encrypt, keys[keyIndex], false, false);
            keyIndex++;
        }
        return encrypt;
    }
}
