using Michal.Project.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Michal.Project.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {

       // private UserRepository _repo = null;

        public RefreshTokensController()
        {

          //  _repo = new UserRepository();
        }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            using (var context = new ApplicationDbContext())
            {
                UserRepository usr = new UserRepository(context);
                return Ok(usr.GetAllRefreshTokens());
            }
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            bool result;
            //var result = await _repo.RemoveRefreshToken(tokenId);
            using (var context = new ApplicationDbContext())
            {
                UserRepository usr = new UserRepository(context);
                result = await usr.RemoveRefreshToken(tokenId);
            }
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");

        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _repo.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}
    }
}