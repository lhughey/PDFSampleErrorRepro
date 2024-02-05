
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Connect.DependencyServices;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PDFSample;
using PDFSample.Utility;
using Xamarin.Forms;

namespace Connect.Model
{
    public class PDFToHtml : INotifyPropertyChanged, IDisposable
    {
        private static bool isfromdashboard;
        private bool ispdfloading;
        private Util.PDFEnum pDFEnum;

        public PDFToHtml(bool IsFromDashboard, string filename)
        {
            isfromdashboard = IsFromDashboard;
            FileName = filename;               
        }

        public bool IsPDFGenerating
        {
            get { return ispdfloading; }
            set
            {
                ispdfloading = value;
                OnPropertyChanged("IsPDFGenerating");
            }
        }

        public Util.PDFEnum Status
        {
            get { return pDFEnum; }
            set
            {
                pDFEnum = value;
                //this.UpdatePDFStatus(value);
                OnPropertyChanged("Status");
            }
        }        

        public string HTMLString { get; set; }

        public string FileName { get; set; }

        public double PageHeight { get; set; } = 1024;

        public double PageWidth { get; set; } = 512;

        public double PageDPI { get; set; } = 300;

        public string FilePath { get; set; }

        public string PDFFilePathWithPageNumber { get; set; }

        public FileStream FileStream { get; set; }

        public byte[] PDFStreamArray { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            if (FileStream != null)
            {
                FileStream.Dispose();
                FileStream = null;
            }

            PDFStreamArray = null;
        }
        
        public async void GeneratePDF()
        {
            try
            {
                this.Status = Util.PDFEnum.Started;
                FilePath = CreateTempPath(FileName);
                PDFFilePathWithPageNumber = CreatePathForPDFWithPageNumber();
                FileStream = File.Create(FilePath);
                //DependencyService.Get<IPDFConverter>().ConvertHTMLtoPDF(this);
            }
            catch (Exception generatePDFException)
            {
                Debugger.Break();
            }
        }

        public static string CreateTempPath(string fileName)
        {
            string path = string.Empty;
            string tempPath = string.Empty;
            try
            {
                if (isfromdashboard)
                {
                    tempPath = Path.Combine(DependencyService.Get<IFileHelper>().DocumentFilePath, "Onboard Connect");
                }

                else
                {
                    tempPath = Path.Combine(DependencyService.Get<IFileHelper>().DocumentFilePath, "temp");
                }
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);

                path = Path.Combine(tempPath, fileName + ".pdf");
                while (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception createTempPathException)
            {
                Debugger.Break();
                path = null;
            }


            return path;
        }

        public static string CreatePathForPDFWithPageNumber()
        {
            string pdfFileWithPageNumber, pdffilepath, pdfName, temppdffilePath;
            try
            {                
                if (isfromdashboard)
                {
                    temppdffilePath = Path.Combine(DependencyService.Get<IFileHelper>().DocumentFilePath, "Onboard Connect");
                    pdffilepath = Path.Combine(temppdffilePath, "Dashboard.pdf");
                    //pdfName = string.Format("Dashboard-{1}_{0}.PDF", "Blake", "Tester");
                    pdfName = "Dashboard.pdf";
                }
                else
                {
                    temppdffilePath = Path.Combine(DependencyService.Get<IFileHelper>().DocumentFilePath, "temp");
                    pdffilepath = Path.Combine(temppdffilePath, "Evaluation_History.pdf");
                    //pdfName = string.Format("Dashboard-{1}_{0}.PDF", "Blake", "Tester");
                    pdfName = "Evaluation_History.pdf";
                }               
                pdfFileWithPageNumber = System.IO.Path.Combine(temppdffilePath, pdfName);
            }
            catch (Exception pdfWithPageNumberException)
            {
                Debugger.Break();
                pdfFileWithPageNumber = null;
            }


            return pdfFileWithPageNumber;
        }

        private async void UpdatePDFStatus(Util.PDFEnum newValue)
        {
            if (newValue == Util.PDFEnum.Started)
                IsPDFGenerating = true;
            else if (newValue == Util.PDFEnum.Failed)
            {
                IsPDFGenerating = false;
                //await App.Current.MainPage.DisplayAlert("ERROR!", "PDF is not generated", "Ok");
                Debugger.Break();
            }
            else if (newValue == Util.PDFEnum.Completed)
            {
                try
                {
                    PDFStreamArray = Device.RuntimePlatform == Device.iOS ? File.ReadAllBytes(FilePath + ".pdf") : new byte[FileStream.Length];

                    if (Device.RuntimePlatform == Device.Android)
                        FileStream.Read(PDFStreamArray, 0, (int)FileStream.Length);

                    await FileStream.WriteAsync(PDFStreamArray, 0, PDFStreamArray.Length);
                    FileStream.Close();
                    IsPDFGenerating = false;

                    string temppdffilePath, pdffilepath;
                    if (isfromdashboard)
                    {
                        temppdffilePath = Path.Combine(DependencyService.Get<IFileHelper>().DocumentFilePath, "Onboard Connect");
                        pdffilepath = Path.Combine(temppdffilePath, "Dashboard.pdf");
                    }
                    else
                    {
                        temppdffilePath = Path.Combine(DependencyService.Get<IFileHelper>().DocumentFilePath, "temp");
                        pdffilepath = Path.Combine(temppdffilePath, "Evaluation_History.pdf");
                    }                  

                    try
                    {                        
                        if (File.Exists(PDFFilePathWithPageNumber))
                        {
                            File.Delete(PDFFilePathWithPageNumber);
                        }
                        PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdffilepath), new PdfWriter(PDFFilePathWithPageNumber));
                        Document doc = new Document(pdfDoc);
                        int pageCount = pdfDoc.GetNumberOfPages();
                        for (int i = 1; i <= pageCount; i++)
                        {
                            Paragraph paragraph = new Paragraph(String.Format("Page {0} of {1}", i, pageCount));
                            paragraph.SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY);
                            paragraph.SetFontSize(10);
                            doc.ShowTextAligned(paragraph, 560, 10, i, iText.Layout.Properties.TextAlignment.RIGHT, iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);
                        }
                        doc.Close();
                    }
                    catch (Exception updatePDFStatusException)
                    {
                        Debugger.Break();
                    }                    

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                      //  await App.Current.MainPage.Navigation.PushAsync(new PDFViewer() { Title = FileName, BindingContext = this });
                    });
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
            }
        }

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
