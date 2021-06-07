using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public struct QTNodeRect
{
    public int x;
    public int y;
    public int extentX;
    public int extentY;

    public QTNodeRect(int x, int y, int extentX, int extentY)
    {
        this.x = x;
        this.y = y;
        this.extentX = extentX;
        this.extentY = extentY;
    }


    public void DrawGizmos(Color color, float floatToIntMod)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(new Vector3(x - extentX, 0f, y - extentY) / floatToIntMod, new Vector3(x + extentX, 0f, y - extentY) / floatToIntMod);
        Gizmos.DrawLine(new Vector3(x + extentX, 0f, y - extentY) / floatToIntMod, new Vector3(x + extentX, 0f, y + extentY) / floatToIntMod);
        Gizmos.DrawLine(new Vector3(x + extentX, 0f, y + extentY) / floatToIntMod, new Vector3(x - extentX, 0f, y + extentY) / floatToIntMod);
        Gizmos.DrawLine(new Vector3(x - extentX, 0f, y + extentY) / floatToIntMod, new Vector3(x - extentX, 0f, y - extentY) / floatToIntMod);
    }
}


public struct QTElementRect
{
    public int x1;
    public int x2;
    public int y1;
    public int y2;

    public QTElementRect(int x1, int x2, int y1, int y2)
    {
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;
    }


    public override string ToString()
    {
        return string.Format("Rect : {0},{1},{2},{3}", x1,y1,x2,y2);
    }
}



public interface IQTVisitor
{
    void Branch(QuadTree qt, int node, int depth, int mx, int my, int sx, int sy);

    void Leaf(QuadTree qt, int node, int depth, int mx, int my, int sx, int sy);
}



public class QuadTree : IEnumerable<IntList>
{
    static class NodeFields
    {
        public const int firstChild = 0;
        public const int count = 1;
    }


    static class ElementNodeFields
    {
        public const int nextIndex = 0;
        public const int index = 1;
    }

    const int nodeDataSize = 6;
    static class NodeDataFields
    {
        public const int index = 0;
        public const int depth = 1;
        public const int x = 2;
        public const int y = 3;
        public const int extentX = 4;
        public const int extentY = 5;
    }


    static class ElementFields
    {
        public const int id = 0;
        public const int nodeId = 1;
        public const int x1 = 2;
        public const int y1 = 3;
        public const int x2 = 4;
        public const int y2 = 5;
    }

    const int FLOAT_TO_INT_SCALE = 100000;

    IntList nodes = new IntList(2);
    IntList elementNodes = new IntList(2);
    IntList elements = new IntList(6);


    int maxElements;
    int maxDepth;

    QTNodeRect rootRect;


    public QuadTree(WorldPlane plane, int maxElements, int maxDepth)
    {
        int x = (int)(plane.origin.x * FLOAT_TO_INT_SCALE);
        int y = (int)(plane.origin.z * FLOAT_TO_INT_SCALE);
        int sizeX = (int)(plane.size.x * FLOAT_TO_INT_SCALE);
        int sizeY = (int)(plane.size.z * FLOAT_TO_INT_SCALE);

        QTNodeRect rect = new QTNodeRect(x, y, sizeX, sizeY);

        this.maxElements = maxElements;
        this.maxDepth = maxDepth;
        this.rootRect = rect;


        nodes.Insert();
        nodes.Set(0, NodeFields.firstChild, -1);
        nodes.Set(0, NodeFields.count, 0);
    }


    public void Clear()
    {
        nodes.Clear();
        elementNodes.Clear();
        elements.Clear();

        nodes.Insert();
        nodes.Set(0, NodeFields.firstChild, -1);
        nodes.Set(0, NodeFields.count, 0);
    }


    public int AddElement(int id, float x1, float x2, float y1, float y2)
    {
        QTElementRect rect = new QTElementRect();
        rect.x1 = (int)x1 * (FLOAT_TO_INT_SCALE);
        rect.x2 = (int)x2 * (FLOAT_TO_INT_SCALE);
        rect.y1 = (int)y1 * (FLOAT_TO_INT_SCALE);
        rect.y2 = (int)y2 * (FLOAT_TO_INT_SCALE);

        return AddElement(id, rect);
    }


