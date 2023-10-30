//
// Created by Jalter on 10/31/2023.
//

// See https://aka.ms/new-console-template for more information

using C4_ECB_on_DES.Lib;

const string MAIN_KEY = "AABB09182736CCDD";
const string PLAIN_TEXT = "Ha Huy Hoang";

RoundKey roundKey = new(Utilities.HexToBinary(MAIN_KEY));
roundKey.Print();

Console.WriteLine($"\nPlain text: {PLAIN_TEXT}.");
string cipherText = Ecb.Encrypt(PLAIN_TEXT, roundKey);
Console.WriteLine($"Cipher text: {cipherText}");
Console.WriteLine($"Decrypted text: {Ecb.Encrypt(cipherText, roundKey, true)}.");


