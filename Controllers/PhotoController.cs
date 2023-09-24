using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PixelSpot1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;

namespace PixelSpot1.Controllers
{
    [EnableCors("*", "*", "*")]

    public class PhotoController : ApiController
    {
        PixelSpotEntities db = new PixelSpotEntities();

       // const String domain = "http://PixelSpot.myproject.com.pl/image/";
        const String domain = "http://localhost:54610/image/";


        #region downloadPhoto
        // GET: api/Photo/downloadPhoto?imageName=boy.jpg

        [HttpGet]
        [Route("api/Photo/downloadPhoto")]
        public  IHttpActionResult DownloadPhoto(string imageName)
        {
            // Get the directory path where photos are stored
            string photoDirectory = HttpContext.Current.Server.MapPath("~/image");

            // Combine the directory path with the requested image name
            string imagePath = Path.Combine(photoDirectory, imageName);

            // Check if the requested image file exists
            if (File.Exists(imagePath))
            {
                // Create an HttpResponseMessage to return the file
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                // Read the image file into a Stream
                FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                response.Content = new StreamContent(fileStream);

                // Set the content type based on the file extension
                string contentType = GetContentType(imageName);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                
                // Create a response message with the file content
                return ResponseMessage(response);
            }

            // If the image file does not exist, return a 404 Not Found response
            return NotFound();
        }

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            switch (extension.ToLower())
            {
                case ".jpg":
                    return "image/jpg";
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                // Add more cases for other image formats as needed
                default:
                    return "application/octet-stream"; // Default to binary data
            }
        }

        #endregion


        #region GetAllPublicPhotos
        // GET: api/Photo/getPhotos
        [HttpGet]
        [Route("api/Photo/getPhotos")]
        public IHttpActionResult getPhotos()
        {
            var photos = db.Photos.Where(p => p.p_state == true & p.p_degree == true).OrderByDescending(p => p.p_datetime)
                             .Select(p => new
                             {
                                 p_id = p.p_id,
                                 p_name = domain + p.p_name,
                                 p_state = p.p_state,
                                 p_degree = p.p_degree,
                                 u_id = p.u_id,
                                 uc_id = p.uc_id,
                                 pc_id = p.pc_id,

                             })
                             .ToList();

            return Ok(photos);
        }

        #endregion

        #region GetPhotoByUserId
        [HttpGet]
        [Route("api/Photo/GetPhotoByUserId/{id}")]
        public IHttpActionResult GetPhotoByUserId(int id)
        {
            var photos = db.Photos.Where(p => p.p_degree == true && p.u_id == id).OrderByDescending(p => p.p_datetime)
                             .Select(p => new
                             {
                                 p_id = p.p_id,
                                 p_name = domain + p.p_name,
                                 p_state = p.p_state,
                                 p_degree = p.p_degree,
                                 u_id = p.u_id,
                                 uc_id = p.uc_id,
                                 pc_id = p.pc_id,

                             })
                             .ToList();

            return Ok(photos);
        }


        #endregion


        #region GetPhotoPublicByUserId
        [HttpGet]
        [Route("api/Photo/GetPhotoPublicByUserId/{id}")]
        public IHttpActionResult GetPhotoPublicByUserId(int id)
        {
            var photos = db.Photos.Where(p => p.p_degree == true && p.u_id == id && p.p_state == true).OrderByDescending(p => p.p_datetime)
                             .Select(p => new
                             {
                                 p_id = p.p_id,
                                 p_name = domain + p.p_name,
                                 p_state = p.p_state,
                                 p_degree = p.p_degree,
                                 u_id = p.u_id,
                                 uc_id = p.uc_id,
                                 pc_id = p.pc_id,

                             })
                             .ToList();

            return Ok(photos);
        }


        #endregion



