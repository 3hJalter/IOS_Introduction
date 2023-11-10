using System.Text;

namespace C5_PKC_Knapsack.Lib;

public class MerkleHelmanCrypto
{
    private readonly List<int> aSuperIncreasing;
    private readonly long m;
    private readonly long miOmega;
    private readonly List<long> a;
    public MerkleHelmanCrypto(string plainText)
    {
        aSuperIncreasing = new List<int> {1};
        for (int i = 1; i < plainText.Length; i++)
        {
            int element = aSuperIncreasing.Sum() + 1;
            aSuperIncreasing.Add(element);
        }
        Console.WriteLine(string.Join(", ", aSuperIncreasing));
        m = aSuperIncreasing.Sum() + 10; // Use long for larger numbers
        long omega = Utilities.GetRandomCoPrime(m); // co-prime with m
        miOmega = Utilities.InverseModulo(omega, m); // Modular multiplicative inverse of omega (mod m)
        a = new List<long>(aSuperIncreasing.Select(i => i * omega % m));
        Console.WriteLine($"Super-increasing vector: {string.Join(", ", aSuperIncreasing)}");
        Console.WriteLine($"m = {m}");
        Console.WriteLine($"Omega: {omega}");
        Console.WriteLine($"Multiplicative inverse of Omega (mod m): {miOmega}");
        Console.WriteLine("Public key: " + string.Join(", ", a));
    }

    public long Encryption(string plainText)
    {
        long cipherText = plainText.Select((t, i) => a[i] * int.Parse(t.ToString())).Sum();
        Console.WriteLine($"T = {cipherText}");
        return cipherText;
    }

    public void Decryption(long cipherText)
    {
        StringBuilder newPlainText = new();
        long newT = cipherText * miOmega % m;
        if (newT < 0) newT += m; // Ensure the result is non-negative
        Console.WriteLine($"T' = {cipherText} * {miOmega} % {m} = {newT}");

        for (int i = aSuperIncreasing.Count - 1; i >= 0; i--)
        {
            char c;
            if (newT >= aSuperIncreasing[i])
            {
                c = '1';
                newT -= aSuperIncreasing[i];
                Console.WriteLine($"T'[{i:00}]: {newT,-6} = T[{i + 1:00}] - {aSuperIncreasing[i],-6} => X[{i:00}]: {c}");
            }
            else
            {
                c = '0';
                Console.WriteLine($"T'[{i:00}]: {newT,-6} = T[{i + 1:00}] => X[{i:00}]: {c}");
            }

            newPlainText.Insert(0, c);
        }

        Console.WriteLine($"Result of the decryption: {newPlainText}");
        Console.WriteLine($"Original name: {Utilities.DecodeBinaryToText(newPlainText.ToString())}");
    }
}
