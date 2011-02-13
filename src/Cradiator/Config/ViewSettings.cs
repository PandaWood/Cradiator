using System.ComponentModel;
using Cradiator.Extensions;

namespace Cradiator.Config
{
    public class ViewSettings : IViewSettings, INotifyPropertyChanged
    {
        protected string _url;
        public string URL
        {
            get { return _url; }
            set
            {
                if (_url == value) return;
                _url = value;
                Notify("URL");
            }
        }

        protected string _skinName;
        public string SkinName
        {
            get { return _skinName; }
            set
            {
                if (_skinName == value) return;
                _skinName = value;
                Notify("SkinName");
            }
        }

        protected string _projectNameRegEx;
        public string ProjectNameRegEx
        {
            get { return _projectNameRegEx.GetRegEx(); }
            set
            {
                if (_projectNameRegEx == value) return;
                _projectNameRegEx = value;
                Notify("ProjectNameRegEx");
            }
        }

        protected string _categoryRegEx;
        public string CategoryRegEx
        {
            get { return _categoryRegEx.GetRegEx(); }
            set
            {
                if (_categoryRegEx == value) return;
                _categoryRegEx = value;
                Notify("CategoryRegEx");
            }
        }


        protected string _viewName;
        public string ViewName
        {
            get { return _viewName; }
            set
            {
                if (_viewName == value) return;
                _viewName = value;
                Notify("ViewName");
            }
        }



        protected bool _showOnlyBroken;
        public bool ShowOnlyBroken
        {
            get { return _showOnlyBroken; }
            set
            {
                if (_showOnlyBroken == value) return;
                _showOnlyBroken = value;
                Notify("ShowOnlyBroken");
            }
        }


        protected string _serverNameRegEx;
        public string ServerNameRegEx
        {
            get { return _serverNameRegEx;}
            set
            {
                if (_serverNameRegEx == value) return;
                   _serverNameRegEx = value;
                Notify("ServerNameRegEx");
            }
        }

        protected bool _ShowServeName;
        public bool ShowServerName
        {
            get { return _ShowServeName; }
            set
            {
                if (_ShowServeName == value) return;
                _ShowServeName = value;
                Notify("ShowServerName");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
