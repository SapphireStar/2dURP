using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Point:IEquatable<Point>
{
    [SerializeField]
    private int m_x;
    public int X
    {
        get => m_x;
        set => m_x = value;
    }
    [SerializeField]
    private int m_y;
    public int Y
    {
        get => m_y;
        set => m_y = value;
    }
    public Point(int x, int y)
    {
        m_x = x;
        m_y = y;
    }
    
    public override bool Equals(object point)
    {
        Point p = (Point)point;

        return (m_x == p.m_x) && (m_y == p.m_y);
    }
    public bool Equals(Point point)
    {
        return (m_x == point.m_x) && (m_y == point.m_y);
    }
    public override int GetHashCode()
    {
        return m_x ^ m_y;
    }
    public override string ToString()
    {
        return $"({m_x}, {m_y})";
    }
    public static float Distance(Point a, Point b)
    {
        return MathF.Sqrt(MathF.Pow(a.X - b.X, 2) + MathF.Pow(a.Y - b.Y, 2));
    }
}
