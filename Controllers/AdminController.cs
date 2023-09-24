using Newtonsoft.Json;
using PixelSpot1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PixelSpot1.Controllers
{
    [EnableCors("*", "*", "*")]

    public class AdminController : ApiController
    {
        PixelSpotEntities db = new PixelSpotEntities();

        #region GetAdmin

        // GET: api/Admin
        [HttpGet]
        [Route("api/Admin/getAdmin")]
        public List<User> getAdmin()
        {
            return db.Users.Where(u => u.u_role == "ADMIN").ToList();
        }

        #endregion

       
        #region GetUsers

        // GET: api/Admin
        [HttpGet]
        [Route("api/Admin/getUsers")]

        public List<User> getUsers()
        {
             

            //var jsonSettings = new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};

            //// Serialize the success message using the configured settings
            //var jsonResponse = JsonConvert.SerializeObject(ls, jsonSettings);

            //// Create an HttpResponseMessage with the serialized JSON content
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");

            //// Return the HttpResponseMessage as an IHttpActionResult
            return  db.Users.Where(u => u.u_role == "CUSTOMER").ToList(); ;
        }

        #endregion


        #region userLogin
        // POST : api/Admin/Login

        [HttpPost]
        [Route("api/Admin/Login")]

        public IHttpActionResult Login(User credentials)
        {
            string email = credentials.u_email;
            string password = credentials.u_password;

            User user = db.Users.Where(u => u.u_email == email && u.u_password == password && u.u_role == "ADMIN")
                 .FirstOrDefault();


            if (user == null)
            {
                return NotFound();
            }

            // Create an object to hold both user data and response status
            var response = new
            {
                User = user,
                Message = "Login successful"
            };

            return Ok(response);
        }
        #endregion

        #region activeUsers
        [HttpGet]
        [Route("api/Admin/activeUsers")]
        public int activeUsers()
        {
            List<User>  U =  db.Users.Where(u => u.u_role == "CUSTOMER" && u.u_status == true).ToList();

            if(U == null || U.Count == 0) return 0;

            return U.Count;

        }

        #endregion

        #region getPhotoCount

        [HttpGet]
        [Route("api/Admin/getPhotoCount")]
        public int getPhotoCount()
        {
            int P = db.Photos.ToList().Count;

            return P;

        }

        #endregion

        #region getDownloadCount

        [HttpGet]
        [Route("api/Admin/getDownloadCount")]
        public int getDownloadCount()
        {
            int d = db.Downlaods.ToList().Count;

            return d;

        }
        #endregion


        #region getTotalUser

        [HttpGet]
        [Route("api/Admin/getTotalUser")]
        public int getTotalUser()
        {
            List<User> U = db.Users.Where(u => u.u_role == "CUSTOMER").ToList();

            if (U == null || U.Count == 0) return 0;

            return U.Count;

        }

        #endregion

        #region getSpamCount

        [HttpGet]
        [Route("api/Admin/getSpamCount")]
        public int getSpamCount()
        {
            int s = db.Spam_photos.ToList().Count;

            return s;

        }

        #endregion


        #region getSpam

        [HttpGet]
        [Route("api/Admin/getSpam")]
        public IHttpActionResult getSpam()
        {
            var s = db.Spam_photos.ToList();

            return Ok(s);

        }

        #endregion

        #region getLikeCount

        [HttpGet]
        [Route("api/Admin/getLikeCount")]
        public int getLikeCount()
        {
            int l = db.Liked_photos.ToList().Count;

            return l;

        }

        #endregion

        #region getCollectionCount
        [HttpGet]
        [Route("api/Admin/getCollectionCount")]
        public int getCollectionCount()
        {
            int cc = db.Collection_category.ToList().Count;

            return cc;

        }
        #endregion

        #region setCategory
        //Post : api/Admin/setCategory
        [HttpPost]
        [Route("api/Admin/setCategory")]
        public IHttpActionResult setPublic([FromBody] Photo_category reques)
        {

            if (reques.pc_name == null)
            {
                return Ok("faild to add category");
            }


           var Photo_category = db.Photo_category.Where(pc => pc.pc_name == reques.pc_name).FirstOrDefault();

            if (Photo_category == null)
            {
                var pc_name = reques.pc_name;


                Photo_category category = new Photo_category();

                category.pc_name = pc_name;

                db.Photo_category.Add(category);
                db.SaveChanges();

                return Ok("add category");
            }
            else
            { return Ok("already present "); }

        }

        #endregion

        #region delCategory

        //Post : api/Admin/delCategory
        [HttpPost]
        [Route("api/Admin/delCategory")]
        public IHttpActionResult delCategory([FromBody] Photo_category reques)
        {

            if (reques.pc_name == null || reques.pc_id <= 0)
            {
                return Ok("faild to add category");
            }


            var Photo_category = db.Photo_category.Where(pc => pc.pc_name == reques.pc_name && pc.pc_id == reques.pc_id).FirstOrDefault();

            if (Photo_category != null)
            {
               
                db.Photo_category.Remove(Photo_category);
                db.SaveChanges();

                return Ok("Remove category");
            }
            else
            { return Ok("not present "); }

        }

        #endregion


        #region delPhoto

        //Post : api/Admin/delCategory
        [HttpPost]
        [Route("api/Admin/delPhoto")]
        public IHttpActionResult delPhoto([FromBody] Photo reques)
        {

            if (reques.p_id <= 0) 
            { 
                return Ok("faild to add category");
            }


            Photo Ph = db.Photos.Where(p => p.pc_id == reques.p_id).FirstOrDefault();
            Spam_photos spam = db.Spam_photos.Where(sp=>sp.p_id==reques.p_id).FirstOrDefault();

            if (Ph != null && spam != null)
            {
             
                db.Spam_photos.Remove(spam);
                Ph.p_degree = false;
                db.SaveChanges();

                return Ok("Remove Photos");
            }
            else
            { return Ok("not present "); }

        }

        #endregion

    }
}
