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

    public string[] Peek(int n)
    {
        if (n == 1) return new string[] { Peek1() };

        string[] sublist = new string[n];
        LinkedListNode<string> node = _queue.First;
        for(int i = 0; i < n; ++i)
        {
            sublist[i] = node.Value;
            node = node.Next;
        }
        return sublist;
    }

    public string Peek1()
    {
        return _queue.First.Value;
    }
}
