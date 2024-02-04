using System;
using System.Diagnostics;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Connect.Model;
using Java.Interop;
using Java.Lang;
using PDFSample.Utility;

namespace Connect.Droid.Native
{
    [Register("android/print/PdfLayoutResultCallback")]
    public class PdfLayoutResultCallback : PrintDocumentAdapter.LayoutResultCallback
    {
        public PrintDocumentAdapter Adapter { get; set; }

        public PDFToHtml PDFToHtml { get; set; }

        public Java.IO.File pdfFile;

        public PdfLayoutResultCallback(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public PdfLayoutResultCallback() : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            try
            {
                if (!(Handle != IntPtr.Zero))
                {
                    unsafe
                    {
                        JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                        SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                        JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                    }
                }
            }
            catch (System.Exception PdfLayoutResultCallback_Exception)
            {
                Debugger.Break();
            }
        }

        public override void OnLayoutFinished(PrintDocumentInfo info, bool changed)
        {
            try
            {
                pdfFile = new Java.IO.File(PDFToHtml.FilePath);
                var fileDescriptor = ParcelFileDescriptor.Open(pdfFile, ParcelFileMode.ReadWrite);
                var writeResultCallback = new PdfWriteResultCallback(PDFToHtml);
                Adapter.OnWrite(new PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(), writeResultCallback);
            }
            catch (System.Exception ex)
            {
                PDFToHtml.Status = Util.PDFEnum.Failed;
                Debugger.Break();
            }

            base.OnLayoutFinished(info, changed);
        }

        public override void OnLayoutCancelled()
        {
            base.OnLayoutCancelled();
            PDFToHtml.Status = Util.PDFEnum.Failed;
        }

        public override void OnLayoutFailed(ICharSequence error)
        {
            base.OnLayoutFailed(error);
            PDFToHtml.Status = Util.PDFEnum.Failed;
        }
    }

    [Register("android/print/PdfWriteResult")]
    public class PdfWriteResultCallback : PrintDocumentAdapter.WriteResultCallback
    {
        readonly PDFToHtml pDFToHtml;

        public PdfWriteResultCallback(PDFToHtml _pDFToHtml, IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            pDFToHtml = _pDFToHtml;
        }

        public PdfWriteResultCallback(PDFToHtml _pDFToHtml) : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            try
            {
                if (!(Handle != IntPtr.Zero))
                {
                    unsafe
                    {
                        JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                        SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                        JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                    }
                }

                pDFToHtml = _pDFToHtml;
            }
            catch (System.Exception PdfWriteResultCallback_Exception)
            {
                Debugger.Break();
            }
        }


        public override void OnWriteFinished(PageRange[] pages)
        {
            base.OnWriteFinished(pages);
            pDFToHtml.Status = Util.PDFEnum.Completed;
        }

        public override void OnWriteCancelled()
        {
            base.OnWriteCancelled();
            pDFToHtml.Status = Util.PDFEnum.Failed;
        }

        public override void OnWriteFailed(ICharSequence error)
        {
            base.OnWriteFailed(error);
            pDFToHtml.Status = Util.PDFEnum.Failed;
        }
    }
}

