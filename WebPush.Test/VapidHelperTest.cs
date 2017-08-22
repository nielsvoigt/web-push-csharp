﻿using System;
using WebPush.Util;
using Xunit;

namespace WebPush.Test
{
    public class VapidHelperTest
    {
        private const string VALID_AUDIENCE = "http://example.com";
        private const string VALID_SUBJECT = "http://example.com/example";
        private const string VALID_SUBJECT_MAILTO = "mailto:example@example.com";

        [Fact]
        public void TestGenerateVapidKeys()
        {
            var keys = VapidHelper.GenerateVapidKeys();
            var publicKey = UrlBase64.Decode(keys.PublicKey);
            var privateKey = UrlBase64.Decode(keys.PrivateKey);

            Assert.Equal(32, privateKey.Length);
            Assert.Equal(65, publicKey.Length);
        }

        [Fact]
        public void TestGenerateVapidKeysNoCache()
        {
            var keys1 = VapidHelper.GenerateVapidKeys();
            var keys2 = VapidHelper.GenerateVapidKeys();

            Assert.NotEqual(keys1.PublicKey, keys2.PublicKey);
            Assert.NotEqual(keys1.PrivateKey, keys2.PrivateKey);
        }

        [Fact]
        public void TestGetVapidHeaders()
        {
            var publicKey = UrlBase64.Encode(new byte[65]);
            var privatekey = UrlBase64.Encode(new byte[32]);
            var headers = VapidHelper.GetVapidHeaders(VALID_AUDIENCE, VALID_SUBJECT, publicKey, privatekey);

            Assert.True(headers.ContainsKey("Authorization"));
            Assert.True(headers.ContainsKey("Crypto-Key"));
        }

        [Fact]
        public void TestGetVapidHeadersAudienceNotAUrl()
        {
            var publicKey = UrlBase64.Encode(new byte[65]);
            var privatekey = UrlBase64.Encode(new byte[32]);

            Assert.Throws(typeof(ArgumentException),
                delegate { VapidHelper.GetVapidHeaders("invalid audience", VALID_SUBJECT, publicKey, privatekey); });
        }

        [Fact]
        public void TestGetVapidHeadersInvalidPrivateKey()
        {
            var publicKey = UrlBase64.Encode(new byte[65]);
            var privatekey = UrlBase64.Encode(new byte[1]);

            Assert.Throws(typeof(ArgumentException),
                delegate { VapidHelper.GetVapidHeaders(VALID_AUDIENCE, VALID_SUBJECT, publicKey, privatekey); });
        }

        [Fact]
        public void TestGetVapidHeadersInvalidPublicKey()
        {
            var publicKey = UrlBase64.Encode(new byte[1]);
            var privatekey = UrlBase64.Encode(new byte[32]);

            Assert.Throws(typeof(ArgumentException),
                delegate { VapidHelper.GetVapidHeaders(VALID_AUDIENCE, VALID_SUBJECT, publicKey, privatekey); });
        }

        [Fact]
        public void TestGetVapidHeadersSubjectNotAUrlOrMailTo()
        {
            var publicKey = UrlBase64.Encode(new byte[65]);
            var privatekey = UrlBase64.Encode(new byte[32]);

            Assert.Throws(typeof(ArgumentException),
                delegate { VapidHelper.GetVapidHeaders(VALID_AUDIENCE, "invalid subject", publicKey, privatekey); });
        }

        [Fact]
        public void TestGetVapidHeadersWithMailToSubject()
        {
            var publicKey = UrlBase64.Encode(new byte[65]);
            var privatekey = UrlBase64.Encode(new byte[32]);
            var headers = VapidHelper.GetVapidHeaders(VALID_AUDIENCE, VALID_SUBJECT_MAILTO, publicKey,
                privatekey);

            Assert.True(headers.ContainsKey("Authorization"));
            Assert.True(headers.ContainsKey("Crypto-Key"));
        }
    }
}