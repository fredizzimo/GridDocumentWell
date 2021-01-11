using Microsoft.VisualStudio.PlatformUI.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridDocumentWell
{
    class GridViewElementFactory : DelegatingViewElementFactory
    {
        public GridViewElementFactory() : base(ViewElementFactory.Current)
        {
        }
        
        protected override DocumentGroupContainer CreateDocumentGroupContainerCore()
        {
            return new GridDocumentGroupContainer();
        }
    }
}
