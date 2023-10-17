//
// Created by Jalter on 10/17/2023.
//

namespace EncryptDES.Lib;

public static class FunctionF
{
    // F function
    public static string F(string right, string subKey)
    {
        // 1. Expand the right half from 32-bits to 48-bits
        string expanded = Utilities.Permute(right, Tables.E);
        // 2. XOR the expanded right half with the sub-key
        string xorEd = Utilities.Xor(expanded, subKey);
        // 3. Split the xorEd string into 8 groups of 6 bits
        string[] groups = new string[8];
        for (int i = 0; i < 8; i++) groups[i] = xorEd.Substring(i * 6, 6);
        // 4. Apply S-boxes to each group
        string result = "";
        for (int i = 0; i < 8; i++) result += S(groups[i], i);
        // 5. Permute the result using P table
        return Utilities.Permute(result, Tables.P);
    }
    
    // S-box
    private static string S(string group, int index)
    {
        int[,] s = index switch
        {
            0 => Tables.S1,
            1 => Tables.S2,
            2 => Tables.S3,
            3 => Tables.S4,
            4 => Tables.S5,
            5 => Tables.S6,
            6 => Tables.S7,
            7 => Tables.S8,
            _ => throw new ArgumentException("Invalid S-box index.")
        };
        int row = Convert.ToInt32(group[0].ToString() + group[5], 2);
        int col = Convert.ToInt32(group.Substring(1, 4), 2);
        int value = s[row, col];
        return Convert.ToString(value, 2).PadLeft(4, '0');
    }
}
