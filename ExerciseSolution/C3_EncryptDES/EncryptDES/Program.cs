//
// Created by Jalter on 10/17/2023.
//

// See https://aka.ms/new-console-template for more information

using EncryptDES.Lib;

const string MAIN_KEY = "AABB09182736CCDD";
const string PLAIN_TEXT = "Ha Huy Hoang";

RoundKey roundKey = new(Utilities.HexToBinary(MAIN_KEY));
roundKey.Print();

Console.WriteLine($"Plain text: {PLAIN_TEXT}.");
string cipherText = Des.Encrypt(PLAIN_TEXT, roundKey);
Console.WriteLine($"Cipher text: {cipherText}");
Console.WriteLine($"Decrypted text: {Des.Decrypt(cipherText, roundKey)}.");


