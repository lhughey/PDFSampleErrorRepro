
using Connect.Model;

namespace PDFSample.DependencyServices
{
    public interface IPDFConverter
    {
        void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml);
    }
}
