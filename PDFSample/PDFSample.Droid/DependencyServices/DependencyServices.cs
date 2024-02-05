using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Locations;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Connect.Droid.Native;
using Connect.Model;
using Xamarin.Forms;
using Environment = System.Environment;
using File = System.IO.File;
using Path = System.IO.Path;
using System.Text.RegularExpressions;
using Task = System.Threading.Tasks.Task;
using AndroidX.Core.Text;
using Android.Views.InputMethods;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Icu.Text;
using Android.Provider;
using AndroidX.Core.App;
using PDFSample.DependencyServices;
using PDFSample.Droid.DependencyServices;
using PDFSample.Utility;
using Plugin.CurrentActivity;
using Xamarin.Essentials;

[assembly: Dependency(typeof(DependencyServices))]
namespace PDFSample.Droid.DependencyServices
{
    public class DependencyServices : IDependencyServices
    {
        private Timer singleAppTimer;
        private const string OnBoardConnect = "OnBoard Connect";
        private const string SignImages = "app_OnBoard_Images";
        private const string Logs = "Logs";
        private string PDFFilePath = String.Empty;
        public event EventHandler<EventArgs> OnFileDownloaded;

        string _packageName => Android.App.Application.Context.PackageName;
        public string AppVersion
        {
            get
            {
                var context = Android.App.Application.Context;
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                return string.Format("{0}.{1}", info.VersionName, info.VersionCode);
            }
        }

        public string UserAgent
        {
            get
            {
                return string.Format("Connect/{0} ({1})", AppVersion, OSVersion);
            }
        }

        public string OSVersion
        {
            get
            {
                return string.Format("Android {0}", Android.OS.Build.VERSION.Release);
            }
        }

      
        public void ShowPDF(Stream stream)
        {

        }

        public bool IsLocationTurned()
        {
            throw new NotImplementedException();
        }

        public void OpenLocationSettings()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                Context ctx = Android.App.Application.Context;

