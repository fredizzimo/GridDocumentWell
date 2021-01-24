using NUnit.Framework;
using GridDocumentWell;
using System.Collections.Generic;
using FluentAssertions;

using IntGrid = GridDocumentWell.RecursiveGrid<int>;
using IntNode = GridDocumentWell.RecursiveGrid<int>.INode;
using TestIntNode = TestGridDocumentWell.TestGridNode<int>;

namespace TestGridDocumentWell
{
    class TestGridNode<ElementType> : RecursiveGrid<ElementType>.INode
    {
        public TestGridNode()
        {
            ChildList = new List<TestGridNode<ElementType>>();
        }

        public NodeType Type
        {
            get; set;
        }

        public ElementType Element
        {
            get; set;
        }

        public IEnumerable<RecursiveGrid<ElementType>.INode> Children
        {
            get { return ChildList; }
        }

        public List<TestGridNode<ElementType>> ChildList
        {
            get; set;
        }
    }

    public class RecursiveGridTests
    {
        [Test]
        public void OneElementGrid()
        {
            var grid = new IntGrid(1);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Element,
                Element = 1
            };
            grid.Root.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void SplitVertically()
        {
            var grid = new IntGrid(1);
            grid.SplitVertically(1, 2);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Vertical,
                ChildList = new List<TestIntNode>
                {
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 1
                    }
                }
            };
            grid.Root.Should().BeEquivalentTo(expected);
        }
    }
}