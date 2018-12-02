using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue
{
    private int _limit;
    private LinkedList<string> _queue;

    public MessageQueue(int limit)
    {
        _limit = limit;
        _queue = new LinkedList<string>();
    }

    public void Clear()
    {
        _queue.Clear();
    }

    public int Count { get { return _queue.Count; } }

    public void AddEntry(string item)
    {
        Debug.Log("LOG: " + item);
        _queue.AddLast(item);
        while(_queue.Count > _limit)
        {
            _queue.RemoveFirst();
        }
    }

    public string PeekFirst()
    {
        return _queue.First.Value;
    }

    public string PeekLast()
    {
        return _queue.Last.Value;
    }

    public string[] PeekLastN(int n)
    {
        if (n == 1) return new string[] { PeekLast() };

        string[] sublist = new string[n];
        LinkedListNode<string> node = _queue.Last;
        for (int i = n - 1; i >= 0; --i)
        {
            sublist[i] = node.Value;
            node = node.Previous;
        }
        return sublist;
    }
}
