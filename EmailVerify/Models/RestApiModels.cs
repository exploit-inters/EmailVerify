using System;

namespace EmailVerify.Models
{
    public class ErrorType
    {
        public string Type;
        public string Description;
    }

    public class EmailVerifyResult
    {
        public string Status;
        public ErrorType Error;
    }
}
