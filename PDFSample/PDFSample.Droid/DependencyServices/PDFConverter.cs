using Android.Views;
using Connect.Droid.Native;
using Connect.Model;
using PDFSample.DependencyServices;
using PDFSample.Droid.DependencyServices;
using PDFSample.Utility;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFConverter))]
namespace PDFSample.Droid.DependencyServices
{
    public class PDFConverter : IPDFConverter
    {
        public void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml)
        {
            try
            {
                var webpage = new Android.Webkit.WebView(Android.App.Application.Context);
                webpage.Settings.JavaScriptEnabled = true;

#pragma warning disable CS0618 // Type or member is obsolete
                webpage.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete

                webpage.SetLayerType(LayerType.Software, null);
                webpage.Layout(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight);
                webpage.LoadData(_PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8");
                webpage.SetWebViewClient(new WebViewCallBack(_PDFToHtml));
            }
            catch
            {
                _PDFToHtml.Status = Util.PDFEnum.Failed;
            }
        }
    }
}
