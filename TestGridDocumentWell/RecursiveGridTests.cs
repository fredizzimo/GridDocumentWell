using NUnit.Framework;
using GridDocumentWell;
using System.Collections.Generic;
using FluentAssertions;

namespace TestGridDocumentWell
{
    class TestGridNode<ElementType> : RecursiveGrid<ElementType>.INode
    {
        public TestGridNode()
        {
            _children = new List<TestGridNode<ElementType>>();

        }

        public RecursiveGrid<ElementType>.NodeType Type
        {
            get; set;
        }

        public ElementType Element
        {
            get; set;
        }

        public IEnumerable<RecursiveGrid<ElementType>.INode> Children
        {
            get { return _children; }
        }

        readonly List<TestGridNode<ElementType>> _children;
    }

    public class RecursiveGridTests
    {
        [Test]
        public void OneElementGrid()
        {
            var grid = new RecursiveGrid<int>(1);
            var expected = new TestGridNode<int>
            {
                Type = RecursiveGrid<int>.NodeType.Element,
                Element = 1
            };
            grid.Root.Should().BeEquivalentTo(expected);
        }
    }
}