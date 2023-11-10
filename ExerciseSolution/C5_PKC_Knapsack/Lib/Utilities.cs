using System.Text;

namespace C5_PKC_Knapsack.Lib;

public static class Utilities
{
    public static string? EncodeToBinary(string text)
    {
        StringBuilder binaryStringBuilder = new StringBuilder();

        foreach (char c in text)
        {
            int charValue;
            switch (c)
            {
                // check if c is a uppercase
                case >= 'A' and <= 'Z':
                    charValue = c - 'A'; // Assuming uppercase letters
                    break;
                case >= 'a' and <= 'z':
                    charValue = c - 'a'; // Assuming lowercase letters
                    break;
                default:
                    return null; // c is not a letter
            }
            string binaryRepresentation = Convert.ToString(charValue, 2).PadLeft(5, '0');
            binaryStringBuilder.Append(binaryRepresentation);
        }

        return binaryStringBuilder.ToString();
    }

    public static string DecodeBinaryToText(string binaryText)
    {
        StringBuilder textStringBuilder = new();

        for (int i = 0; i < binaryText.Length; i += 5)
        {
            string binaryRepresentation = binaryText.Substring(i, 5);
            int charValue = Convert.ToInt32(binaryRepresentation, 2);
            char c = (char)(charValue + 'A');
            textStringBuilder.Append(c);
        }

        return textStringBuilder.ToString();
    }

    public static long InverseModulo(long number, long modulo)
    {
        for (long i = 1; i < modulo; i++)
        {
            if ((number * i) % modulo == 1)
            {
                return i;
            }
        }
        throw new ArithmeticException("Modular inverse does not exist.");
    }

    public static long GetRandomCoPrime(long number)
    {
        Random random = new();
        long randomCoPrime = random.Next(2, (int)number);
        while (Gcd(number, randomCoPrime) != 1)
        {
            randomCoPrime = random.Next(2, (int)number);
        }
        return randomCoPrime;
    }

    private static long Gcd(long number, long randomCoPrime)
    {
        while (randomCoPrime != 0)
        {
            long temp = randomCoPrime;
            randomCoPrime = number % randomCoPrime;
            number = temp;
        }
        return number;
    }
}