        #region GetPhotoPublicById
        [HttpGet]
        [Route("api/Photo/GetPhotoPublicById/{id}")]
        public IHttpActionResult GetPhotoPublicById(int id)
        {
            var photos = db.Photos.Where(p => p.p_degree == true && p.p_id == id && p.p_state == true).OrderByDescending(p => p.p_datetime)
                             .Select(p => new
                             {
                                 p_id = p.p_id,
                                 p_name = domain + p.p_name,
                                 p_state = p.p_state,
                                 p_degree = p.p_degree,
                                 u_id = p.u_id,
                                 uc_id = p.uc_id,
                                 pc_id = p.pc_id,

                             })
                             .ToList();

            return Ok(photos);
        }


        #endregion



        #region popular
        [HttpGet]
        [Route("api/Photo/popular/{id}")]
        public IHttpActionResult popular(int id)
        {
            var ids = db.Liked_photos.Where(l => l.u_id == id)
                             .Select(l => l.p_id)
                             .ToList();
            List<Photo> photo = new List<Photo>();
            foreach (var item in ids)
            {
                Photo ph = db.Photos.Where(p => p.p_id == item).FirstOrDefault();
                ph.p_name = domain + ph.p_name;
                photo.Add(ph);
            }

            return Ok(photo);
        }
        #endregion

        #region GetPhotosByPhotoCategoryID
        // GET: api/Photo/photoCategoryById/5

        [HttpGet]
        [Route("api/Photo/photoCategoryById/{id}")]
        public IHttpActionResult photoCategoryById(int Id)
        {   //photo by photo_category ID;

            var photosByCId = db.Photos.Where(p => p.pc_id == Id & p.p_state == true & p.p_degree == true).OrderByDescending(p => p.p_datetime)
                               .Select(p => new
                               {
                                   p_id = p.p_id,
                                   p_name = domain + p.p_name,
                                   p_state = p.p_state,
                                   p_degree = p.p_degree,
                                   u_id = p.u_id,
                                   uc_id = p.uc_id,
                                   pc_id = p.pc_id,

                               }).ToList();

            return Ok(photosByCId);
        }
        #endregion


        #region GetPhotoByCategoryName
        // GET: api/Photo/photoCategoryName?categoryName=name

        [HttpGet]
        [Route("api/Photo/photoCategoryName")]
        public IHttpActionResult photoCategoryName(String categoryName)
        {   //photo by photo_category categoryName;

            int pcId = db.Photo_category
                         .Where(pc => pc.pc_name == categoryName)
                         .Select(pc => pc.pc_id)
                         .FirstOrDefault();

            var photoByCName = db.Photos.Where(p => p.pc_id == pcId & p.p_state == true & p.p_degree == true)
                                .OrderByDescending(p => p.p_datetime)
                                 .Select(p => new
                                 {
                                     p_id = p.p_id,
                                     p_name = domain + p.p_name,
                                     p_state = p.p_state,
                                     p_degree = p.p_degree,
                                     u_id = p.u_id,
                                     uc_id = p.uc_id,
                                     pc_id = p.pc_id,

                                 }).ToList();
            return Ok(photoByCName);

        }

        #endregion


        #region GetPhotoByUserCollection
        // GET: api/Photo/photoUserCollection?uId=uId&ccId=ccId

        [HttpGet]
        [Route("api/Photo/photoUserCollection")]
        public IHttpActionResult photoUserCollection(int uId, int ccId)
        {   //photo by userID and Collection_categoryId;


            List<int> listOfUserCollectionIds = db.User_collection
                                               .Where(uc => uc.u_id == uId && uc.cc_id == ccId)
                                               .Select(uc => uc.uc_id)
                                               .ToList();

            var photosByUcollection = db.Photos
                                .Where(p => listOfUserCollectionIds.Contains((Int32)p.uc_id))
                                .OrderByDescending(p => p.p_datetime)
                                .Select(p => new
                                {
                                    p_id = p.p_id,
                                    p_name = domain + p.p_name,
                                    p_state = p.p_state,
                                    p_degree = p.p_degree,
                                    u_id = p.u_id,
                                    uc_id = p.uc_id,
                                    pc_id = p.pc_id,

                                }).ToList();


            return Ok(photosByUcollection);
        }
        #endregion

        #region GetDownlaodPhotoById
        [HttpGet]
        [Route("api/Photo/GetDownlaodPhotoById/{id}")]
        public IHttpActionResult GetDownlaodPhotoById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            List<Photo> photo = new List<Photo>();

