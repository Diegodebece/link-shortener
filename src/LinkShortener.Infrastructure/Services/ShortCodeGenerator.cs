using LinkShortener.Domain.Services;
using LinkShortener.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LinkShortener.Infrastructure.Services
{
    public class ShortCodeGenerator : IShortCodeGenerator
    {

        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int CodeLength = 7;

        public ShortCode Generate()
        {
            var characters = new char[CodeLength];

            for (var i = 0; i < CodeLength; i++)
            {
                var index = RandomNumberGenerator.GetInt32(Alphabet.Length);
                characters[i] = Alphabet[index];
            }

            return ShortCode.Create(new string(characters));
        }
    }
}