    public int AddElement(int id, QTElementRect rect)
    {
        int newElement = elements.Insert();

        elements.Set(newElement, ElementFields.x1, rect.x1);
        elements.Set(newElement, ElementFields.y1, rect.y1);
        elements.Set(newElement, ElementFields.x2, rect.x2);
        elements.Set(newElement, ElementFields.y2, rect.y2);
        elements.Set(newElement, ElementFields.id, id);
        elements.Set(newElement, ElementFields.nodeId, -1);

        InsertElementToNode(0, 0, rootRect, newElement);

        return newElement;
    }



    void InsertElementToNode(int index, int depth, QTNodeRect rect, int elementId)
    {
        QTElementRect elementRect = new QTElementRect();
        elementRect.x1 = elements.Get(elementId, ElementFields.x1);
        elementRect.y1 = elements.Get(elementId, ElementFields.y1);
        elementRect.x2 = elements.Get(elementId, ElementFields.x2);
        elementRect.y2 = elements.Get(elementId, ElementFields.y2);

        IntList leaves = FindLeaves(index, depth, rect, elementRect);
        for(int i = 0; i < leaves.Count; ++i)
        {
            int leafIndex = leaves.Get(i, NodeDataFields.index);
            int leafDepth = leaves.Get(i, NodeDataFields.depth);
            QTNodeRect leafRect = new QTNodeRect();
            leafRect.x = leaves.Get(i, NodeDataFields.x);
            leafRect.y = leaves.Get(i, NodeDataFields.y);
            leafRect.extentX = leaves.Get(i, NodeDataFields.extentX);
            leafRect.extentY = leaves.Get(i, NodeDataFields.extentY);
            InsertElementToLeaf(leafIndex, leafDepth, leafRect, elementId);
        }
        leaves.Clear();
    }

    IntList tempEltsList = new IntList(1);
    void InsertElementToLeaf(int nodeIndex, int depth, QTNodeRect rect, int elementId)
    {
        int firstChildren = nodes.Get(nodeIndex, NodeFields.firstChild);
        nodes.Set(nodeIndex, NodeFields.firstChild, elementNodes.Insert());
        elementNodes.Set(nodes.Get(nodeIndex, NodeFields.firstChild), ElementNodeFields.nextIndex, firstChildren);
        elementNodes.Set(nodes.Get(nodeIndex, NodeFields.firstChild), ElementNodeFields.index, elementId);
        elements.Set(elementId, ElementFields.nodeId, nodeIndex);
        if (nodes.Get(nodeIndex, NodeFields.count) == maxElements && depth < maxDepth)
        {
            tempEltsList.Clear();

            while(nodes.Get(nodeIndex, NodeFields.firstChild) != -1)
            {
                int index = nodes.Get(nodeIndex, NodeFields.firstChild);
                int nextIndex = elementNodes.Get(index, ElementNodeFields.nextIndex);
                int element = elementNodes.Get(index, ElementNodeFields.index);

                nodes.Set(nodeIndex, NodeFields.firstChild, nextIndex);
                elementNodes.Erase(index);

                tempEltsList.Set(tempEltsList.PushBack(), 0, element);

            }
            int newFirstChild = nodes.Insert();
            nodes.Insert();
            nodes.Insert();
            nodes.Insert();
            nodes.Set(nodeIndex, NodeFields.firstChild, newFirstChild);

            for(int i = 0; i < 4; ++i)
            {
                nodes.Set(newFirstChild + i, NodeFields.firstChild, -1);
                nodes.Set(newFirstChild + i, NodeFields.count, 0);
            }

            nodes.Set(nodeIndex, NodeFields.count, -1);
            for (int i = 0; i < tempEltsList.Count; ++i)
            {
                InsertElementToNode(nodeIndex, depth, rect, tempEltsList.Get(i, 0));
            }
        }
        else
        {
             nodes.Set(nodeIndex, NodeFields.count, nodes.Get(nodeIndex, NodeFields.count) + 1);
        }

    }

