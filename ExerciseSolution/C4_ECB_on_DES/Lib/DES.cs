//
// Created by Jalter on 10/31/2023.
//

namespace C4_ECB_on_DES.Lib;

public static class Des
{
    public static string Encrypt64BitString(string text, RoundKey roundKey, bool isBinaryText = true, bool isReverseKey = false)
    {
        string binaryText = text;
        // 1. Convert the text to binary
        if (!isBinaryText) binaryText = Utilities.StringToBinary(text);
        // 2. Permute the binary text using IP table
        // Console.WriteLine("Binary text: " + binaryText);
        string permutedText = Utilities.Permute(binaryText, Tables.Ip);
        // 3. Split the permuted text into two halves
        string left = permutedText.Substring(0, 32);
        string right = permutedText.Substring(32, 32);
        // 4. Apply 16 rounds
        if (!isReverseKey) for (int i = 0; i < roundKey.Count; i++) ApplyFunctionF(i);
        else for (int i = roundKey.Count - 1; i >= 0; i--) ApplyFunctionF(i);
        // 5. Merge the two halves
        string merged = right + left;
        // 6. Permute the merged string using IP-1 table
        string permuted = Utilities.Permute(merged, Tables.Ip1);
        // 7. Convert the permuted string to cipher text
        return Utilities.BinaryToString(permuted);

        void ApplyFunctionF(int keyI)
        {
            // 4.1. Save the old right half
            string oldRight = right;
            // 4.2. Apply the F function to the right half
            string f = FunctionF.F(right, roundKey.GetKey(keyI));
            // 4.3. XOR the left half with the F function result
            right = Utilities.Xor(left, f);
            // 4.4. Set the left half to the old right half
            left = oldRight;
        }
    }
}
