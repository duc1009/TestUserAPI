namespace TestUserApi.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.WebPages;
    using TestUserApi.Models;

    /// <summary>
    /// Defines the <see cref="UserInfoesController" />.
    /// </summary>
    public class UserInfoesController : ApiController
    {
        /// <summary>
        /// Defines the db.
        /// </summary>
        private UsermanagementEntities1 db = new UsermanagementEntities1();

        /// <summary>
        /// The GetUserInfoes.
        /// </summary>
        /// <returns>The <see cref="IQueryable{UserInfo}"/>.</returns>
        public IQueryable<UserInfo> GetUserInfoes()
        {
            return db.UserInfoes;
        }

        /// <summary>
        /// The GetUserInfo.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [ResponseType(typeof(UserInfo))]
        public IHttpActionResult GetUserInfo(int id)
        {
            UserInfo userInfo = db.UserInfoes.Find(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return Ok(userInfo);
        }

        /// <summary>
        /// The PutUserInfo.
        /// </summary>
        /// <param name="userInfo">The userInfo<see cref="UserInfo"/>.</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserInfo(UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != userInfo.Id)
            //{
            //    return BadRequest();
            //}

            db.Entry(userInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserInfoExists(userInfo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// The PostUserInfo.
        /// </summary>
        /// <param name="userInfo">The userInfo<see cref="UserInfo"/>.</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [ResponseType(typeof(UserInfo))]
        public IHttpActionResult PostUserInfo(UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserInfoes.Add(userInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userInfo.Id }, userInfo);
        }

        /// <summary>
        /// The DeleteUserInfo.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [ResponseType(typeof(UserInfo))]
        [HttpDelete]
        public IHttpActionResult DeleteUserInfo(int id)
        {
            UserInfo userInfo = db.UserInfoes.Find(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            db.UserInfoes.Remove(userInfo);
            db.SaveChanges();

            return Ok(userInfo);
        }

        /// <summary>
        /// The SearchUser.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [ResponseType(typeof(UserInfo))]
        // POST: api/UserInfoes/key=?
        [HttpGet]
        public IHttpActionResult SearchUser(string key)
        {
            object user = null;
            if (key.IsEmpty())
            {
                user = db.UserInfoes;
            }
            else
            {
                user = db.UserInfoes.Where(u => u.username.Contains(key));
            };
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// The Login.
        /// </summary>
        /// <param name="user">The user<see cref="UserLogin"/>.</param>
        /// <returns>The <see cref="IHttpActionResult"/>.</returns>
        [HttpPost]
        [Route("api/Login")]
        public IHttpActionResult Login(UserLogin user)
        {
            var isLoginSuccess = db.UserInfoes.Any(_ => _.username == user.UserName && _.password == user.PassWord);

            return Ok(isLoginSuccess);
        }

        /// <summary>
        /// Defines the <see cref="UserLogin" />.
        /// </summary>
        public class UserLogin
        {
            /// <summary>
            /// Gets or sets the UserName.
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// Gets or sets the PassWord.
            /// </summary>
            public string PassWord { get; set; }
        }

        /// <summary>
        /// The Dispose.
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// The UserInfoExists.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool UserInfoExists(int id)
        {
            return db.UserInfoes.Count(e => e.Id == id) > 0;
        }
    }
}