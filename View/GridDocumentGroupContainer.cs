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
                "Grid", typeof(GridViewModel), typeof(GridDocumentGroupContainer)
            );

        public GridViewModel Grid
        {
            get
            {
                return (GridViewModel)GetValue(GridProperty);
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
                    var grid = new GridViewModel();
                    foreach (var child in VisibleChildren)
                    {
                        grid.Direction = Direction.Horizontal;
                        var control = new DocumentGroupContainerControl()
                        {
                            ItemsSource = new List<ViewElement>()
                            {
                                child
                            },
                            Orientation = System.Windows.Controls.Orientation.Horizontal
                        };

                        grid.Items.Add(control);
                    }
                    Grid = grid;
                }
            }
        }
    }
}
