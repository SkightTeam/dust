﻿using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Dust;
using Dust.Core;
using Dust.Core.SignatureBaseStringParts.Parameters;
using Dust.Core.SignatureBaseStringParts.Parameters.Nonce;
using Dust.Core.SignatureBaseStringParts.Parameters.Timestamp;
using Dust.Http;
using fit;
using fitlibrary;
using Wiki.Machinery.Http;
using Wiki.Machinery.Support;

namespace Wiki.Machinery.Examples
{
    public class XeroConnectionFlowFixture : DoFixture
    {
        private Consumer _consumer;
        private X509Certificate2 _cert;

        public void Assuming_we_have_a_consumer_for_xero()
        {
            _consumer = Settings.Xero;
        }

        public void And_a_signing_certificate()
        {
            _cert = Settings.XeroSigningCert;
        }

        public Fixture Then_we_can_connect()
        {
            var oAuthParameters = new OAuthParameters(
                _consumer.ConsumerKey,
                new TokenKey(_consumer.ConsumerKey.Value),
                "RSA-SHA1",
                new DefaultTimestampSequence(),
                new DefaultNonceSequence(),
                string.Empty,
                "1.0"
            );

            var earl = new Uri("https://api.xero.com/api.xro/2.0/Organisation");

            var signatureBaseString =
                new SignatureBaseString(
                    new Request
                    {
                        Url = earl,
                        Verb = "GET"
                    },
                    oAuthParameters
                );

            var signature = new RsaSha1(_cert).Sign(signatureBaseString);

            oAuthParameters.SetSignature(signature);

            var header = new AuthorizationHeader(oAuthParameters, string.Empty);

            var req = (HttpWebRequest)WebRequest.Create(earl);

            req.Headers.Add("Authorization", header.Value);

            var response = TInternet.Get(req);
            const HttpStatusCode expected = HttpStatusCode.OK;
            var actual = response.StatusCode;

            return new YesNoFixture(actual == expected, "Expected [" + expected + "]. Got [" + actual + "]. And here is the body: " + response.Body, 2);
        }
    }
}