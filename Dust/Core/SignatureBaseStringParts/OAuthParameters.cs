﻿using System.Collections.Generic;

namespace Dust.Core.SignatureBaseStringParts
{
    public class OAuthParameters
    {
        private readonly ConsumerKey _key;
        private readonly TokenKey _tokenKey;
        private readonly string _signatureMethod;
        private readonly string _timestamp;
        private readonly string _nonce;

        public OAuthParameters(ConsumerKey key, TokenKey tokenKey, string signatureMethod, string timestamp, string nonce)
        {
            _key = key;
            _tokenKey = tokenKey;
            _signatureMethod = signatureMethod;
            _timestamp = timestamp;
            _nonce = nonce;
        }

        public static OAuthParameters Empty = new OAuthParameters(
            new ConsumerKey(string.Empty), 
            new TokenKey(string.Empty), 
            string.Empty, 
            string.Empty, 
            string.Empty
        );

        internal List<Parameter> List()
        {
            return new List<Parameter>
            {
                new Parameter(Name.Version, "1.0"),
                new Parameter(Name.ConsumerKey, _key.Value),
                new Parameter(Name.Token, _tokenKey.Value),
                new Parameter(Name.SignatureMethod, _signatureMethod),
                new Parameter(Name.Timestamp, _timestamp),
                new Parameter(Name.Nonce, _nonce)
            };
        }
    }
}