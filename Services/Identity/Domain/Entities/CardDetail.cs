using System.Security.Cryptography;
using System.Text;
using Identity.Models;
using Identity.Models.Messages;
using Shared.Abstraction;
using Aes = System.Security.Cryptography.Aes;

namespace Identity.Domain.Entities;

public class CardDetail : IIdentifiable
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

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

    /// <summary>
    /// Reference for User
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Foreign key for User
    /// </summary>
    public Guid UserId { get; set; }

    public CardDetail()
    {
    }

    private CardDetail(string cardNumber, string expiryMonth, string expiryYear, string cvv, string cardHolderName,
        Guid userId)
    {
        Cn = cardNumber;
        Em = expiryMonth;
        Ey = expiryYear;
        Cv = cvv;
        Chn = cardHolderName;
        UserId = userId;
    }

    public static string Encrypt(string text)
    {
        string password = "dotnetrunn";
        byte[] salt = Encoding.UTF8.GetBytes(password);
        byte[] encryptedBytes;
        using (var aes = Aes.Create())
        {
            var key = new Rfc2898DeriveBytes(password, salt);
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

    public static CardDetail FromDto(CardCreateCommand dto, Guid userId)
        => new(dto.CardNumber, dto.ExpiryMonth, dto.ExpiryYear, dto.Cvv, dto.CardHolderName, userId);

    public CardReadDto ToDto()
        => new(this.Cn, this.Em, this.Ey, this.Cv, this.Chn);
}