    public void Remove(int element)
    {
        int x1 = elements.Get(element, ElementFields.x1);
        int y1 = elements.Get(element, ElementFields.y1);
        int x2 = elements.Get(element, ElementFields.x2);
        int y2 = elements.Get(element, ElementFields.y2);

        QTElementRect rect = new QTElementRect(x1, x2, y1, y2);

        IntList leaves = FindLeaves(0, 0, rootRect, rect);

        for (int i = 0; i < leaves.Count; i++)
        {
            int nodeIndex = leaves.Get(i, NodeDataFields.index);

            int elementNode = nodes.Get(nodeIndex, NodeFields.firstChild);
            int previousNode = -1;
            while (elementNode != -1 && elementNodes.Get(elementNode, ElementNodeFields.index) != element)
            {
                previousNode = elementNode;
                elementNode = elementNodes.Get(elementNode, ElementNodeFields.nextIndex);
            }

            if(elementNode != -1)
            {
                int nextIndex = elementNodes.Get(elementNode, ElementNodeFields.nextIndex);
                if(previousNode == -1)
                {
                    nodes.Set(nodeIndex, NodeFields.firstChild, nextIndex);
                }
                else
                {
                    elementNodes.Set(previousNode, ElementNodeFields.nextIndex, nextIndex);
                }
                elementNodes.Erase(elementNode);
                nodes.Set(nodeIndex, NodeFields.count, nodes.Get(nodeIndex, NodeFields.count) - 1);
            }
        }

        elements.Erase(element);
        leaves.Clear();
    }


    public void Cleanup()
    {
        toProcessTempList.Clear();

        if(nodes.Get(0, NodeFields.count) == -1)
        {
            toProcessTempList.Set(toProcessTempList.PushBack(), 0, 0);
        }

        while (toProcessTempList.Count > 0)
        {
            int node = toProcessTempList.Get(toProcessTempList.Count-1, 0);
            int firstChildren = nodes.Get(node, NodeFields.firstChild);
            int emptyLeavesCount = 0;
            toProcessTempList.PopBack();

            for(int i = 0; i < 4; i++)
            {
                int child = firstChildren + i;
                
                if(nodes.Get(child, NodeFields.count) == 0)
                {
                    ++emptyLeavesCount;
                }
                else if(nodes.Get(child, NodeFields.count) == -1)
                {
                    toProcessTempList.Set(toProcessTempList.PushBack(), 0, child);
                }

                if(emptyLeavesCount == 4)
                {
                    nodes.Erase(firstChildren + 3);
                    nodes.Erase(firstChildren + 2);
                    nodes.Erase(firstChildren + 1);
                    nodes.Erase(firstChildren + 0);

                    nodes.Set(node, NodeFields.firstChild, -1);
                    nodes.Set(node, NodeFields.count, 0);
                }
            }
        }

    }


