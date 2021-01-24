using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridDocumentWell
{
    public enum NodeType
    {
        Element,
        Vertical,
        Horizontal,
    }

    public class RecursiveGrid<ElementType>
    {

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

        public void SplitVertically(ElementType elementToSplit, ElementType newElement)
        {
            Split(NodeType.Vertical, elementToSplit, newElement);
        }

        public void SplitHorizontally(ElementType elementToSplit, ElementType newElement)
        {
            Split(NodeType.Horizontal, elementToSplit, newElement);
        }

        private void Split(NodeType mode, ElementType elementToSplit, ElementType newElement)
        {
            var result = FindElement(elementToSplit);
            if (result == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (result.parent == null)
            {
                // It's the root element
                var newRoot = new SplitNode(mode);
                newRoot.ChildList.Add(result.node);
                newRoot.ChildList.Add(new LeafNode(newElement));
                _root = newRoot;
            }
            else if (result.parent.Type == mode)
            {
                result.parent.ChildList.Insert(result.index + 1, new LeafNode(newElement));
            }
            else
            {
                var newSplit = new SplitNode(mode);
                newSplit.ChildList.Add(result.node);
                newSplit.ChildList.Add(new LeafNode(newElement));
                result.parent.ChildList[result.index] = newSplit;
            }
        }

        FindElementResult FindElement(ElementType element)
        {
            if (Root is LeafNode rootNode)
            {
                if (EqualityComparer<ElementType>.Default.Equals(rootNode.Element, element))
                {
                    return new FindElementResult { node = rootNode, parent = null, index = 0 };
                }
            }
            else if (Root is SplitNode split)
            {
                return FindElement(element, split);
            }
            return null;
        }

        FindElementResult FindElement(ElementType element, SplitNode parent)
        {
            for (int i = 0; i < parent.ChildList.Count; i++)
            {
                var child = parent.ChildList[i];
                if (child is LeafNode childNode)
                {
                    if (EqualityComparer<ElementType>.Default.Equals(childNode.Element, element))
                    {
                        return new FindElementResult { node = childNode, index = i, parent = parent };
                    }
                }
                else if (child is SplitNode split)
                {
                    return FindElement(element, split);
                }
            }
            return null;
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

        class SplitNode : INode
        {
            public SplitNode(NodeType nodeType)
            {
                Type = nodeType;
                ChildList = new List<INode>();
            }

            public NodeType Type
            {
                get;
                private set;
            }

            public ElementType Element
            {
                get;
                private set;
            }

            public IEnumerable<INode> Children
            {
                get
                {
                    return ChildList;
                }

            }

            public List<INode> ChildList
            {
                get;
                private set;
            }
        }

        class FindElementResult
        {
            public LeafNode node;
            public SplitNode parent;
            public int index;
        }

        INode _root;
    }
}
