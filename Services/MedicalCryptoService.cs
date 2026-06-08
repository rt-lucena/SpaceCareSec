using System.Security.Cryptography;
using System.Text;

namespace SpaceCare.Services
{
    /// <summary>Serviço de criptografia AES-256-GCM para telemetria médica.</summary>
    public class MedicalCryptoService
    {
        private readonly byte[] _key;
        private const int TagSize = 16;   // 128 bits
        private const int NonceSize = 12; // 96 bits (NIST SP 800-38D)

        /// <summary>Inicializa o serviço carregando a chave de ambiente ou configuração.</summary>
        public MedicalCryptoService(IConfiguration configuration)
        {
            var keyHex = Environment.GetEnvironmentVariable("SPACECARE_AES_KEY")
                        ?? configuration["Crypto:AesKey"]
                        ?? throw new InvalidOperationException("Chave criptográfica não configurada (SPACECARE_AES_KEY ou Crypto:AesKey).");

            _key = Convert.FromHexString(keyHex);

            if (_key.Length != 32)
                throw new InvalidOperationException("A chave AES deve ter exatamente 256 bits (32 bytes).");
        }

        /// <summary>Cifra o texto. Retorno: BASE64_NONCE:BASE64_CIPHERTEXT:BASE64_TAG</summary>
        public string Encrypt(string plaintext)
        {
            var nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);

            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var ciphertext = new byte[plaintextBytes.Length];
            var tag = new byte[TagSize];

            using var aesGcm = new AesGcm(_key, TagSize);
            aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag);

            return $"{Convert.ToBase64String(nonce)}:{Convert.ToBase64String(ciphertext)}:{Convert.ToBase64String(tag)}";
        }

        /// <summary>Decifra e valida o formato gerado por Encrypt.</summary>
        public string Decrypt(string encryptedValue)
        {
            var parts = encryptedValue.Split(':');
            if (parts.Length != 3)
                throw new FormatException("Formato inválido. Esperado: BASE64_NONCE:BASE64_CIPHERTEXT:BASE64_TAG");

            var nonce = Convert.FromBase64String(parts[0]);
            var ciphertext = Convert.FromBase64String(parts[1]);
            var tag = Convert.FromBase64String(parts[2]);
            var plaintext = new byte[ciphertext.Length];

            using var aesGcm = new AesGcm(_key, TagSize);
            aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

            return Encoding.UTF8.GetString(plaintext);
        }

        /// <summary>Gera uma nova chave aleatória de 256 bits em string hexadecimal.</summary>
        public static string GenerateKey()
        {
            var key = new byte[32];
            RandomNumberGenerator.Fill(key);
            return Convert.ToHexString(key).ToLowerInvariant();
        }
    }
}