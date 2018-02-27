using HRInvoiceApp.Helpers;
using HRInvoiceApp.UWP.Helpers;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace HRInvoiceApp.UWP.Helpers
{
    class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string fileName)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);
        }
    }
}
