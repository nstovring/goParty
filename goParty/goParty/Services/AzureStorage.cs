﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using goParty.Helpers;
using Xamarin.Forms;

namespace goParty.Services
{

    public enum ContainerType
    {
        Image,
        Text
    }

    public static class AzureStorage
    {
        static CloudBlobContainer GetContainer(ContainerType containerType)
        {
            var account = CloudStorageAccount.Parse(Locations.StorageConnection);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(containerType.ToString().ToLower());
        }

        public static async Task<IList<string>> GetFilesListAsync(ContainerType containerType)
        {
            var container = GetContainer(containerType);

            var allBlobsList = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobsList.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);

            return allBlobsList;
        }

        public static async Task<byte[]> GetFileAsync(ContainerType containerType, string name)
        {
            var container = GetContainer(containerType);

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public static async Task<Stream> GetFileFromStreamAsync(ContainerType containerType, string name)
        {
            var container = GetContainer(containerType);
            var blob = container.GetBlobReference(name);

            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                var memoryStream = new MemoryStream();
                using (memoryStream)
                {
                    await blob.DownloadToStreamAsync(memoryStream);
                }
                return memoryStream;
            }
            return null;
        }

        public static async Task<string> UploadFileAsync(ContainerType containerType, Stream stream)
        {
            var container = GetContainer(containerType);
            await container.CreateIfNotExistsAsync();
            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);

            return name;
        }

        public static async Task<bool> DeleteFileAsync(ContainerType containerType, string name)
        {
            var container = GetContainer(containerType);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public static async Task<bool> DeleteContainerAsync(ContainerType containerType)
        {
            var container = GetContainer(containerType);
            return await container.DeleteIfExistsAsync();
        }


        public static async Task<ImageSource> LoadImage(string imageId)
        {
            ByteArrayToImageSource byteArrayToImageSource = new ByteArrayToImageSource();
            Byte[] imageByteArray = await GetFileAsync(ContainerType.Image, imageId);
            Byte[] resizedImageByteArray = ImageResizer.ResizeImage(imageByteArray, 800, 533);
            ImageSource img = byteArrayToImageSource.Convert(resizedImageByteArray, typeof(ImageSource), null, null) as ImageSource;
            ImageHelper.LoadedImages.Add(new ImageHelper.ImageHelperItem { image = img, imageId = imageId });
            return byteArrayToImageSource.Convert(resizedImageByteArray, typeof(ImageSource), null, null) as ImageSource;
        }
    }
}
