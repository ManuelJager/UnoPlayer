#pragma warning disable CS1998 // For compatibility with UWP some these methods will be async, even though they may just run synchronous code

using System;
using System.Collections.Generic;
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
            string path = Path.Combine(dir, filename);

            File.Create(path);
            return path;
        }

        /// <summary>
        /// Deletes file asynchronously
        /// </summary>
        /// <param name="path"></param>
        public static async Task<string> DeleteFileAsync(string path)
        {
            System.IO.File.Delete(path);
            return path;
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
            return new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        }

        /// <summary>
        /// Opens a filestream with write permissions from the given path asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<System.IO.Stream> OpenFileForWriteAsync(string path)
        {
            return new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Write);
        }

        /// <summary>
        /// Determines if file exists asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<bool> FileExistsAsync(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// Determines if folder exists asynchronously
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<bool> FolderExistsAsync(string path)
        {
            return System.IO.Directory.Exists(path);
        }
    }
}
