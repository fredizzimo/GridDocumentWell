using NUnit.Framework;
using GridDocumentWell;
using System.Collections.Generic;
using FluentAssertions;

using IntGrid = GridDocumentWell.RecursiveGrid<int>;
using IntNode = GridDocumentWell.RecursiveGrid<int>.INode;
using TestIntNode = TestGridDocumentWell.TestGridNode<int>;
using FluentAssertions.Equivalency;

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
        EquivalencyAssertionOptions<TExpectation> NodeCompareOptions<TExpectation>(EquivalencyAssertionOptions<TExpectation> options)
        {
            return options.WithStrictOrdering();
        }

        [Test]
        public void OneElementGrid()
        {
            var grid = new IntGrid(1);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Element,
                Element = 1
            };
            grid.Root.Should().BeEquivalentTo(expected, NodeCompareOptions);
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
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 2
                    }
                }
            };
            grid.Root.Should().BeEquivalentTo(expected, NodeCompareOptions);
        }

        [Test]
        public void SplitVerticallyTwice()
        {
            var grid = new IntGrid(1);
            grid.SplitVertically(1, 2);
            grid.SplitVertically(2, 3);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Vertical,
                ChildList = new List<TestIntNode>
                {
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 1
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 2
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 3
                    }
                }
            };
            grid.Root.Should().BeEquivalentTo(expected, NodeCompareOptions);
        }

        [Test]
        public void SplitFirstElementVertically()
        {
            var grid = new IntGrid(1);
            grid.SplitVertically(1, 2);
            grid.SplitVertically(1, 3);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Vertical,
                ChildList = new List<TestIntNode>
                {
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 1
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 3
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 2
                    },
                }
            };
            grid.Root.Should().BeEquivalentTo(expected, NodeCompareOptions);
        }

        [Test]
        public void SplitHorizontally()
        {
            var grid = new IntGrid(1);
            grid.SplitHorizontally(1, 2);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Horizontal,
                ChildList = new List<TestIntNode>
                {
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 1
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 2
                    }
                }
            };
            grid.Root.Should().BeEquivalentTo(expected, NodeCompareOptions);
        }

        [Test]
        public void SplitVerticallyThenHorizontally()
        {
            var grid = new IntGrid(1);
            grid.SplitVertically(1, 2);
            grid.SplitHorizontally(2, 3);
            IntNode expected = new TestIntNode
            {
                Type = NodeType.Vertical,
                ChildList = new List<TestIntNode>
                {
                    new TestIntNode
                    {
                        Type = NodeType.Element,
                        Element = 1
                    },
                    new TestIntNode
                    {
                        Type = NodeType.Horizontal,
                        ChildList = new List<TestIntNode>
                        {
                            new TestIntNode
                            {
                                Type = NodeType.Element,
                                Element = 2
                            },
                            new TestIntNode
                            {
                                Type = NodeType.Element,
                                Element = 3
                            }
                        }
                    },
                }
            };
            grid.Root.Should().BeEquivalentTo(expected, NodeCompareOptions);
        }
    }
}