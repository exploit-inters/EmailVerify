using System.Web.Http;
using EmailVerify.Singleton;
using EmailVerify.Core;
using EmailVerify.Models;

namespace EmailVerify.Controllers
{
    [RoutePrefix("api")]
    public class RestApiController : ApiController
    {
        // GET api/universe_answer
        [HttpGet]
        [Route("universe_answer")]
        public string AnswerForUniverse()
        {
            Logger.Log.Trace("AnswerForUniverse called.");
            return "42";
        }

        // GET api/verify_email
        [HttpGet]
        [Route("verify_email")]
        public EmailVerifyResult VerifyEmail(string email)
        {
            Logger.Log.Trace("VerifyEmail called.");
            return EmailVerifier.VerifyAsync(email);
        }
    }
}
