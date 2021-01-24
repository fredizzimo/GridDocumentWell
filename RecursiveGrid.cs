using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridDocumentWell
{
    public class RecursiveGrid<ElementType>
    {
        public enum NodeType
        {
            Element,
            Vertical,
            Horizontal,
        }

        public interface INode
        {
            NodeType Type { get; }
            ElementType Element { get; }
            IEnumerable<INode> Children { get; }
        }

        public INode Root
        {
            get { return _root;  }
        }

        public RecursiveGrid(ElementType initialElement)
        {
            _root = new LeafNode(initialElement);
        }


        class LeafNode : INode
        {
            public LeafNode(ElementType element)
            {
                Element = element;
            }
            public NodeType Type => NodeType.Element;

            public ElementType Element
            {
                get;
                private set;
            }

            public IEnumerable<INode> Children => Enumerable.Empty<INode>();
        }


        INode _root;
    }
}
