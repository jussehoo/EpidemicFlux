﻿// (C) 2018, single-linked list

// TODO
//		- Insert: before or after curret?
//		- Exceptions and exception tests.
//		- Prevent concurrent modification. List operations and iterators have unique IDs.
//		- Add/remove by index.
//		- Iterator/enumerator with a condition.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class MListNode<T>
{
	public T Value;
	public MListNode<T> Next;
}

public class MListIterator<T>
{
	private MList<T> list;
	private MListNode<T> previous, current, next;

	public T Value
	{
		set { current.Value = value; }
		get { return current.Value; }
	}
	public MListIterator(MList<T> _list, MListNode<T> _previous)
	{
		list = _list;
		previous = _previous;
		current = null;
		next = _previous.Next;
	}
	public bool HasValue()
	{
		return current != null;
	}
	public bool HasNext()
	{
		return next != null;
	}
	public bool Next()
	{
		if (next == null) return false;
		if (current != null) previous = current; // current is null at the beginning and after Remove
		current = next;
		next = current.Next;
		return true;
	}
	public void Remove()
	{
		Debug.Assert(current != null);
		list.Remove(this);
	}
	public void InsertAfter(T t)
	{
		list.InsertAfter(current, t);
		Next();
	}
	public void InsertBefore(T t)
	{
		Debug.Assert(current != null); // can't be beginning or end of list, or just deleted
		previous = list.InsertAfter(previous, t); // add new node and update iterator's previous
	}
	public bool Finished()
	{
		return (next == null);
	}
	public void _Skip() // internal operation to do on Remove
	{
		if (next == null)
		{
			previous.Next = null; // end of list
			next = current = previous = null;
			return;
		}
		previous.Next = next;
		current = null;
	}
	public MListNode<T> _CurrentNode()
	{
		return current;
	}
	public MListNode<T> _PreviousNode()
	{
		return previous;
	}
	public void AssertValid()
	{
		if (previous != null)
		{
			if (current == null) Debug.Assert(previous.Next == next);
			else Debug.Assert(previous.Next == current);
		}
		else if (current != null)
		{
			Debug.Assert(current.Next == next);
		}
		else
		{
			Debug.Assert(next == null);
		}
	}
}

// standard enumerator for 'foreach' e.g.
public class MListEnumerator<T> : IEnumerable<T>
{
	private MList<T> list; MListIterator<T> it;
	public MListEnumerator(MList<T> _list) { list = _list; it = list.Iterator(); }
	IEnumerator IEnumerable.GetEnumerator()	{ yield return list.GetEnumerator(); }
	IEnumerator<T> IEnumerable<T>.GetEnumerator() { while (it.Next()) yield return it.Value; }
	public T Current { get { return it.Value; } }
	public bool MoveNext() { return it.Next(); }
}


public class MList<T>
{
	private int size = 0;
	private MListNode<T>
		root, // first, empty (dummy) node
		tail; // last node, containing real data
	
	public MList() { root = new MListNode<T>(); tail = null; }
	public MListEnumerator<T> GetEnumerator() {	return new MListEnumerator<T>(this); }
	public MListIterator<T> Iterator() { return new MListIterator<T>(this, root); }
	public T First() { return root.Next.Value; }
	public T Last()	{ return tail.Value; }
	public int Size() { return size; }

	public void AddFirst(T t)
	{
		if (Size() == 0)
		{
			AddLast(t);
			return;
		}
		var it = Iterator();
		it.Next();
		it.InsertBefore(t);
	}
	public void AddLast(T t)
	{
		MListNode<T> node = new MListNode<T> { Value = t };
		if (size == 0) root.Next = node;
		else tail.Next = node;
		tail = node;
		size++;
	}
	public void RemoveFirst()
	{
		if (size == 0) return;
		if (size == 1) root.Next = tail = null;
		else root.Next = root.Next.Next;
		size--;
	}
	//public void RemoveLast()
	//{
	//	// Difficult because new tail can't be defined
	//}
	public void RemoveAll()
	{
		while (size > 0) RemoveFirst();
	}
	public void Remove(MListIterator<T> it)
	{
		if (it._CurrentNode() == tail)
		{
			tail = it._PreviousNode();
		}
		it._Skip();
		size--;
		if (size == 0) root.Next = tail = null;
	}
	public MListNode<T> InsertAfter(MListNode<T> position, T t)
	{
		// add a node after 'position', which can't be null
		// special cases: first, only one, last
		Debug.Assert(position != null);
		MListNode<T> node = new MListNode<T> { Value = t, Next = position.Next };
		position.Next = node;
		if (tail == position) tail = node;
		size++;
		return node;
	}

	public void AssertValid()
	{
		if (size == 0)
		{
			Debug.Assert(tail == null);
			Debug.Assert(root.Next == null);
		}
		else
		{
			int n = 0;
			var it = Iterator();
			while (it.Next())
			{
				it.AssertValid();
				n++;
			}
			Debug.Assert(it.Value.Equals(tail.Value));
			Debug.Assert(n == size);
			Debug.Assert(it.Finished());
		}
	}
}
