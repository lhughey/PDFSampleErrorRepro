using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Connect.Model;
using PDFSample.DependencyServices;
using PDFSample.Utility;
using ReactiveUI;
using Xamarin.Forms;

namespace PDFSample
{
    public class MainPageViewModel: ReactiveObject
    {
        
        private PDFToHtml PDFToHtml { get; set; }
        public ReactiveCommand<Unit, Unit> CreatePDFCommand { get; set; }
        public MainPageViewModel()
        {
            CreatePDFCommand = ReactiveCommand.CreateFromTask(CreatePDFCommandExecute);
            CreatePDFCommand.Subscribe(c=>
            {
                Debug.WriteLine(c);
            });
            CreatePDFCommand.ThrownExceptions.Subscribe(error =>
            {
                Debug.WriteLine(error);
                Debugger.Break();
            });
            
            CreatePDF(GetPDFHtmlBody());
        }

        private Task CreatePDFCommandExecute()
        {
            //set status to downloaded, cause the html is static
            PDFToHtml.Status = Util.PDFEnum.Completed;
            
            DependencyService.Get<IDependencyServices>().ConvertHtmltoPDF(PDFToHtml, GetPDFHtmlBody());
            return Task.CompletedTask;
        }
        
        private async void CreatePDF(string text)
        {
            try
            {
                PDFToHtml = new PDFToHtml(true, "Dashboard");
                //this.BindingContext = PDFToHtml;
                PDFToHtml.HTMLString = text;
            }
            catch (Exception createPDFException)
            {
                Debugger.Break();
            }
        }

        public string GetPDFHtmlBody()
        {
            var str = @"<!DOCTYPE html>
<html>
<head>       
    <style type=""text/css""> 
		.Detailstable{
            width:100%;
            border-collapse:collapse;  
         }
		 p{
         padding:0px; 
         margin:1px; 
         page-break-inside:avoid;
         font-size: 11px;
         font-family: Helvetica;
        }
         .Detailstable td{
            width: auto;
            page-break-inside: avoid;
			border: 1px solid #000000;
            font-size: 11px;
            font-family: Helvetica;
         }
         .Detailstable th{
            font-size: 12px;
			text-align: center;
            font-family: Helvetica;
         }
		 .Detailstable tr:nth-child(odd){ 
            background: #D3D3D3;
            border: 1px solid #000000;
            width: auto;
            display: table-row;
            page-break-inside: avoid;
            page-break-after: always;
            height: auto;
         }
         .Detailstable tr:nth-child(even){
            background: #ffffff;
            border: 1px solid #000000;
            width: auto;
            display: table-row;
            page-break-inside: avoid;
            page-break-after: always;
            height: auto;
         }
		 .TFtable{
            width:100%;
            border-collapse:collapse;           
     
         }
         .TFtable td{
            width: auto;
            page-break-inside: avoid;
			border: 1px solid #000000;
            font-size: 11px;
            font-family: Helvetica;
         }
         .TFtable th{
            font-size: 12px;
			text-align: center;
            font-family: Helvetica;
         }
		 .TFtable tr:nth-child(odd){ 
            background: #D3D3D3;
            border: 1px solid #000000;
            width: auto;
            display: table-row;
            page-break-inside: avoid;
            page-break-after: always;
            height: auto;
         }
         .TFtable tr:nth-child(even){
            background: #ffffff;
            border: 1px solid #000000;
            width: auto;
            display: table-row;
            page-break-inside: avoid;
            page-break-after: always;
            height: auto;
         }
		 p.small {
          line-height: 0.7;
          margin-block-end: 0;
          font-size: 12px;
          font-family: Helvetica;
         }

         .EvaluationDetail_Table{                 
                  width: 100%; 
                  border-spacing: 10px;
         }

         .EvaluationDetail_Table td{
                  background-color: #0C3255; 
                  width: fit-content; 
                  height: 60px;
                  border-radius: 12px;
                  font-size: small;
                  color: white;
                  text-align: center;                                 
         }

	</style>	
</head>
<body>
  <header>    
        <table border=""0"" style=""width: 100%;"">
              <th style=""width: 10%; text-align: start;""><img src=""LearnLogo.png"" style=""text-align: start;""></th>
              <th style=""width: 50%; padding-top: 12px; text-align: start;"">
                <div style=""color: #000000; font-size: 13px; text-align: start;font-family: Helvetica;""><b>OnBoard Learning Management System</b></div> 
                <div style=""color: #000000; font-size: 13px; text-align: start;font-family: Helvetica;""><b>Industrial Training Services, Inc. - Dashboard Report</b></div>  
                <div style=""color: #000000; font-size: 12px; text-align: start;font-family: Helvetica;""><div style=""text-align: start;font-size: 12px; display: inline-block;font-family: Helvetica; ""> <i>Run By:&nbsp </i></div><span id=""runby-details"" style=""display: inline-block;""><p class=""small""><i>Blake Tester1 Eval on 02/01/2024 10:21 PM</i></p></span></div>
           </th>
		   <th text-align:="""" end;""=""""><img src=""OnLogo.png"" style=""float: right;""></th>
        </table>
 </header>
  <br>

  <table class=""EvaluationDetail_Table"">
    <tr>
        <td class=""Evaluation_Row"">
            <div id=""evals-thisweek"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">0</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Evals this week</p>
            </div>
        </td>

        <td class=""Evaluation_Row"">
            <div id=""evals-today"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">0</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Eval Today</p>
            </div>
           </td>

        <td class=""Evaluation_Row"">
            <div id=""students-thisweek"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">0</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Students this Week</p>
            </div>
            </td>

        <td class=""Evaluation_Row"">
            <div id=""students-today"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">0</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Students Today</p>
            </div>
           </td>

    </tr>

    <tr> </tr>

    <tr>

        <td class=""Evaluation_Row"">
           <div id=""average-time"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p><span style=""font-size: large;"">342</span> Minutes</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Average Time per PE</p>
            </div>
        </td>

        <td class=""Evaluation_Row"">
           <div id=""passed-evaluations"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">62%</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Passed Evaluations</p>
            </div>

          </td>

        <td class=""Evaluation_Row"">

         <div id=""images-added"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">0</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Images Added</p>
            </div>
          </td>

        <td class=""Evaluation_Row"">
        <div id=""total-PE"" style=""font-weight: bold; font-size: large; padding-bottom: 5px;""><p style=""font-size: large;"">2132</p></div>
            <div>
                <p style=""font-weight: bold; font-size: 14px;"">Total PE</p>
            </div>
         </td>

    </tr>
</table> 
  <br>
  <table id=""Top5Table"" border=""1"" class=""TFtable"">    
         <tr style=""background-color: #0c3255; color: #ffffff;"">
              <th style=""padding: 5px;"">Top 5 Performance Evaluations</th>
         </tr>               
  <tr><td style=""padding: 5px;""><p>''''21''' - '''''''''''</p></td></tr><tr><td style=""padding: 5px;""><p>''test'' - testing ''</p></td></tr><tr><td style=""padding: 5px;""><p>TLEAK004-P - TLEAK004 PE: Leak Investigations</p></td></tr><tr><td style=""padding: 5px;""><p>TESTSkillsssss - Test Skill</p></td></tr><tr><td style=""padding: 5px;""><p>02.09 - PERFORMANCE - 02.09 F.S. MECHANICAL REPAIR</p></td></tr></table>
</body>
</html>";


            return str;
        }
    }
}