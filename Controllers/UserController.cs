using PixelSpot1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Services.Description;

namespace PixelSpot1.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        PixelSpotEntities db = new PixelSpotEntities();


        #region GetUserByID
        // GET: api/User/getUserById/5
        [HttpGet]
        [Route("api/User/getUserById/{id}")]
        public IHttpActionResult getUserById(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var user = db.Users
                .Where(u => u.u_id == id)
                .Select(us => new
                {
                    us.u_first_name,
                    us.u_last_name,
                    us.u_email,
                    us.u_about
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        #endregion


        #region userLogin
        // POST : api/User/Login

        [HttpPost]
        [Route("api/User/Login")]

        public IHttpActionResult Login(User credentials)
        {
            string email = credentials.u_email;
            string password = credentials.u_password;
            int? userId = credentials.u_id;
            User user =  new User();

            if (email != null && password != null)
            {
                 user = db.Users.Where(u => u.u_email == email && u.u_password == password && u.u_role == "CUSTOMER")
                     .FirstOrDefault();
            }

            if (userId > 0 && email != null)
            {
                user = db.Users.Where(u => u.u_email == email && u.u_id == userId && u.u_role == "CUSTOMER")
                    .FirstOrDefault();
            }

            if (user != null)
            {
                user.u_status = true;
                db.SaveChanges();
            }
           

            if (user == null)
            {
                return NotFound();
            }

            // Create an object to hold both user data and response status
            var response = new
            {
                Login = true,

                u_id = user.u_id,
                u_first_name = user.u_first_name,
                u_last_name = user.u_last_name,
                u_email = user.u_email,
                u_address = user.u_address,
                u_mobile = user.u_mobile,
                u_about = user.u_about,

                Message = "Login successful"
            };

            return Ok(response);
        }
        #endregion


        #region userLogout

        [HttpGet]
        [Route("api/User/Logout/{id}")]

        public IHttpActionResult Logout(int id)
        {
           
            User user = db.Users.Where(u => u.u_id == id && u.u_role == "CUSTOMER")
                 .FirstOrDefault();

            user.u_status = false;
            db.SaveChanges();

            if (user == null)
            {
                return NotFound();
            }

            // Create an object to hold both user data and response status
          

            return Ok("logout");
        }

        #endregion


        #region RegisterUser

        // POST: api/User
        [HttpPost]
        [Route("api/User/Register")]
        public IHttpActionResult Register([FromBody] User user)
        {

            db.Users.Add(user);
            int count = db.SaveChanges();


            if (count == 0)
            {
                return NotFound();
            }


            var response = new { Message = "Registration successful" };


            return Ok(response);
        }

        #endregion


        #region UpdateUserByID
        // PUT: api/User/5
        [HttpPut]
        [Route("api/User/UpdateUser")]
        public IHttpActionResult UpdateUser([FromBody] User user)
        {
            User usertoChange = db.Users.Find(user.u_id);
            usertoChange.u_first_name = user.u_first_name;
            usertoChange.u_last_name = user.u_last_name;
            usertoChange.u_about = user.u_about;
            usertoChange.u_mobile = user.u_mobile;
            usertoChange.u_address = user.u_address;

            int count = db.SaveChanges();

            if (count == 0)
            {
                return NotFound();
            }


            var response = new { Message = "Update successful" };


            return Ok(response);
        }
        #endregion

        #region updatePassword
        [HttpPut]
        [Route("api/User/updatePassword")]
        public IHttpActionResult updatePassword([FromBody] User user) 
        {
            int uId = user.u_id;
            String oldPass = user.u_first_name;
            String newPass = user.u_password;

            if(uId <= 0  ||oldPass == null || newPass == null)
            {
                return Ok("Update failed");
            }

            User U = db.Users.Where( u => u.u_password == oldPass && u.u_id == uId).FirstOrDefault();
            
            if (U != null) 
            {
               U.u_password = newPass;
               db.SaveChanges();

               return Ok("update Password Successfull");

            }

            return Ok("Update failed");

        }

        #endregion

        #region updateEmail
        [HttpPut]
        [Route("api/User/updateEmail")]
        public IHttpActionResult updateEmail([FromBody] User user)
        {
            int uId = user.u_id;
            String oldEmail = user.u_first_name;
            String newEmail = user.u_email;
            String Password = user.u_password;

            if (uId <= 0 || oldEmail == null || newEmail == null)
            {
                return Ok("Update Email failed");
            }

            User U = db.Users.Where(u => u.u_email == oldEmail && u.u_id == uId && u.u_password == Password).FirstOrDefault();

            if (U != null)
            {
                U.u_email = newEmail;
                db.SaveChanges();

                return Ok("update Email Successfull");

            }

            return Ok("Update Email failed");

        }

        #endregion


        #region setSpamPhoto
        [HttpPost]
        [Route("api/User/setSpamPhoto")]
        public IHttpActionResult setSpamPhoto([FromBody] Spam_photos spam)
        {
            int pId = spam.p_id;
            String details = spam.sp_details;
          

            if (pId <= 0 || details == null)
            {
                return Ok("set SpamPhoto failed");
            }

            Spam_photos sp = new Spam_photos();
            sp.p_id = pId;
            sp.sp_details = details;
             
            db.Spam_photos.Add(sp);
            db.SaveChanges();
           
            return Ok("setSpamPhoto");

        }

        #endregion


        #region DeleteUser
        // DELETE: api/User/DeleteUser/5
        [HttpDelete]
        [Route("api/User/DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            User usertoRemove = db.Users.Find(id);
            if (usertoRemove != null)
            {
                usertoRemove.User_collection.Clear();
                db.Users.Remove(usertoRemove);
                int count = db.SaveChanges();

                if (count == 1)
                {
                    var response = new { Message = "Deleted" };
                    return Ok(response);

                }

            }
            return NotFound();
        }
        #endregion

    }
}
