using Topshelf;

namespace EmailVerify
{
    class Program
    {
        static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<WebApiService>(s =>
                {
                    s.ConstructUsing(name => new WebApiService());
                    s.WhenStarted(svc => svc.Start());
                    s.WhenStopped(svc => svc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Служба верификации email");
                x.SetDisplayName("Email verify");
                x.SetServiceName("EmailVerify");
            });
        }
    }
}
