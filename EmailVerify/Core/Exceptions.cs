using System;

namespace EmailVerify.Core
{
	#region --- Exception classes ---

	public class EmailVerifyException : Exception
	{
		public EmailVerifyException()
		{
		}

		public EmailVerifyException(string message) : base(message)
		{
		}

		public EmailVerifyException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class EmailFormatException : EmailVerifyException
	{
		public EmailFormatException()
		{
		}

		public EmailFormatException(string message) : base(message)
		{
		}

		public EmailFormatException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class DomainValidationException : EmailVerifyException
	{
		public DomainValidationException()
		{
		}

		public DomainValidationException(string message) : base(message)
		{
		}

		public DomainValidationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class NxDomainException : DomainValidationException
	{
		public NxDomainException()
		{
		}

		public NxDomainException(string message) : base(message)
		{
		}

		public NxDomainException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class NoMxRecordException : DomainValidationException
	{
		public NoMxRecordException()
		{
		}

		public NoMxRecordException(string message) : base(message)
		{
		}

		public NoMxRecordException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class EmailValidationException : EmailVerifyException
	{
		public EmailValidationException()
		{
		}

		public EmailValidationException(string message) : base(message)
		{
		}

		public EmailValidationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class NxEmailException : EmailVerifyException
	{
		public NxEmailException()
		{
		}

		public NxEmailException(string message) : base(message)
		{
		}

		public NxEmailException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class TimeOutException : EmailVerifyException
	{
		public TimeOutException()
		{
		}

		public TimeOutException(string message) : base(message)
		{
		}

		public TimeOutException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
	#endregion
}