                Intent settingsIntent = new Intent();
                settingsIntent.SetAction(Android.Provider.Settings.ActionLocationSourceSettings);
                settingsIntent.AddFlags(ActivityFlags.NewTask);
                ctx.StartActivity(settingsIntent);

            }
            else
            {

            }
        }

        public string LogFilePath
        {
            get
            {
                var path = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Personal), Logs);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    FileInfo fileInfo;
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    if (IsExternalStorageAvailable())
                    {
                        documents = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
                    }
                    string[] files = Directory.GetFiles(documents);
                    foreach (string file in files)
                    {
                        fileInfo = new FileInfo(file);
                        var onBoardConnectLog = "OnBoard_Connect_log_";
                        if (fileInfo.Name.StartsWith(onBoardConnectLog, StringComparison.CurrentCulture))
                        {
                            var targetFile = Path.Combine(path, fileInfo.Name);
                            if (!File.Exists(targetFile))
                            {
                                File.Move(fileInfo.FullName, targetFile);
                            }
                        }
                    }
                }
                return path;
            }
        }

        public string ImagesPath
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), OnBoardConnect);
                if (!Directory.Exists(path))
                {
                    var existingPictures = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    if (IsExternalStorageAvailable())
                    {
                        existingPictures = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).ToString(), OnBoardConnect);
                    }
                    DirectoryMove(existingPictures, path, true);
                }
                return path;
            }
        }

        public string SignatureImagesPath
        {
            get
            {
                var sandboxDirectory = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).ToString();
                var path = Path.Combine(sandboxDirectory, SignImages);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public void StopSingleAppMode()
        {
            throw new NotImplementedException();
        }

        public string SendDb
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SendDb");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }


        public async void CaptureQRCode()
        {
            return;
        }

        /// <summary>
        /// Checks to see if the external storage state is available.
        /// </summary>
        /// <returns>true if available</returns>
        public static bool IsExternalStorageAvailable()
        {
            return Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted);
        }

        
       

        public Stream Create()
        {
            var files = File.Create(Path.Combine(LogFilePath, "sample.zip"));
            return files;
        }

        public void LockOrientation()
        {
            throw new NotImplementedException();
        }

        public void LockScreenOrientation(int flag)
        {
            throw new NotImplementedException();
        }

        public void UnlockOrientation()
        {
            throw new NotImplementedException();
        }


        public void ClearCookies()
        {
            try
            {
                var cookieManager = Android.Webkit.CookieManager.Instance;
                cookieManager.RemoveAllCookie();
            }
            catch (Exception clearCookieException)
            {
                Debugger.Break();
                
                Console.WriteLine(clearCookieException);
            }
        }
        public void NotificationAlert(string message)
        {

        }
        public bool BiometricsType()
        {
            return false;
        }

        public async Task<bool> BiometricsAuthentication()
        {
            return true;
        }

        private void DirectoryMove(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (dir.Exists)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.MoveTo(temppath);
                }

                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryMove(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
        }
        public double calculateWidth(string text)
        {
            Android.Graphics.Rect bounds = new Android.Graphics.Rect();
            TextView textView = new TextView(Forms.Context);
            textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
            var length = bounds.Width();
            return length / Resources.System.DisplayMetrics.ScaledDensity;
        }
        public void LockScreenForScreenShot()
        {

        }

        public void UnlockScreen()
        {
            CrossCurrentActivity.Current.Activity.Window.ClearFlags(WindowManagerFlags.Secure);
        }

        public List<byte[]> History_PE_PDFImageBytes()
        {
            return null;
        }

        public void LockScreenForDroid()
        {
            try
            {
                CrossCurrentActivity.Current.Activity.Window.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);                
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

        }

        public void MinimizeApp()
        {

        }

        public bool DefaultMail()
        {
            return true;
        }

        public void DisableTabbedPage()
        {

        }

        public void EnableTabbedPage()
        {
            
        }

        public bool IsLocationPermissionGranted()
        {
            throw new NotImplementedException();
        }

        public bool ConvertHtmltoPDF(PDFToHtml _PDFToHtml, string htmlContent)
        {
            try
            {
                if (_PDFToHtml.IsPDFGenerating)
                    //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Alert", "PDF is generating...", "Ok");
                {
                    
                }
                else if (_PDFToHtml.Status == Util.PDFEnum.Completed)
                {
                    //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Alert", "PDF is generated successfully", "Ok");
                    
                }
                // else if (!_PDFToHtml.IsPDFGenerating)
                // {
                    _PDFToHtml.GeneratePDF();
                //}

                PDFFilePath = _PDFToHtml.PDFFilePathWithPageNumber;                                   

                Device.BeginInvokeOnMainThread(() =>
                {
                    var webpage = new Android.Webkit.WebView(Android.App.Application.Context);
                    webpage.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                    webpage.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete
                    webpage.SetLayerType(LayerType.Software, null);
                    webpage.Layout(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight);                    
                   // webpage.LoadData(_PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8");
                    webpage.LoadDataWithBaseURL(Get(), _PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8", null);
                    webpage.SetWebViewClient(new WebViewCallBack(_PDFToHtml));
                });

                return true;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                
                return false;
            }
        }
        public string Get()
        {
            return "file:///android_asset/";
        }        

        public string GetHistoryPDFFIlePath()
        {
            return PDFFilePath;
        }


        public void NotifyFileDownloaded()
        {
            throw new NotImplementedException();
        }

        public async void SendEmail(string Message, string Subject)
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.Q)
                {
                    var message = new EmailMessage
                    {
                        Subject = Subject,
                        Body = Message,
                        BodyFormat = EmailBodyFormat.Html,
                    };
                    var file = GetHistoryPDFFIlePath();
                    message.Attachments.Add(new EmailAttachment(file, "mailto:"));
                    await Email.ComposeAsync(message);
                }
                else
                {
                    var path = Android.Net.Uri.FromFile(new Java.IO.File(GetHistoryPDFFIlePath()));
                    Intent emailIntent = new Intent(Intent.ActionSendto);
                    emailIntent.SetData(Android.Net.Uri.Parse("mailto:"));
                    emailIntent.PutExtra(Intent.ExtraStream, path);
                    emailIntent.PutExtra(Intent.ExtraSubject, Subject);
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
                    {
                        emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(Message, HtmlCompat.FromHtmlModeLegacy));
                    }
                    else
                    {
                        emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(Message));

                    }
                    emailIntent.SetFlags(ActivityFlags.GrantReadUriPermission);
                    emailIntent.AddFlags(ActivityFlags.NewTask);

                    Intent chooserIntent = Intent.CreateChooser(emailIntent, "Send Email...");
                    chooserIntent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(chooserIntent);
                }
            }
            catch (Exception ex)
            {
              Debugger.Break();
            }
        }

        public Task<bool> SendEmailForHelp(string Message, string LogFilePath)
        {
            throw new NotImplementedException();
        }

        public string DocumentFilePath => GetLocalFilePath();

        private string GetLocalFilePath()
        {
            //For dummy file path creation.
            //return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var loc = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            return loc;
        }
        

        public void CloseConnectAlert()
        {
            
        }

        public bool ConvertDashboardDetails(PDFToHtml _PDFToHtml, string htmlContent)
        {
            try
            {
                if (_PDFToHtml.IsPDFGenerating)
                    //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Alert", "PDF is generating...", "Ok");
                {
                    
                }
                else if (_PDFToHtml.Status == Util.PDFEnum.Completed)
                {
                    //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Alert", "PDF is generated successfully", "Ok");
                    
                }
                // else if (!_PDFToHtml.IsPDFGenerating)
                // {
                    _PDFToHtml.GeneratePDF();
                //}

                PDFFilePath = _PDFToHtml.PDFFilePathWithPageNumber;

                Device.BeginInvokeOnMainThread(() =>
                {
                    var webpage = new Android.Webkit.WebView(Android.App.Application.Context);
                    webpage.Settings.JavaScriptEnabled = true;
#pragma warning disable CS0618 // Type or member is obsolete
                    webpage.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete
                    webpage.SetLayerType(LayerType.Software, null);
                    webpage.Layout(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight);
                    // webpage.LoadData(_PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8");
                    webpage.LoadDataWithBaseURL(Get(), _PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8", null);
                    webpage.SetWebViewClient(new WebViewCallBack(_PDFToHtml));
                });

                return true;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                
                return false;
            }
        }

        public string GetDashboardPDFFIlePath()
        {
            throw new NotImplementedException();
        }

        public void SetThemeBasedOnDeviceAndUser(bool isFromLoginPage)
        {
            
        }

        public bool IsAppVersionNeedUpdate()
        {
            throw new NotImplementedException();
        }

        public void CheckForUpdate()
        {
            throw new NotImplementedException();
        }

        public string ProcedureImagesPath
        {
            get
            {
                var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ProcedureImages");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }               
                return path;
            }
        }

        public bool WindowSizeChanged()
        {
            return false;
        }

        public void HideKeyboard()
        {
            try
            {
                var context = CrossCurrentActivity.Current.Activity;
                var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                if (inputMethodManager != null)
                {
                    var token = context.CurrentFocus?.WindowToken;
                    inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                    context.Window.DecorView.ClearFocus();
                }
            }
            catch (Exception hideKeyboard_exception)
            {
                Debugger.Break();
            }
        }

        public double calculateStringWidth(string text, double fontSize = 12)
        {
            return 0;
        }

        public bool IsAppInLockTaskMode()
        {
            throw new NotImplementedException();
        }

        public void ForceCloseAllConnectAlerts()
        {
           
        }

        public async void SendEvalPDFEmail(string Message, string Subject, string PDFPath)
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.Q)
                {
                    var message = new EmailMessage
                    {
                        Subject = Subject,
                        Body = Message,
                        BodyFormat = EmailBodyFormat.Html,
                    };
                    var file = PDFPath;
                    message.Attachments.Add(new EmailAttachment(file, "mailto:"));
                    await Email.ComposeAsync(message);
                }
                else
                {
                    var path = Android.Net.Uri.FromFile(new Java.IO.File(PDFPath));
                    Intent emailIntent = new Intent(Intent.ActionSendto);
                    emailIntent.SetData(Android.Net.Uri.Parse("mailto:"));
                    emailIntent.PutExtra(Intent.ExtraStream, path);
                    emailIntent.PutExtra(Intent.ExtraSubject, Subject);
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
                    {
                        emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(Message, HtmlCompat.FromHtmlModeLegacy));
                    }
                    else
                    {
                        emailIntent.PutExtra(Intent.ExtraText, Html.FromHtml(Message));

                    }
                    emailIntent.SetFlags(ActivityFlags.GrantReadUriPermission);
                    emailIntent.AddFlags(ActivityFlags.NewTask);

                    Intent chooserIntent = Intent.CreateChooser(emailIntent, "Send Email...");
                    chooserIntent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(chooserIntent);
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }

        public Task<bool> ShowConnectAlert(string title, string message, string yesText, string noText, string cancelText, bool isCheckBoxShown = false)
        {
            return null;
        }

        /// <summary>
        /// To check whether app notification is enabled or not
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetApplicationNotificationSettings()
        {
            try
            {
                var nm = NotificationManagerCompat.From(Android.App.Application.Context);
                bool enabled = nm.AreNotificationsEnabled();
                return enabled;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return false;
            }
        }
    }
}
