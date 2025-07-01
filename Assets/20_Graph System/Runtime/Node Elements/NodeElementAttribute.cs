using System;

public class NodeElementAttribute : Attribute
{
    private NodeType[] _elementTypes;
    public NodeType[] ElementTypes => _elementTypes;

    public NodeElementAttribute(NodeType elementType)
    {
        _elementTypes = new NodeType[] { elementType };
    }

    public NodeElementAttribute(NodeType elementType1, NodeType elementType2)
    {
        _elementTypes = new NodeType[] { elementType1, elementType2 };
    }

    public NodeElementAttribute(NodeType[] elementTypes)
    {
        _elementTypes = elementTypes;
    }
}
