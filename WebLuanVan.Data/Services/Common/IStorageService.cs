using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WebLuanVan.Data.Services.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream meadiaBinaryStream, string fileName);
        Task DeleteFileAsync(string fileName);
        string GetFolder();
        string GetPath(string filename);
    }
}
