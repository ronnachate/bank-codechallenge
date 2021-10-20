using System;

namespace WeDev.Payment.Services.Transactions.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class InquiryDomainException : Exception
    {
        public InquiryDomainException()
        { }

        public InquiryDomainException(string message)
            : base(message)
        { }

        public InquiryDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
