namespace EncryptDES.Lib;

public static class Des
{
    // Encrypt the given plain text with larger than 64 bit length using key stored in RoundKey object by cutting the plain text into 64 bit blocks
    public static string Encrypt(string plainText, RoundKey roundKey)
    {
        // 1. If plain text is not a multiple of 64 bit, pad it with spaces
        if (plainText.Length % 8 != 0)
        {
            int padLength = 8 - plainText.Length % 8;
            for (int i = 0; i < padLength; i++) plainText += " ";
        }

        // Console.WriteLine("New plain text: " + plainText + ".");
        // 2. Convert the plain text to binary
        string binaryPlainText = Utilities.StringToBinary(plainText);
        // 3. Split the binary plain text into 64 bit blocks
        List<string> blocks = new();
        for (int i = 0; i < binaryPlainText.Length; i += 64)
            blocks.Add(i + 64 > binaryPlainText.Length
                ? binaryPlainText.Substring(i)
                : binaryPlainText.Substring(i, 64));
        // Console.WriteLine("Block " + (i / 64 + 1) + ": " + blocks[^1]);
        // 4. Encrypt each block and Return the cipher text
        return blocks.Aggregate("", (current, block) => current + Encrypt64BitString(block, roundKey));
    }

    // Decrypt the given cipher text with larger than 64 bit length using key stored in RoundKey object by cutting the cipher text into 64 bit blocks
    public static string Decrypt(string cipherText, RoundKey roundKey)
    {
        // 1. Convert the cipher text to binary
        string binaryCipherText = Utilities.StringToBinary(cipherText);
        // 2. Split the binary cipher text into 64 bit blocks
        List<string> blocks = new();
        for (int i = 0; i < binaryCipherText.Length; i += 64)
            blocks.Add(i + 64 > binaryCipherText.Length
                ? binaryCipherText.Substring(i)
                : binaryCipherText.Substring(i, 64));
        // 3. Decrypt each block
        string plainText = blocks.Aggregate("", (current, block) => current + Decrypt64BitString(block, roundKey));
        // 4. Remove the padding spaces
        plainText = plainText.TrimEnd();
        // 5. Return the plain text
        return plainText;
    }


    // Encrypt the given plain text using key stored in RoundKey object
    private static string Encrypt64BitString(string plainText, RoundKey roundKey, bool isBinaryPlainText = true)
    {
        string binaryPlainText = plainText;
        // 1. Convert the plain text to binary
        if (!isBinaryPlainText) binaryPlainText = Utilities.StringToBinary(plainText);
        // 2. Permute the binary plain text using IP table
        // Console.WriteLine("Binary plain text: " + binaryPlainText);
        string permutedPlainText = Utilities.Permute(binaryPlainText, Tables.Ip);
        // 3. Split the permuted plain text into two halves
        string left = permutedPlainText.Substring(0, 32);
        string right = permutedPlainText.Substring(32, 32);
        // 4. Apply 16 rounds
        for (int i = 0; i < 16; i++)
        {
            // 4.1. Save the old right half
            string oldRight = right;
            // 4.2. Apply the F function to the right half
            string f = FunctionF.F(right, roundKey.GetKey(i));
            // 4.3. XOR the left half with the F function result
            right = Utilities.Xor(left, f);
            // 4.4. Set the left half to the old right half
            left = oldRight;
        }

        // 5. Merge the two halves
        string merged = right + left;
        // 6. Permute the merged string using IP-1 table
        string permuted = Utilities.Permute(merged, Tables.Ip1);
        // 7. Convert the permuted string to cipher text
        return Utilities.BinaryToString(permuted);
    }

    // Decrypt the given cipher text using key stored in RoundKey object
    private static string Decrypt64BitString(string cipherText, RoundKey roundKey, bool isBinaryCipherText = true)
    {
        string binaryCipherText = cipherText;
        // 1. Convert the cipher text to binary
        if (!isBinaryCipherText) binaryCipherText = Utilities.StringToBinary(cipherText);
        // Console.WriteLine("Binary cipher text: " + binaryCipherText);
        // 2. Permute the binary cipher text using IP table
        string permutedCipherText = Utilities.Permute(binaryCipherText, Tables.Ip);
        // 3. Split the permuted cipher text into two halves
        string left = permutedCipherText.Substring(0, 32);
        string right = permutedCipherText.Substring(32, 32);
        // 4. Apply 16 rounds in reverse order
        for (int i = 15; i >= 0; i--)
        {
            // 4.1. Save the old right half
            string oldRight = right;
            // 4.2. Apply the F function to the right half
            string f = FunctionF.F(right, roundKey.GetKey(i));
            // 4.3. XOR the left half with the F function result
            right = Utilities.Xor(left, f);
            // 4.4. Set the left half to the old right half
            left = oldRight;
        }

        // 5. Merge the two halves
        string merged = right + left;
        // 6. Permute the merged string using IP-1 table
        string permuted = Utilities.Permute(merged, Tables.Ip1);
        // 7. Convert the permuted string to plain text
        return Utilities.BinaryToString(permuted);
    }
}
