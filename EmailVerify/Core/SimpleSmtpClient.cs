using System.IO;
using System.Net.Mail;
using System.Net.Sockets;

namespace EmailVerify.Core
{
    public class SmtpResponse
    {
        public SmtpStatusCode StatusCode;
        public string Description;

        public SmtpResponse(string response)
        {
            if (response.Length < 4)
                throw new SmtpException($"Response is to short: {response}");

            if (response[3] != ' ' && response[3] != '-')
                throw new SmtpException($"Response format is wrong: {response}");

            StatusCode = (SmtpStatusCode)int.Parse(response.Substring(0, 3));
            Description = response;
        }
    }

    public class SimpleSmtpClient
    {
        private readonly TcpClient _tcpClient;
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        public SimpleSmtpClient(string host, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(host, port);
            _writer = new StreamWriter(_tcpClient.GetStream());
            _reader = new StreamReader(_tcpClient.GetStream());
        }

        ~SimpleSmtpClient()
        {
            _reader.Close();
            _writer.Close();
            _tcpClient.Close();
        }

        public SmtpResponse Read()
        {
            return new SmtpResponse(_reader.ReadLine());
        }

        public SmtpResponse SendCommand(string command)
        {
            _writer.Write(command);
            _writer.Write("\r\n");
            _writer.Flush();

            return Read();
        }
    }
}
