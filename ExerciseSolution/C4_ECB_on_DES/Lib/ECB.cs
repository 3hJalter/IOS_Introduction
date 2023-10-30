namespace C4_ECB_on_DES.Lib;

public static class Ecb
{
    // Encrypt the given text using the given round key, isReverseKey is true for Decryption, false for encryption
    public static string Encrypt(string text, RoundKey roundKey, bool isReverseKey = false)
    {
        // 1. If plain text is not a multiple of 64 bit, pad it with spaces
        if (!isReverseKey && text.Length % 8 != 0)
        {
            int padLength = 8 - text.Length % 8;
            for (int i = 0; i < padLength; i++) text += " ";
        }

        // Console.WriteLine("New plain text: " + plainText + ".");
        // 2. Convert the text to binary
        string binaryText = Utilities.StringToBinary(text);
        // 3. Split the binary text into 64 bit blocks
        List<string> blocks = new();
        for (int i = 0; i < binaryText.Length; i += 64)
            blocks.Add(i + 64 > binaryText.Length
                ? binaryText[i..]
                : binaryText.Substring(i, 64));
        // 4. Encrypt each block and Return the encryption text
        string result = blocks.Aggregate("",
            (current, block) => current + Des.Encrypt64BitString(block, roundKey, true, isReverseKey));
        return isReverseKey ? result.TrimEnd() : result;
    }
}
