using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GridDocumentWell
{
    /// <summary>
    /// Interaction logic for GridControl.xaml
    /// </summary>
    public partial class GridControl : Grid
    {
        public GridControl()
        {
            InitializeComponent();
        }

        public static DependencyProperty GridProperty =
            DependencyProperty.Register(
                "Grid", typeof(GridViewModel), typeof(GridControl),
                new FrameworkPropertyMetadata(OnGridChanged)
            );

        public GridViewModel Grid
        {
            get { return (GridViewModel)GetValue(GridProperty); }
            set
            {
                SetValue(GridProperty, value);
            }
        }
       
        private void OnGridChanged(GridViewModel value)
        {
            Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            if (value.Direction == Direction.Horizontal)
            {
                var definition = new RowDefinition();
                definition.Height = new GridLength(1, GridUnitType.Star);
                RowDefinitions.Add(definition);
            }
            else
            {
                var definition = new ColumnDefinition();
                definition.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinitions.Add(definition);

            }

            for (int i=0; i<value.Items.Count; i++)
            {
                var item = value.Items[i];

                UIElement childElement = null;
                if (item is GridViewModel gridItem)
                {
                    var innerGrid = new GridControl();
                    innerGrid.Grid = gridItem;
                    childElement = innerGrid;
                }
                else if (item is UIElement element)
                {
                    childElement = element;
                }
                if (childElement != null)
                {
                    if (value.Direction == Direction.Horizontal)
                    {
                        var definition = new ColumnDefinition();
                        definition.Width = new GridLength(1, GridUnitType.Star);
                        ColumnDefinitions.Add(definition);
                        SetColumn(childElement, i);
                    }
                    if (value.Direction == Direction.Vertical)
                    {
                        var definition = new RowDefinition();
                        definition.Height = new GridLength(1, GridUnitType.Star);
                        RowDefinitions.Add(definition);
                        SetRow(childElement, i);
                    }
                    Children.Add(childElement);
                }
            }
            LayoutSynchronizer.Update((Visual)this);
        }

        private static void OnGridChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            GridControl control = source as GridControl;
            control.OnGridChanged((GridViewModel)e.NewValue);
        }
    }
}
