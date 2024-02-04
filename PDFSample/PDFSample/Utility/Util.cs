using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PDFSample.Utility
{
    public class Util : INotifyPropertyChanged
    {
        public enum SeriesApiEnum
        {
            NotStarted = 0,
            InProgress = 1,
            Error = 2,
            Completed = 5 //API completed.
        }

        public enum SkillApiEnum
        {
            NotStarted = 0,
            InProgress = 1,
            Error = 2,
            Completed = 5 //API completed.
        }

        public enum CourseApiEnum
        {
            NotStarted = 0,
            InProgress = 1,
            Error = 2,
            Completed = 5 //API completed.
        }

        public enum ImageDownloadEnum
        {
            NotStarted = 0,
            InProgress = 1,
            Error = 2,
            Completed = 5 //API completed.
        }

        public enum PDFEnum
        {
            Started = 0,
            Failed = 1,
            Completed = 2
        }

        public enum Theme
        {
            Default,
            Light,
            Dark
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Util _instance;
        public static Util Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Util();
                }
                return _instance;
            }
        }
        
    }
}
