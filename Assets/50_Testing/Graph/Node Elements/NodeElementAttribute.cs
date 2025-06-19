using System;

public class NodeElementAttribute : Attribute
{
    private NodeType _elementType;
    public NodeType ElementType => _elementType;

    public NodeElementAttribute(NodeType elementType)
    {
        _elementType = elementType;
    }
}
