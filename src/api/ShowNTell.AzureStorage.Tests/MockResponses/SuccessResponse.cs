using Azure;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace ShowNTell.AzureStorage.Tests.MockResponses
{
    public class SuccessResponse : Response
    {
        public override int Status => 200;

        public override string ReasonPhrase => throw new NotImplementedException();

        public override Stream ContentStream { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ClientRequestId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        protected override bool ContainsHeader(string name)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<HttpHeader> EnumerateHeaders()
        {
            throw new NotImplementedException();
        }

        protected override bool TryGetHeader(string name, out string value)
        {
            throw new NotImplementedException();
        }

        protected override bool TryGetHeaderValues(string name, out IEnumerable<string> values)
        {
            throw new NotImplementedException();
        }
    }
}
