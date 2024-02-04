using System;
using System.Diagnostics;
using Android.Print;
using Android.Webkit;
using Connect.Model;
using PDFSample.DependencyServices;
using PDFSample.Utility;
using Xamarin.Forms;
using Task = System.Threading.Tasks.Task;

namespace Connect.Droid.Native
{
    public class WebViewCallBack : WebViewClient
    {
        bool _complete;
        readonly PDFToHtml pDFToHtml;
        PdfLayoutResultCallback layoutResultCallback;

        public WebViewCallBack(PDFToHtml _PDFToHtml)
        {
            pDFToHtml = _PDFToHtml;
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            try
            {
                if (!_complete)
                {
                    _complete = true;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        OnPageLoaded(view);
                    });
                }
            }
            catch (Exception OnPageFinished_Exception)
            {
               Debugger.Break();
            }
        }

        public override void OnLoadResource(Android.Webkit.WebView view, string url)
        {
            try
            {
                base.OnLoadResource(view, url);
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    // if (!_complete)
                    //     OnPageFinished(view, url);
                    //
                    // long PdfLength = layoutResultCallback.pdfFile.Length();
                    // int fileLengthCount = Convert.ToInt32(PdfLength);
                    //
                    // if (fileLengthCount > 0)
                    // {
                    //     DependencyService.Get<IDependencyServices>().NotifyFileDownloaded();
                    //     return false;
                    // }

                    return true;
                });
            }
            catch (Exception OnLoadResource_Exception)
            {
                Debugger.Break();
            }
        }

        internal void OnPageLoaded(Android.Webkit.WebView webView)
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var builder = new PrintAttributes.Builder();
                    builder.SetMediaSize(PrintAttributes.MediaSize.IsoA4);
                    builder.SetResolution(new PrintAttributes.Resolution("pdf", "pdf", (int)pDFToHtml.PageDPI, (int)pDFToHtml.PageDPI));
                    builder.SetMinMargins(PrintAttributes.Margins.NoMargins);
                    var attributes = builder.Build();
                    var adapter = webView.CreatePrintDocumentAdapter(pDFToHtml.FileName);
                    layoutResultCallback = new PdfLayoutResultCallback();
                    layoutResultCallback.Adapter = adapter;
                    layoutResultCallback.PDFToHtml = pDFToHtml;
                    //HACK: add this delay as the issue with the PDF generation seems to be some type of race condition. 
                    Task.Delay(3500);
                    adapter.OnLayout(null, attributes, null, layoutResultCallback, null);
                }
            }
            catch (Exception ex)
            {
                pDFToHtml.Status = Util.PDFEnum.Failed;
                Debugger.Break();
            }
        }

        public override void OnReceivedError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);
            pDFToHtml.Status = Util.PDFEnum.Failed;
        }

        public override void OnReceivedHttpError(Android.Webkit.WebView view, IWebResourceRequest request, WebResourceResponse errorResponse)
        {
            base.OnReceivedHttpError(view, request, errorResponse);
            pDFToHtml.Status = Util.PDFEnum.Failed;
        }
    }
}