            List<int?> ids = db.Downlaods.Where(d => d.u_id == id)
                             .Select(d => d.p_id)
                             .ToList();


            if (ids.Count == 0)
            { return BadRequest(); }

            // Fetch the required data from the database
            var photoData = db.Photos
                .Where(i => ids.Contains(i.p_id))
                .OrderByDescending(p => p.p_datetime)
                .Select(p => new
                {
                    p.p_id,
                    p.p_name,
                    p.p_state,
                    p.p_degree,
                    p.u_id,
                    p.uc_id,
                    p.pc_id,
                })
                .ToList();

            // Transform the fetched data into PhotoDTO instances
            foreach (var item in photoData)
            {
                var p = new Photo
                {
                    p_id = item.p_id,
                    p_name = domain + item.p_name,
                    p_state = item.p_state,
                    p_degree = item.p_degree,
                    u_id = item.u_id,
                    uc_id = item.uc_id,
                    pc_id = item.pc_id,
                };
                photo.Add(p);
            }

            return Ok(photo);

        }
        #endregion


        #region PostLikePhoto
        // POST: api/Photo/postLike
        [HttpPost]
        [Route("api/Photo/postLike")]
        public IHttpActionResult postLike([FromBody] Liked_photos request)
        {
            int userId = request.u_id;
            int photoId = request.p_id;

            User user = db.Users.Find(userId);
            Photo photo = db.Photos.Find(photoId);

            if (user == null || photo == null)
            {
                return NotFound(); // Return a 404 response if user or photo not found
            }
            else
            {
                Liked_photos liked_Photos = new Liked_photos();

                liked_Photos.u_id = userId;
                liked_Photos.p_id = photoId;

                // Add the record to the Liked_photos table
                db.Liked_photos.Add(liked_Photos);

                // Save changes to the database
                db.SaveChanges();

                return Ok("Photo liked successfully");
            }
        }



        #endregion


        #region DeleteLikePhoto
        // POST: api/Photo/deleteLike
        [HttpPost]
        [Route("api/Photo/deleteLike")]
        public IHttpActionResult deleteLike([FromBody] Liked_photos request)
        {
            int userId = request.u_id;
            int photoId = request.p_id;

            User user = db.Users.Find(userId);
            Photo photo = db.Photos.Find(photoId);

            if (user == null || photo == null)
            {
                return NotFound(); // Return a 404 response if user or photo not found
            }
            else
            {
                Liked_photos liked_Photos = new Liked_photos();

                liked_Photos.u_id = userId;
                liked_Photos.p_id = photoId;

                Liked_photos plId = db.Liked_photos.Where(p => p.u_id == userId && p.p_id == photoId).FirstOrDefault();

                if (plId != null)
                {
                    // Add the record to the Liked_photos table
                    db.Liked_photos.Remove(plId);

                    // Save changes to the database
                    db.SaveChanges();

                    return Ok("Photo Unliked successfully");
                }
                return NotFound();
            }
        }

        #endregion


        #region GetLikePhoto
        //GET: api/Photo/getLike/1

        [HttpGet]
        [Route("api/Photo/getLike/{id}")]
        public IHttpActionResult getLike(int id)
        {
            int userId = id;

            List<Int32> likedPhotos = db.Liked_photos.Where(lp => lp.u_id == userId)
                            .Select(lp => lp.p_id)
                            .ToList();

            var response = new {
                list = likedPhotos,
                message = "list of liked Photos"
            };



            return Ok(response);

        }

        #endregion


        #region UploadPhoto


        [HttpPost]
        [Route("api/Photo/UploadPhoto/{userId}")]
        public async Task<IHttpActionResult> UploadPhoto(int userId) //List<HttpPostedFileBase> file
        {
            try
            {
                // Check if the request contains a file
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return BadRequest("Unsupported media type");
                }

                var provider = new MultipartMemoryStreamProvider();

                // Read the form data and files
                await Request.Content.ReadAsMultipartAsync(provider);

                // Assuming you have a 'files' collection in the form data


                if (provider.Contents != null)
                {
                    foreach (var file in provider.Contents)
                    {
                        var fileBytes = await file.ReadAsByteArrayAsync();
                        var fileName = $"{userId}_{DateTime.Now:yyyyMMddHHmmssfff}_{file.Headers.ContentDisposition.FileName.Trim('\"')}";
                        var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/image"), fileName);

                        File.WriteAllBytes(filePath, fileBytes);

                        Photo photoUpload = new Photo();
                        photoUpload.u_id = userId;
                        photoUpload.p_name = fileName;
                        photoUpload.p_state = false;
                        photoUpload.p_degree = true;
                        photoUpload.p_datetime = DateTime.Now;

                        db.Photos.Add(photoUpload);
                        try
                        {
                            db.SaveChanges();
                            return Ok("Photo uploaded and saved successfully");
                        }
                        catch (Exception saveException)
                        {
                            // Handle the exception related to SaveChanges
                            return InternalServerError(saveException);
                        }

                    }



                    return Ok("Photo uploaded and saved successfully");
                }
                else
                {
                    return BadRequest("No file uploaded");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return InternalServerError(ex);
            }
        }


        #endregion


        #region setPublic
        //Post : api/Photo/setPublic
        [HttpPost]
        [Route("api/Photo/setPublic")]
        public IHttpActionResult setPublic([FromBody] Liked_photos reques)
        {
            var p_id = reques.p_id;
            var u_id = reques.u_id;

            Photo photos = db.Photos.Where(p => p.p_id == p_id && p.p_state == false && p.u_id == u_id)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            if (photos != null)
            {
                photos.p_state = true;

                db.SaveChanges();

                return Ok("updated");
            }

            return Ok("faild to updated");

        }



        #endregion

        #region setPrivate

        //Post : api/Photo/setPrivate
        [HttpPost]
        [Route("api/Photo/setPrivate")]
        public IHttpActionResult setPrivate([FromBody] Liked_photos reques)
        {
            var p_id = reques.p_id;
            var u_id = reques.u_id;

            Photo photos = db.Photos.Where(p => p.p_id == p_id && p.p_state == true && p.u_id == u_id)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            if (photos != null)
            {
                photos.p_state = false;

                db.SaveChanges();

                return Ok("updated");
            }

            return Ok("faild to updated");

        }

        #endregion


        #region deletePhoto

        [HttpPost]
        [Route("api/Photo/deletePhoto")]
        public IHttpActionResult deletePhoto([FromBody] Liked_photos reques)
        {
            var p_id = reques.p_id;
            var u_id = reques.u_id;

            Photo photos = db.Photos.Where(p => p.p_id == p_id && p.u_id == u_id)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            if (photos != null)
            {
                photos.p_state = false;
                photos.p_degree = false;
                photos.uc_id = null;
                photos.pc_id = null;


                db.SaveChanges();

                return Ok("updated");
            }

            return Ok("faild to updated");

        }
        #endregion

        #region backupPhoto
        [HttpPost]
        [Route("api/Photo/backupPhoto")]
        public IHttpActionResult backupPhoto([FromBody] Liked_photos reques)
        {

            var u_id = reques.u_id;

            List<Photo> photos = db.Photos.Where(p => p.u_id == u_id && p.p_degree == false)
                           .OrderByDescending(p => p.p_datetime)
                           .ToList();

            if (photos != null)
            {
                foreach (var item in photos)
                {
                    item.p_state = false;
                    item.p_degree = true;
                    item.uc_id = null;
                    item.pc_id = null;


                    db.SaveChanges();

                }



                return Ok("updated");
            }

            return Ok("faild to updated");

        }
        #endregion



        #region setCollection
        // POST: api/Photo/setCollection
        [HttpPost]
        [Route("api/Photo/setCollection/{userId}")]
        public IHttpActionResult setCollection([FromBody] Collection_category reques, int userId)
        {

            if (reques.cc_name == null || userId <= 0)
            {
                return Ok("faild to add category");
            }


            int CollectionCategoryId = db.Collection_category.Where(cc => cc.cc_name == reques.cc_name)
                                        .Select(cc => cc.cc_id)
                                        .FirstOrDefault();

            var userCollection = db.User_collection.Where(uc => uc.cc_id == CollectionCategoryId && uc.u_id == userId).FirstOrDefault();

            if (userCollection == null)
            {

                Collection_category cc = new Collection_category();

                cc.cc_name = reques.cc_name;

                db.Collection_category.Add(cc);
                db.SaveChanges();

                int ccId = db.Collection_category.Where(c => c.cc_name == reques.cc_name)
                                     .Select(c => c.cc_id).FirstOrDefault();

                User_collection uc = new User_collection();
                uc.u_id = userId;
                uc.cc_id = ccId;

                db.User_collection.Add(uc);
                db.SaveChanges();

                return Ok("add collection");
            }
            else
            { return Ok("already present "); }

        }

        #endregion

        #region removeCollection
        // POST: api/Photo/removeCollection
        [HttpPost]
        [Route("api/Photo/removeCollection/{userId}")]
        public IHttpActionResult removeCollection([FromBody] Collection_category reques, int userId)
        {

            if (reques.cc_name == null || userId <= 0 )
            {
                return Ok("faild to remove category");
            }


            int CollectionCategoryId = db.Collection_category.Where(cc => cc.cc_name == reques.cc_name)
                                        .Select(cc => cc.cc_id)
                                        .FirstOrDefault();

            var userCollection = db.User_collection.Where(uc => uc.cc_id == CollectionCategoryId && uc.u_id == userId).FirstOrDefault();

            if (userCollection != null)
            {


                Collection_category cc = db.Collection_category.Where(c => c.cc_name == reques.cc_name && c.cc_id == CollectionCategoryId)
                                         .FirstOrDefault();



                db.Collection_category.Remove(cc);
                db.SaveChanges();

                return Ok("remove collection");
            }
            else
            { return Ok("not present "); }

        }
        #endregion


        #region setPhotoToCollection
        //Post : api/Photo/setPhotoToCollection/1
        [HttpPost]
        [Route("api/Photo/setPhotoToCollection/{pId}")]
        public IHttpActionResult setPhotoToCategory([FromBody] Collection_category reques, int pId)
        {
            int cc_id = reques.cc_id;
            var cc_name = reques.cc_name;

            if (cc_name == null || cc_id <= 0  || pId <= 0)
            {
                return Ok("faild to set PhotoToCollection");
            }

            Photo photos = db.Photos.Where(p => p.p_id == pId)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            User_collection userCollection = db.User_collection.Where(uc => uc.cc_id == cc_id)
                                              .FirstOrDefault();

            if (photos != null)
            {
                photos.uc_id = userCollection.uc_id;
                db.SaveChanges();

                return Ok("Set PhotoToCollection Successfully ");
            }

            return Ok("faild to Set");

        }
        #endregion

        #region removePhotoFromCollection
        //Post : api/Photo/removePhotoFromCollection
        [HttpPost]
        [Route("api/Photo/removePhotoFromCollection")]
        public IHttpActionResult removePhotoFromCollection()
        {
            String requestBody = Request.Content.ReadAsStringAsync().Result;

            JObject o = JObject.Parse(requestBody);

            int pId = o["pId"].Value<Int32>();

            if (pId <= 0 )
            {
                return Ok("faild to remove PhotoFromCollection");
            }

            Photo photos = db.Photos.Where(p => p.p_id == pId)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            //Photo photos = db.Photos.FirstOrDefault(p => p.p_id == pId);

            if (photos != null)
            {
                photos.uc_id = null;
                db.SaveChanges();

                return Ok("Remove PhotoFromCollection Successfully ");
            }

            return Ok("faild to Remove");

        }
        #endregion


        #region setPhotoToCategory
        //Post : api/Photo/setPhotoToCategory/1
        [HttpPost]
        [Route("api/Photo/setPhotoToCategory/{pId}")]
        public IHttpActionResult setPhotoToCategory([FromBody] Photo_category reques, int pId)
        {
            int pc_id = reques.pc_id;
            var pc_name = reques.pc_name;

            if (pc_name == null || pc_id <= 0 || pId <= 0)
            {
                return Ok("faild to set PhotoToCategory");
            }

            Photo photos = db.Photos.Where(p => p.p_id == pId)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            if (photos != null)
            {
                photos.pc_id = pc_id;
                photos.p_state = true;

                db.SaveChanges();

                return Ok("Set PhotoToCategory Successfully ");
            }

            return Ok("faild to Set");

        }


        #endregion

        #region removePhotoFromCategory
        //Post : api/Photo/removePhotoFromCategory
        [HttpPost]
        [Route("api/Photo/removePhotoFromCategory")]
        public IHttpActionResult removePhotoFromCategory([FromBody] Photo reques)
        {
            int p_id = reques.p_id;


            if (p_id <= 0)
            {
                return Ok("faild to remove PhotoToCategory");
            }

            Photo photos = db.Photos.Where(p => p.p_id == p_id)
                           .OrderByDescending(p => p.p_datetime)
                           .FirstOrDefault();

            if (photos != null)
            {
                photos.pc_id = null;

                db.SaveChanges();

                return Ok("remove PhotoToCategory Successfully ");
            }

            return Ok("faild to remove");
        }
        #endregion


        #region getCollectionByUserId
        // GET: api/Photo/getCollectionByUserId/1
        [HttpGet]
        [Route("api/Photo/getCollectionByUserId/{userId}")]
        public IHttpActionResult getCollectionByUserId(int userId)
        {
            var userCollection = db.User_collection.Where(uc => uc.u_id == userId)
                                          .Select(uc => uc.cc_id).ToList();

            List<Collection_category> listUsercollection = new List<Collection_category>();

            foreach (var cc_id in userCollection)
            {

                Collection_category c = db.Collection_category.Where(cc => cc.cc_id == cc_id)
                                        .FirstOrDefault();
                if (c != null)
                {
                    listUsercollection.Add(c);
                }

            }
            var res = new
            {

                list = listUsercollection,
                message = "complete"

            };

            if (listUsercollection == null || listUsercollection.Count == 0)
            {
                return Ok("No collection present");
            }

            return Ok(res);
        }

        #endregion

        #region getAllCategory
        // GET: api/Photo/getAllCategory
        [HttpGet]
        [Route("api/Photo/getAllCategory")]

        public IHttpActionResult getAllCategory()
        {
            var photos = db.Photo_category.ToList();

            return Ok(photos);
        }

        #endregion


        #region setProfile

        [HttpPost]
        [Route("api/Photo/setProfile")]
        public IHttpActionResult setProfile([FromBody] Photo ph )
        {
          int? uid =  ph.u_id;
          int? pid = ph.p_id;

            if( uid == null && pid == null ) { return BadRequest(); }

            Photo pr = db.Photos.Where(p => p.u_id == uid && p.p_profilePhoto == true).FirstOrDefault();
           
            if( pr != null )
            {
                pr.p_profilePhoto = false;
                db.SaveChanges();
            }

            Photo spr = db.Photos.Where(p => p.u_id == uid && p.p_id == pid ).FirstOrDefault();

            if (spr != null)
            {
                spr.p_profilePhoto = true;
                db.SaveChanges();
            }

            return Ok("Profile Set");
        }

        #endregion


        #region getProfile
        [HttpGet]
        [Route("api/Photo/getProfile/{id}")]
        public IHttpActionResult getProfile(int id)
        {
           if( id <= 0 )
            {
                return BadRequest();
            }

        


            var pr = db.Photos.Where(p => p.u_id == id && p.p_profilePhoto == true).OrderByDescending(p => p.p_datetime)
                           .Select(p => new
                           {
                               p_id = p.p_id,
                               p_name = domain + p.p_name,
                               p_state = p.p_state,
                               p_degree = p.p_degree,
                               u_id = p.u_id,
                               uc_id = p.uc_id,
                               pc_id = p.pc_id,

                           })
                           .ToList();


            if (pr == null )
            {
                return NotFound();
            }

            return Ok(pr);
        }
        #endregion


    }
}
