using System.Security.Cryptography;
using System.Text;

namespace NeuralLab.Backend.Utils;

/// <summary>
///     Security manager to the data.
/// </summary>
public class Cryptography
{
    /// <summary>
    ///     Compute a SHA Hash with a entry string.
    /// </summary>
    /// <param name="content">String to compute.</param>
    /// <returns>Return a Base64 String with the hash computed.</returns>
    public static string ComputeHash(string content)
    {
        //  Compute the finish hash.
        return Convert.ToBase64String(SHA512.HashData(Encoding.ASCII.GetBytes(content)));
    }
}