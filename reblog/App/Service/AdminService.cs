using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using reblog.App.Domain;
using reblog.App.Repository;
using reblog.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace reblog.App.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository repository;
        private Password password;

        public AdminService(IAdminRepository repository)
        {
            password = new Password();
            this.repository = repository;
        }

        public User Get(User user)
        {
            return repository.Get(user);
        }

        public User Login(LoginModel login)
        {
            var user = new User();
            if (login.UserName.Contains('@'))
                user.Email = login.UserName;
            else
                user.Nick = login.UserName;

            var loggeduser = repository.Get(user);

            if (loggeduser == null || password.ValidatePassword(login.Password, loggeduser.Password) == false)
                throw new Exception("Usuário ou Senha incorretos");

            return loggeduser;
        }

        public User Register(RegisterModel register)
        {
            var user = new User
            {
                Email = register.Email,
                Name = register.Name,
                Nick = register.Nick,
            };
            user.Password = password.CreateHash(register.Password);
            repository.Save(user);
            return user;
        }

        public List<Owner> Owners()
        {
            return repository.Owners();
        }

        public List<Tag> Tags()
        {
            return repository.Tags();
        }

        public List<Category> Categories()
        {
            return repository.Categories();
        }

        public Post CreatePost(Post post, HttpPostedFileBase image, string tags)
        {
            if (post.Tags == null)
                post.Tags = new List<Tag>();

            var ts = tags.Split(',');
            foreach (var t in ts)
            {
                var tagname = t.TrimEnd().TrimStart();
                var newTag = new Tag { Name = tagname };
                var tagexists = repository.TagExists(newTag);
                if (tagexists == false)
                    repository.Save(newTag);
                else
                    newTag = repository.Get(newTag);
                    

                post.Tags.Add(newTag);
            }

            post = repository.Save(post);

            try
            {
                var date = post.Date.ToString("dd-MM-yyyy");
                var folder = HttpContext.Current.Server.MapPath("~/Uploads/" +  date);

                if (Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                var path = "~/Uploads/" + date + "/temp-" + post.Id + Path.GetExtension(image.FileName);
                var filePath = HttpContext.Current.Server.MapPath(path);

                Bitmap originalBMP = new Bitmap(image.InputStream);

                Bitmap newBMP = new Bitmap(originalBMP, originalBMP.Width, originalBMP.Height);
                Graphics oGraphics = Graphics.FromImage(newBMP);

                oGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                oGraphics.DrawImage(originalBMP, 0, 0, originalBMP.Width, originalBMP.Height);

                newBMP.Save(filePath);

                originalBMP.Dispose();
                newBMP.Dispose();
                oGraphics.Dispose();
            }
            catch
            {
            }
            return post;
        }

        public void CropImage(long postid, DateTime postdate, int x, int y, int w, int h)
        {
            if (w == 0 && h == 0)
                throw new Exception("A crop selection was not made.");

            ModifyImage(postid, postdate, x, y, w, h);
        }

        public void CreateCategory(Category category)
        {
            repository.Save(category);
        }

        public Category CreateCategory(string category)
        {
            return repository.Save(new Category { Name = category });
        }

        public void CreateTag(Tag tag)
        {
            repository.Save(tag);
        }

        public void CreateOwner(Owner owner)
        {
            repository.Save(owner);
        }

        public Owner CreateOwner(string owner)
        {
            return repository.Save(new Owner { Name = owner });
        }

        private void ModifyImage(long postid, DateTime postdate, int x, int y, int w, int h)
        {
            var imagepath = HttpContext.Current.Server.MapPath("~/Uploads/" + postdate.ToString("dd-MM-yyyy") + "/temp-" + postid + ".jpg");
            Image img = Image.FromFile(imagepath);

            using (System.Drawing.Bitmap _bitmap = new System.Drawing.Bitmap(w, h))
            {
                _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    _graphic.DrawImage(img, 0, 0, w, h);
                    _graphic.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);



                    using (EncoderParameters encoderParameters = new EncoderParameters(1))
                    {
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                        imagepath = imagepath.Replace("temp-", "");
                        _bitmap.Save(imagepath, GetImageCodec(".jpg"), encoderParameters);
                    }
                }
            }
        }

        private ImageCodecInfo GetImageCodec(string extension)
        {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.Contains(extension))
                {
                    return codec;
                }
            }
            return codecs[1];
        }

        private class Password
        {
            private const int SALT_BYTE_SIZE = 24;
            private const int HASH_BYTE_SIZE = 24;
            private const int PBKDF2_ITERATIONS = 1000;

            private const int ITERATION_INDEX = 0;
            private const int SALT_INDEX = 1;
            private const int PBKDF2_INDEX = 2;

            public string CreateHash(string password)
            {
                RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                byte[] salt = new byte[SALT_BYTE_SIZE];
                csprng.GetBytes(salt);

                byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
                return PBKDF2_ITERATIONS + ":" +
                    Convert.ToBase64String(salt) + ":" +
                    Convert.ToBase64String(hash);
            }

            public bool ValidatePassword(string password, string correctHash)
            {
                try
                {
                    char[] delimiter = { ':' };
                    string[] split = correctHash.Split(delimiter);
                    int iterations = Int32.Parse(split[ITERATION_INDEX]);
                    byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
                    byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

                    byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
                    return SlowEquals(hash, testHash);
                }
                catch
                {
                    return false;
                }
            }

            private bool SlowEquals(byte[] a, byte[] b)
            {
                uint diff = (uint)a.Length ^ (uint)b.Length;
                for (int i = 0; i < a.Length && i < b.Length; i++)
                    diff |= (uint)(a[i] ^ b[i]);
                return diff == 0;
            }

            private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}