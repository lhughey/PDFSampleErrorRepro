using System;
using Connect.DependencyServices;
using Xamarin.Forms;

namespace PDFSample.Utility
{
    public class PDFUtils
    {
        private static string GetBaseUrl()
        {
            var fileHelper = DependencyService.Get<IFileHelper>();
            return fileHelper.ResourcesBaseUrl + "pdfjs/";
        }

        public static string PdfJsViewerUri => GetBaseUrl() + "web/viewer.html";
    }
}
