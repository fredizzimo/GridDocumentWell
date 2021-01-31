using Microsoft.VisualStudio.PlatformUI.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace GridDocumentWell
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(GridDocumentWellPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class GridDocumentWellPackage : AsyncPackage
    {
        /// <summary>
        /// GridDocumentWellPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "97b4ef32-f091-4296-9bb4-b3615e7aaa5f";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            MergeResources();
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await NewGridCommand.InitializeAsync(this);
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

        private void MergeResources()
        {
            if (Application.Current == null)
                throw new InvalidOperationException("We need an application set.");
            MergeResource("Datatemplates.xaml");
            MergeResource("GridDocumentGroupContainerControlStyle.xaml");
        }

        private void MergeResource(string name)
        {
            ResourceDictionary resourceDictionary = LoadResource<ResourceDictionary>(name);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private T LoadResource<T>(string xamlName)
        {
            var uri = new Uri(Assembly.GetExecutingAssembly().GetName().Name + ";component/" + xamlName, UriKind.Relative);
            return (T)Application.LoadComponent(uri);
        }


        #endregion
    }
}
