using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Connect.Model;

namespace PDFSample.DependencyServices
{
    public interface IDependencyServices
    {
        string AppVersion { get; }
        string UserAgent { get; }
        string OSVersion { get; }
        string LogFilePath { get; }
        string ImagesPath { get; }
        void CaptureQRCode();
       // void StartSingleAppMode(WrittenExam url);
        void StopSingleAppMode();
        string SendDb { get; }
        string SignatureImagesPath { get; }
        System.IO.Stream Create();
        void LockOrientation();
        void LockScreenOrientation(int flag);
        void UnlockOrientation();
        void LockScreenForScreenShot();
        void UnlockScreen();
        void ClearCookies();
        void NotificationAlert(string message);
        bool BiometricsType();
        void MinimizeApp();
        bool DefaultMail();
        bool IsLocationTurned();
        void OpenLocationSettings();
        Task<bool> BiometricsAuthentication();
        double calculateWidth(string text);
        void LockScreenForDroid();
        void DisableTabbedPage();
        void EnableTabbedPage();
        bool IsLocationPermissionGranted();
        bool ConvertHtmltoPDF(PDFToHtml PDFToHtml, string htmlContent);
        string GetHistoryPDFFIlePath();
        event EventHandler<EventArgs> OnFileDownloaded;
        void NotifyFileDownloaded();
        void SendEmail(string Message, string Subject);
        Task<bool> SendEmailForHelp(string Message, string LogFilePath);
        Task<bool> ShowConnectAlert(string title, string message, string yesText, string noText, string cancelText, bool isCheckBoxShown = false);
        void CloseConnectAlert();
        bool ConvertDashboardDetails(PDFToHtml PDFToHtml, string htmlContent);
        string GetDashboardPDFFIlePath();
        //Utility.Util.Theme GetOperatingSystemTheme();
        void SetThemeBasedOnDeviceAndUser(bool isFromLoginPage);
        bool IsAppVersionNeedUpdate();
        void CheckForUpdate();
        string ProcedureImagesPath { get; }
        //Position GetLocationPoints();
        void HideKeyboard();

        double calculateStringWidth(string text, double fontSize = 12);
        void ShowPDF(Stream stream);
        void SendEvalPDFEmail(string Message, string Subject, string Path);
        bool IsAppInLockTaskMode();

        void ForceCloseAllConnectAlerts();

        List<byte[]> History_PE_PDFImageBytes();
        bool WindowSizeChanged();
        Task<bool> GetApplicationNotificationSettings();
    }
}