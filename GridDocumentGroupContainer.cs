using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Platform.WindowManagement;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.PlatformUI.Shell;
using Microsoft.VisualStudio.PlatformUI.Shell.Controls;

namespace GridDocumentWell
{
    class GridDocumentGroupContainer : WMDocumentGroupContainer
    {
        public GridDocumentGroupContainer()
        {
        }

        public static DependencyProperty GridProperty =
            DependencyProperty.Register(
                "Grid", typeof(VSGrid), typeof(GridDocumentGroupContainer)
            );

        public VSGrid Grid
        {
            get
            {
                return (VSGrid)GetValue(GridProperty);
            }
            set
            {
                SetValue(GridProperty, value);
            }
        }

        protected override void OnVisibleChildrenChanged(NotifyCollectionChangedEventArgs args)
        {
            base.OnVisibleChildrenChanged(args);
            using (LayoutSynchronizer.BeginLayoutSynchronization())
            {
                using (ViewManager.Instance.DeferActiveViewChanges())
                {
                    var grid = new VSGrid();
                    grid.Direction = Direction.Horizontal;
                    /*
                    var control = new System.Windows.Controls.TextBox();
                    control.Text = "Hello World";
                    control.FontSize = 100;
                    grid.Items.Add(control);
                    */
                    var control = new DocumentGroupContainerControl();
                    control.ItemsSource = VisibleChildren;
                    control.Orientation = System.Windows.Controls.Orientation.Horizontal;
                    grid.Items.Add(control);
                    Grid = grid;
                }
            }
        }
    }
}
