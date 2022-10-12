

namespace app.ui
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Autodesk.Revit.UI;
    /// <summary>
    /// Interaction logic for FamilyManagerMainPage.xaml
    /// </summary>
    public partial class FamilyManagerMainPage : Page, IDisposable, IDockablePaneProvider
    {
        #region constructor

        public FamilyManagerMainPage()
        {
            InitializeComponent();
        }
        #endregion

        #region public methods
        public void Dispose()
        {
            this.Dispose();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Right,
            };
        }

        #endregion
    }
}
