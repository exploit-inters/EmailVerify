using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;
using EmailVerify.Models;
using EmailVerify.Singleton;

namespace EmailVerify.Core
{
    class EmailVerifier
    {
        private static readonly int _smtpPort = 25;
        private static readonly string _identity = "example.com";
        private static readonly string _sender = "sender@example.com";

        private static bool IsError(SmtpStatusCode status)
        {
            return (int)status > 400;
        }

	    public static EmailVerifyResult VerifyAsync(string email)
	    {
			var task = Task.Run(() => Verify(email));
			if (!task.Wait(1000))
			    return new EmailVerifyResult
				{
					Status = "OK",
					Error = null
				};

		    return task.Result;
	    }

        public static EmailVerifyResult Verify(string email)
        {
            var result = new EmailVerifyResult
            {
                Status = "OK",
                Error = null
            };

            try
            {
                if (!new EmailAddressAttribute().IsValid(email))
                    throw new EmailFormatException("Invalid email format.");

				Verify(new MailAddress(email));
            }
            catch (EmailVerifyException ex)
            {
	            string description;
	            if (ex is EmailFormatException)
		            description = "Некорректный формат почтового ящика.";
				else if (ex is NxDomainException || ex is NoMxRecordException)
					description = $"Извините, нам не удалось проверить существование почтового ящика. Возможно вы ошиблись в домене {new MailAddress(email).Host}";
				else if (ex is NxEmailException)
					description = "Извините, нам не удалось проверить существование почтового ящика.";
				else
					return result;

				result.Status = "ERROR";
                result.Error = new ErrorType
                {
                    Type = ex.GetType().Name,
                    Description = description
				};
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex);
            }

            return result;
        }

        public static void Verify(MailAddress email)
        {
            // Try get SMTP server
			var client = new DnsClient(DnsClient.GetLocalConfiguredDnsServers(), 1000);
            var dnsMessage = client.Resolve(DomainName.Parse(email.Host), RecordType.Mx);

			if (dnsMessage == null)
				throw new TimeOutException("Operation has timed out!");

	        if (dnsMessage.ReturnCode == ReturnCode.NxDomain)
		        throw new NxDomainException("Domain not exists!");

            if (dnsMessage.ReturnCode != ReturnCode.NoError)
                throw new DomainValidationException("Unable to validate domain!");

            if (!dnsMessage.AnswerRecords.Any())
                throw new NoMxRecordException("No MX records!");

            var mainServer = dnsMessage.AnswerRecords.OfType<MxRecord>().OrderBy(s => s.Preference).First();

            // Try validate account
            var smtpClient = new SimpleSmtpClient(mainServer.ExchangeDomainName.ToString(), _smtpPort);

            var response = smtpClient.Read();
            if (IsError(response.StatusCode))
                throw new EmailValidationException($"Connection refused: {response.Description}");

            response = smtpClient.SendCommand($"HELO {_identity}");
            if (IsError(response.StatusCode))
                throw new EmailValidationException($"Greeting failed: {response.Description}");

            response = smtpClient.SendCommand($"VRFY {email.Address}");
            if (!IsError(response.StatusCode))
                return;

			if (response.StatusCode == SmtpStatusCode.MailboxUnavailable)
				throw new NxEmailException($"Email does not exists: {response.Description}");

			response = smtpClient.SendCommand($"MAIL FROM:<{_sender}>");
            if (IsError(response.StatusCode))
                throw new EmailValidationException($"SMTP server is insane: {response.Description}");

            response = smtpClient.SendCommand($"RCPT TO:<{email.Address}>");
            if (IsError(response.StatusCode))
                throw new NxEmailException($"Email is invalid: {response.Description}");
        }
    }
}
