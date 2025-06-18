using System.Net.Mail;

namespace PRA_project.DataSaver
{
    public class GetProfilePicture
    {
        public static string GetProfilePicturePath(string emailAddress)
        {
            string basePath = @"C:\Users\Korisnik\Documents\Faks\Algebra_2_G\PRA\PRA_projekt_službeno\PRA_project\PRA_project\ProfilePictureRepo";
            string fileName = $"{emailAddress}_profilepicture.jpg";
            string fullPath = Path.Combine(basePath, fileName);

            if (File.Exists(fullPath))
            {
                return fullPath;
            }

            return Path.Combine(basePath, "default_picture.jpg");
        }
    }
}
