namespace PhotoContest.Infrastructure.Services
{
    using PhotoContest.Data.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;
    using File = Google.Apis.Drive.v2.Data.File;
    public class PictureService : BaseService
    {
        public PictureService(IPhotoContestData data) : base(data)
        {
        }

        private const int MaxImageSize = 1000000;
        private const string GoogleDriveFolderId = "0By2WSCXLYL1JNVJacmZvcXROeVk";

        public string[] UploadImageToGoogleDrive(HttpPostedFileBase file, string fileName, string fileType)
        {
            string mediaType = fileType;
            byte[] byteArray = Convert.FromBase64String(this.GetBase64String(file));
            MemoryStream stream = new MemoryStream(byteArray);

            var service = GoogleDriveService.Get();

            Google.Apis.Drive.v2.Data.File body = new File
            {
                Title = fileName,
                MimeType = mediaType,
                Parents = new List<ParentReference>
                        {
                            new ParentReference
                                {
                                    Id = GoogleDriveFolderId
                                }
                        },
                Permissions = new List<Permission>()
                {
                    new Permission() { Type = "anyone", Role = "reader", WithLink = true }
                }
            };

            try
            {
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, mediaType);
                request.Upload();

                return new[] { "success", request.ResponseBody.Id };
            }
            catch (Exception exception)
            {
                return new[] { "error", string.Format("Something happened.\r\n" + exception.Message) };
            }
        }

        public string[] DeleteImageFromGoogleDrive(string fileId)
        {
            var service = GoogleDriveService.Get();

            try
            {
                service.Files.Delete(fileId).Execute();
                return new[] { "success" };
            }
            catch (Exception e)
            {
                return new[] { "error", string.Format("Something happened.\r\n" + e.Message) };
            }
        }

        public string ValidateImageData(HttpPostedFileBase file)
        {
            if (!file.ContentType.Contains("image"))
            {
                return "The file is not a picture";
            }

            if (file.ContentLength > MaxImageSize)
            {
                return "Picture size must be in range [1 - 1024 kb]";
            }

            return null;
        }

        public string GetBase64String(HttpPostedFileBase file)
        {
            byte[] fileBuffer = new byte[file.ContentLength];
            file.InputStream.Read(fileBuffer, 0, file.ContentLength);

            return Convert.ToBase64String(fileBuffer);
        }
    }
}