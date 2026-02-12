using System.Security.Cryptography;
using System.Text;
using Locked_IN_Backend.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Locked_IN_Backend.Services;

public class InviteTokenService : IInviteTokenService
{
    private readonly string _secret;

    public InviteTokenService(IConfiguration configuration)
    {
        _secret = configuration["Jwt:Secret"];
    }

    public string GenerateInviteToken(int teamId)
    {
        var key = Encoding.UTF8.GetBytes(_secret.Substring(0, 32));
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(teamId.ToString());
        }

        return Convert.ToBase64String(ms.ToArray()).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    public int DecodeInviteToken(string token)
    {
        try
        {
            var base64 = token.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            var buffer = Convert.FromBase64String(base64);

            var key = Encoding.UTF8.GetBytes(_secret.Substring(0, 32));
            using var aes = Aes.Create();
            aes.Key = key;

            var iv = new byte[aes.BlockSize / 8];
            Array.Copy(buffer, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            var text = sr.ReadToEnd();
            return int.Parse(text);
        }
        catch
        {
            throw new Exception("Invalid invite token.");
        }
    }
}
