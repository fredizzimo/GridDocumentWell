using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using System.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Microsoft.VisualStudio.PlatformUI.Shell;
using System.Windows;

namespace GridDocumentWell
{
    public class GridDocumentWellService : SGridDocumentWellService, IGridDocumentWellService
    {
        private IAsyncServiceProvider _asyncServiceProvider;

        public GridDocumentWellService(IAsyncServiceProvider provider)
        {
            _asyncServiceProvider = provider;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await TaskScheduler.Default;
            // do background operations that involve IO or other async methods

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            // query Visual Studio services on main thread unless they are documented as free threaded explicitly.
            // The reason for this is the final cast to service interface (such as IVsShell) may involve COM operations to add/release references.

            IVsShell vsShell = this._asyncServiceProvider.GetServiceAsync(typeof(SVsShell)) as IVsShell;
            // use Visual Studio services to continue initialization
        }
        public void NewGrid()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var factory = new GridViewElementFactory();
            ViewElementFactory.Current = factory;
            var profile = ViewManager.Instance.WindowProfile;
            DocumentGroupContainer[] oldGroupContainers = profile.FindAll<DocumentGroupContainer>().Where(group => !group.GetType().Equals(typeof(GridDocumentGroupContainer)) && group.Parent != null).ToArray();

            if (oldGroupContainers.Length == 0)
                return;
            using (ViewManager.Instance.DeferActiveViewChanges())
            {
                foreach (DocumentGroupContainer oldGroup in oldGroupContainers)
                {
                    DocumentGroupContainer newGroup = DocumentGroupContainer.Create();
                    CopyGroupProperties(oldGroup, newGroup);
                    ReplaceGroupContainer(oldGroup, newGroup);
                    MoveChildrenToNewGroup(oldGroup, newGroup);
                }
            }
        }

        private void CopyGroupProperties(DocumentGroupContainer oldGroup, DocumentGroupContainer newGroup)
        {
            LocalValueEnumerator localValueEnumerator = oldGroup.GetLocalValueEnumerator();
            while (localValueEnumerator.MoveNext())
            {
                LocalValueEntry current = localValueEnumerator.Current;
                if (!current.Property.ReadOnly)
                {
                    try
                    {
                        newGroup.SetValue(current.Property, current.Value);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void ReplaceGroupContainer(DocumentGroupContainer oldGroup, DocumentGroupContainer newGroup)
        {
            ViewGroup parent = oldGroup.Parent;
            int index = parent.Children.IndexOf(oldGroup);
            parent.Children.Insert(index, newGroup);
            oldGroup.Detach();
        }

        private void MoveChildrenToNewGroup(DocumentGroupContainer oldGroup, DocumentGroupContainer newGroup)
        {
            if (oldGroup != null && newGroup != null)
            {
                while (oldGroup.Children.Count != 0)
                {
                    ViewElement viewElement = oldGroup.Children[0];
                    viewElement.Detach();
                    newGroup.Children.Add(viewElement);
                }
            }
        }
    }
}