    IntList leavesTempList = new IntList(nodeDataSize);
    IntList toProcessTempList = new IntList(nodeDataSize);
    IntList FindLeaves(int node, int depth, QTNodeRect nodeRect, QTElementRect elementRect)
    {
        leavesTempList.Clear();
        toProcessTempList.Clear();
        PushNode(toProcessTempList, node, depth, nodeRect);
        while(toProcessTempList.Count > 0)
        {
            int backIndex = toProcessTempList.Count - 1;

            QTNodeRect processRect = new QTNodeRect();

            int processNodeIndex = toProcessTempList.Get(backIndex, NodeDataFields.index);
            int processNodeDepth = toProcessTempList.Get(backIndex, NodeDataFields.depth);
            processRect.x = toProcessTempList.Get(backIndex, NodeDataFields.x);
            processRect.y = toProcessTempList.Get(backIndex, NodeDataFields.y);
            processRect.extentX = toProcessTempList.Get(backIndex, NodeDataFields.extentX);
            processRect.extentY = toProcessTempList.Get(backIndex, NodeDataFields.extentY);
            toProcessTempList.PopBack();
            if (nodes.Get(processNodeIndex, NodeFields.count) != -1)
            {
                PushNode(leavesTempList, processNodeIndex, processNodeDepth, processRect);
            }
            else
            {
                int firstChildren = nodes.Get(processNodeIndex, NodeFields.firstChild);

                int extentX = processRect.extentX / 2;
                int extentY = processRect.extentY / 2;

                int xLeft = processRect.x - extentX;
                int xRight = processRect.x + extentX;
                int yBottom = processRect.y - extentY;
                int yTop = processRect.y + extentY;

                QTNodeRect rect;

                if(elementRect.y2 <= processRect.y)
                {
                    if(elementRect.x1 <= processRect.x)
                    {
                        rect = new QTNodeRect(xLeft, yBottom, extentX, extentY);
                        PushNode(toProcessTempList, firstChildren + 0, processNodeDepth + 1, rect);
                    }
                    if(elementRect.x2 > processRect.x)
                    {
                        rect = new QTNodeRect(xRight, yBottom, extentX, extentY);
                        PushNode(toProcessTempList, firstChildren + 1, processNodeDepth + 1, rect);
                    }
                }

                if (elementRect.y1 > processRect.y)
                {
                    if (elementRect.x1 <= processRect.x)
                    {
                        rect = new QTNodeRect(xLeft, yTop, extentX, extentY);
                        PushNode(toProcessTempList, firstChildren + 2, processNodeDepth + 1, rect);
                    }
                    if (elementRect.x2 > processRect.x)
                    {
                        rect = new QTNodeRect(xRight, yTop, extentX, extentY);
                        PushNode(toProcessTempList, firstChildren + 3, processNodeDepth + 1, rect);
                    }
                }
            }
        }
        return leavesTempList;
    }


    void PushNode(IntList nodesList, int nodeIndex, int nodeDepth, QTNodeRect rect)
    {
        int listIndex = nodesList.PushBack();
        nodesList.Set(listIndex, NodeDataFields.index, nodeIndex);
        nodesList.Set(listIndex, NodeDataFields.depth, nodeDepth);
        nodesList.Set(listIndex, NodeDataFields.x, rect.x);
        nodesList.Set(listIndex, NodeDataFields.y, rect.y);
        nodesList.Set(listIndex, NodeDataFields.extentX, rect.extentX);
        nodesList.Set(listIndex, NodeDataFields.extentY, rect.extentY);
    }

    bool[] temp = new bool[1000];
    int tempSize = 1000;
    public IntList Query(QTElementRect rect, int omitElement = -1)
    {
        IntList output = tempEltsList;
        output.Clear();

        if(tempSize < elements.Count)
        {
            tempSize = elements.Count;
            temp = new bool[tempSize]; 
        }

        IntList leaves = FindLeaves(0, 0, rootRect, rect);

        for (int i = 0; i < leaves.Count; i++)
        {
            int nodeIndex = leaves.Get(i, NodeDataFields.index);

            int elementNodeIndex = nodes.Get(nodeIndex, NodeFields.firstChild);

            while(elementNodeIndex != -1)
            {
                int element = elementNodes.Get(elementNodeIndex, ElementNodeFields.index);
                int left = elements.Get(element, ElementFields.x1);
                int top = elements.Get(element, ElementFields.y2);
                int right = elements.Get(element, ElementFields.x2);
                int bottom = elements.Get(element, ElementFields.y1);

                if(!temp[element] && element != omitElement && Intersect(rect.x1, rect.y2, rect.x2, rect.y1, left, top, right, bottom))
                {
                    output.Set(output.PushBack(), 0, element);
                    temp[element] = true;
                }
                elementNodeIndex = elementNodes.Get(elementNodeIndex, ElementNodeFields.nextIndex);
            }
        }

        for(int i = 0; i < output.Count; i++)
        {
            temp[output.Get(i, 0)] = false;
        }

        return output;
    }


