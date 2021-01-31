using Microsoft.VisualStudio.PlatformUI.Shell;
using System;

namespace GridDocumentWell
{
    class GridViewElementFactory : ViewElementFactory
    {
        public GridViewElementFactory()
        {
            _innerFactory = Current;
        }
        
        protected override DocumentGroupContainer CreateDocumentGroupContainerCore()
        {
            return new GridDocumentGroupContainer();
        }

        private readonly ViewElementFactory _innerFactory;

        protected override TabGroup CreateTabGroupCore()
        {
            return _innerFactory.CreateTabGroup();
        }

        protected override DockGroup CreateDockGroupCore()
        {
            return _innerFactory.CreateDockGroup();
        }

        protected override DocumentGroup CreateDocumentGroupCore()
        {
            return _innerFactory.CreateDocumentGroup();
        }

        protected override AutoHideGroup CreateAutoHideGroupCore()
        {
            return _innerFactory.CreateAutoHideGroup();
        }

        protected override AutoHideChannel CreateAutoHideChannelCore()
        {
            return _innerFactory.CreateAutoHideChannel();
        }

        protected override AutoHideRoot CreateAutoHideRootCore()
        {
            return _innerFactory.CreateAutoHideRoot();
        }

        protected override DockRoot CreateDockRootCore()
        {
            return _innerFactory.CreateDockRoot();
        }

        protected override FloatSite CreateFloatSiteCore()
        {
            return _innerFactory?.CreateFloatSite();
        }

        protected override MainSite CreateMainSiteCore()
        {
            return _innerFactory?.CreateMainSite();
        }

        protected override View CreateViewCore(Type viewType)
        {
            return _innerFactory?.CreateView(viewType);
        }

        protected override ViewBookmark CreateViewBookmarkCore()
        {
            return _innerFactory?.CreateViewBookmark();
        }
    }
}
