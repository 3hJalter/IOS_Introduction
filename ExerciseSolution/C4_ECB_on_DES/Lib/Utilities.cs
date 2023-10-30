//
// Created by Jalter on 10/31/2023.
//


using System.Text;

namespace C4_ECB_on_DES.Lib;

public static class Utilities
{
    // String to binary
    public static string StringToBinary(string input)
    {
        StringBuilder result = new();
        foreach (char c in input) result.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        return result.ToString();
    }
    
    // Binary to string
    public static string BinaryToString(string input)
    {
        StringBuilder result = new();
        for (int i = 0; i < input.Length; i += 8) result.Append((char) Convert.ToInt32(input.Substring(i, 8), 2));
        return result.ToString();
    }
    
    // Hexadecimal to binary
    public static string HexToBinary(string hex)
    {
        StringBuilder result = new();
        foreach (char c in hex) result.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
        return result.ToString();
    }
    
    // XOR two strings
    public static string Xor(string a, string b)
    {
        string result = "";
        for (int i = 0; i < a.Length; i++) result += a[i] == b[i] ? '0' : '1';
        return result;
    }
    
    // Permute the given string using the given table
    public static string Permute(string input, IEnumerable<int> table)
    {
        return table.Aggregate("", (current, index) => current + input[index - 1]);
    }
}