    static bool Intersect(int l1, int t1, int r1, int b1,
                            int l2, int t2, int r2, int b2)
    {
        return l2 <= r1 && r2 >= l1 && t2 <= b1 && b2 >= t1;
    }

#if UNITY_EDITOR
    public void DrawGizmos()
    {
        if (nodes.Count > 0)
        {
            NodeDrawRecursive(0, 0, rootRect);
        }

    }


    void NodeDrawRecursive(int node, int depth, QTNodeRect nodeRect)
    {
        Vector3 center = new Vector3(nodeRect.x, 0f, nodeRect.y);
        if (nodes.Get(node, NodeFields.count) != -1)
        {
            Handles.Label(center, "" + nodes.Get(node, NodeFields.count));
        }
        nodeRect.DrawGizmos(Color.green, FLOAT_TO_INT_SCALE);

        int firstChildren = nodes.Get(node, NodeFields.firstChild);
        int elementsCount = nodes.Get(node, NodeFields.count);
        QTNodeRect rect = new QTNodeRect();


        int extentX = nodeRect.extentX / 2;
        int extentY = nodeRect.extentY / 2;

        int xLeft = nodeRect.x - extentX;
        int xRight = nodeRect.x + extentX;
        int yBottom = nodeRect.y - extentY;
        int yTop = nodeRect.y + extentY;

        if (elementsCount < 0)
        {
            depth += 1;
            NodeDrawRecursive(firstChildren, depth, new QTNodeRect(xLeft, yBottom, extentX, extentY));
            NodeDrawRecursive(firstChildren + 1, depth, new QTNodeRect(xRight, yBottom, extentX, extentY));
            NodeDrawRecursive(firstChildren + 2, depth, new QTNodeRect(xLeft, yTop, extentX, extentY));
            NodeDrawRecursive(firstChildren + 3, depth, new QTNodeRect(xRight, yTop, extentX, extentY));
        }
    }

#endif



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int FloatToInt(float x)
    {
        return (int)(x * FLOAT_TO_INT_SCALE);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float IntToFloat(int y)
    {
        return ((float)y / FLOAT_TO_INT_SCALE);
    }


    IntList iterationTempList = new IntList(1);
    IEnumerator<IntList> IEnumerable<IntList>.GetEnumerator()
    {
        return GetGroupEnumerator();
    }


    public QTElementRect GetElementRect(Vector3 point, float extents)
    {
        QTElementRect elementRect = new QTElementRect()
        {
            x1 = FloatToInt(point.x - extents),
            x2 = FloatToInt(point.x + extents),
            y1 = FloatToInt(point.z + extents),
            y2 = FloatToInt(point.z - extents)
        };
        return elementRect;
    }


    public IEnumerator GetEnumerator()
    {
        return GetGroupEnumerator();
    }


    IEnumerator<IntList> GetGroupEnumerator()
    {
        toProcessTempList.Clear();
        PushNode(toProcessTempList, 0, 0, rootRect);

        while (toProcessTempList.Count > 0)
        {
            iterationTempList.Clear();
            int backIndex = toProcessTempList.Count - 1;
            int nodeIndex = toProcessTempList.Get(backIndex, NodeDataFields.index);
            int firstChildren = nodes.Get(nodeIndex, NodeFields.firstChild);
            toProcessTempList.PopBack();

            if (nodes.Get(nodeIndex, NodeFields.count) != -1)
            {
                int elementNodeIndex = nodes.Get(nodeIndex, NodeFields.firstChild);
                while (elementNodeIndex != -1)
                {
                    int element = elementNodes.Get(elementNodeIndex, ElementNodeFields.index);

                    int i = iterationTempList.Insert();
                    iterationTempList.Set(i, 0, element);
                    elementNodeIndex = elementNodes.Get(elementNodeIndex, ElementNodeFields.nextIndex);
                }
                if (iterationTempList.Count == 0)
                    continue;
                yield return iterationTempList;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int index = toProcessTempList.PushBack();
                    toProcessTempList.Set(index, NodeDataFields.index, firstChildren + i);
                }
            }
        }
    }
}
