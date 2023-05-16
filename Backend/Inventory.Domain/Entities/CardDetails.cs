using System.Security.Cryptography;
using System.Text;
using Inventory.Domain.Abstractions;
using Aes = System.Security.Cryptography.Aes;

namespace Inventory.Domain.Entities;

public class CardDetail : IIdentifiable
{
    private const string Password = "DAVR";
    private static readonly byte[] Salt = Encoding.Unicode.GetBytes("This is my salt value.");
    public Guid Id { get; set; }

    public string Cn { get; set; }

    // [NotMapped]
    // public string CardNumber
    // {
    //     get => Encrypt(_cn);
    //     set => _cn = Decrypt(value);
    // }

    public string Em { get; set; }

    // [NotMapped]
    // public string ExpirationMonth
    // {
    //     get => Encrypt(_em);
    //     set => _em = Decrypt(value);
    // }

    public string Ey { get; set; }

    // [NotMapped]
    // public string ExpirationYear
    // {
    //     get => Encrypt(_ey);
    //     set => _ey = Decrypt(value);
    // }

    public string Cv { get; set; }

    // [NotMapped]
    // public string CVV
    // {
    //     get => Encrypt(_cv);
    //     set => _cv = Decrypt(value);
    // }

    public string Chn { get; set; }

    // [NotMapped]
    // public string CardHolderName
    // {
    //     get => Encrypt(_chn);
    //     set => _chn = Decrypt(value);
    // }

    public Customer Customer { get; set; }

    public CardDetail()
    {
    }

    private CardDetail(string cardNumber, string expiryMonth, string expiryYear, string cvv,
        string cardHolderName)
    {
        Cn = Encrypt(cardNumber);
        Em = Encrypt(expiryMonth);
        Ey = Encrypt(expiryYear);
        Cv = Encrypt(cvv);
        Chn = Encrypt(cardHolderName);
    }


    public static CardDetail Create(string cardNumber, string expiryMonth, string expiryYear, string cvv,
        string cardHolderName) => new(cardNumber, expiryMonth, expiryYear, cvv, cardHolderName);

    private string Encrypt(string text)
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

    private string Decrypt(string encryptedText)
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
                    {
                        memoryStreamDecrypted.Write(buffer, 0, read);
                    }

                    decryptedBytes = memoryStreamDecrypted.ToArray();
                }
            }
        }

        return Encoding.Unicode.GetString(decryptedBytes);
    }
}