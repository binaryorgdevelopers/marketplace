using System.Security.Cryptography;
using System.Text;
using Inventory.Domain.Abstractions;

namespace Inventory.Domain.Entities;

public class CardDetail : IIdentifiable
{
    private const string Password = "dotnetrun";
    private static readonly byte[] Salt = Encoding.Unicode.GetBytes("somekey");

    public CardDetail()
    {
    }

    private CardDetail(string cardNumber, string expiryMonth, string expiryYear, string cvv,
        string cardHolderName, Guid userId)
    {
        Cn = cardNumber;
        Em = expiryMonth;
        Ey = expiryYear;
        Cv = cvv;
        Chn = cardHolderName;
        CustomerId = userId;
    }

    /// <summary>
    ///     Cn-Card Number in encrypted format
    /// </summary>
    public string Cn { get; set; }


    /// <summary>
    ///     Expiry Month in encrypted format
    /// </summary>
    public string Em { get; set; }


    /// <summary>
    ///     Expiry Year in encrypted format
    /// </summary>
    public string Ey { get; set; }


    /// <summary>
    ///     CVV in encrypted format
    /// </summary>
    public string Cv { get; set; }

    /// <summary>
    ///     CardHolder Name in encrypted format
    /// </summary>
    public string Chn { get; set; }


    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public Guid Id { get; set; }


    public static CardDetail Create(string cardNumber, string expiryMonth, string expiryYear, string cvv,
        string cardHolderName, Guid userId)
    {
        return new(cardNumber, expiryMonth, expiryYear, cvv, cardHolderName, userId);
    }

    public static string Encrypt(string text)
    {
        byte[] encryptedBytes;
        using (var aes = Aes.Create())
        {
            var key = new Rfc2898DeriveBytes(Password, Salt);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            using (var encryptor = aes.CreateEncryptor())
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(text);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                encryptedBytes = memoryStream.ToArray();
            }
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] decryptedBytes;

        using (var aes = Aes.Create())
        {
            var key = new Rfc2898DeriveBytes(Password, Salt);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            using (var decryptor = aes.CreateDecryptor())
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText)))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                var buffer = new byte[1024];
                using (var memoryStreamDecrypted = new MemoryStream())
                {
                    int read;
                    while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                        memoryStreamDecrypted.Write(buffer, 0, read);

                    decryptedBytes = memoryStreamDecrypted.ToArray();
                }
            }
        }

        return Encoding.Unicode.GetString(decryptedBytes);
    }
}