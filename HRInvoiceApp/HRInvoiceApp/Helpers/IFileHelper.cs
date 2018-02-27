using System;
using System.Collections.Generic;
using System.Text;

namespace HRInvoiceApp.Helpers
{
    interface IFileHelper
    {
        string GetLocalFilePath(string fileName);
    }
}
