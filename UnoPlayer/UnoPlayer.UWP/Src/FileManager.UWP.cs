using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UnoPlayer
{
    public partial class FileManager
    {
        /// <summary>
        /// Creates a path of given name in the given directory asynchronously
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static async Task<string> CreateFileAsync(string dir, string filename)
        {
            var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(dir);
            var file = await folder.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            return file.Path;
        }

        /// <summary>
        /// Deletes file asynchronously
        /// </summary>
        /// <param name="path"></param>
        public static async Task DeleteFileAsync(string path)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);
            await file.DeleteAsync();
        }

        /// <summary>
        /// Tries to delete a file
        /// This is a safe alternative to <see cref="DeleteFileAsync(string)"/>
        /// </summary>
        /// <param name="path"></param>
        public static async Task TryDeleteFile(string path)
        {
            try
            {
                await DeleteFileAsync(path);
            }
            catch { }
        }

        /// <summary>
        /// Opens a filestream with read permissions from the given path asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<System.IO.Stream> OpenFileForReadAsync(string path)
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);
            return await storageFile.OpenStreamForReadAsync();
        }

        /// <summary>
        /// Opens a filestream with write permissions from the given path asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<System.IO.Stream> OpenFileForWriteAsync(string path)
        {
            var storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);
            return await storageFile.OpenStreamForWriteAsync();
        }

        /// <summary>
        /// Determines if file exists asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<bool> FileExistsAsync(string path)
        {
            try
            {
                var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if folder exists asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<bool> FolderExistsAsync(string path)
        {
            try
            {
                var file = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
