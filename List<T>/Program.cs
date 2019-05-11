using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework30
{
    public class MyCollection<T> : IEnumerable
    {
        private int _capacity;
        private T[] _arr;
        private int _count;

        public MyCollection()
        {
            _capacity = 16;
            _arr = new T[_capacity];
            _count = 0;
        }
        public MyCollection(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("Capacity is less than 0.");
            _capacity = capacity;
            _arr = new T[_capacity];
            _count = 0;
        }
        public MyCollection(MyCollection<T> coll)
        {
            if (coll._count == 0)
                throw new ArgumentNullException("Collection is null.");
            _capacity = coll._capacity;
            _arr = new T[_capacity];
            coll._arr.CopyTo(_arr, 0);
            _count = coll._count;
        }
        public int Count
        {
            get { return _count; }
        }
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value < _count)
                    throw new ArgumentOutOfRangeException("Capacity is set to a value that is less than count.");
                else if (value > _capacity)
                    throw new OutOfMemoryException("There is not enough memory available on the system.");
                _capacity = value;
            }
        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("Index is less than 0 or index is equal to or greater than count.");
                return _arr[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("Index is less than 0 or index is equal to or greater than count.");
                _arr[index] = value;
            }
        }
        public void Add(T item)
        {
            if (_capacity == _count)
            {
                _capacity *= 2;
                Array.Resize<T>(ref _arr, _capacity);
            }
            _arr[_count] = item;
            _count++;
        }
        public void AddRange(MyCollection<T> coll)
        {
            for (var i = 0; i < coll.Count; i++)
            {
                this.Add(coll[i]);
            }
        }
        public void Insert(int index, T item)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("Index is less than 0 or index is greater than count.");
            if (_capacity == _count)
            {
                _capacity *= 2;
                Array.Resize<T>(ref _arr, _capacity);
            }
            for (var i = _count; i != index; i--)
            {
                _arr[i] = _arr[i - 1];
            }
            _arr[index] = item;
            _count++;
        }
        public void InsertRange(int index, MyCollection<T> coll)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("Index is less than 0 or index is greater than count.");
            else if (coll._count == 0 || _count == 0)
                throw new ArgumentNullException("Collection is null");
            var tempCount = _count + coll._count;
            var tempCapacity = _capacity;
            while (_capacity < tempCount)
                _capacity *= 2;
            if (tempCapacity != _capacity)
                Array.Resize<T>(ref _arr, _capacity);
            var i = tempCount - 1;
            for (var j = _count - 1; j != index; i--, j--)
                _arr[i] = _arr[j];
            i = index;
            for (var j = 0; j < coll._count; i++, j++)
                _arr[i] = coll._arr[j];
            _count = tempCount;
        }
        public bool Remove(T item)
        {
            var removeIndex = Array.IndexOf(_arr, item);
            if (removeIndex == -1)
                return false;
            for (var i = removeIndex; i <= _count - 2; i++)
            {
                _arr[i] = _arr[i + 1];
            }
            _count--;
            if (_count * 2.5 < _capacity)
            {
                _capacity /= 2;
                Array.Resize(ref _arr, _capacity);
            }
            return true;
        }
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException("Index is less than 0 or count is less than 0.");
            if (_count < index + count)
                throw new ArgumentException("Index and count do not denote a valid range of elements.");
            if (index + count < _count)
            {
                for (var i = index + count; i < _count; i++)
                    _arr[i - count] = _arr[i];
            }
            _count -= count;
            if (_count * 2.5 < _capacity)
            {
                _capacity /= 2;
                Array.Resize(ref _arr, _capacity);
            }
        }
        public int IndexOf(T item, int index, int count)
        {
            if (index < 0 || index >= _count || count < 0 || _count < index + count)
                throw new ArgumentOutOfRangeException("Index is outside the range of valid indexes " +
                    "or count is less than 0 " +
                    "or index and count do not specify a valid section.");
            for (var i = index; i < index + count; i++)
            {
                if ((object)_arr[i] == (object)item)
                    return i;
            }
            return -1;
        }
        public int IndexOf(T item, int index = 0)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException("Index is outside the range of valid indexes");
            for (; index < _count; index++)
            {
                if ((object)_arr[index] == (object)item)
                    return index;
            }
            return -1;
        }
        public void Reverse(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException("Index is less than 0 or count is less than 0.");
            if (_count < index + count)
                throw new ArgumentException("Index and count do not denote a valid range of elements.");
            Array.Reverse(_arr, index, count);
        }
        public void Reverse()
        {
            Array.Reverse(_arr);
        }
        public IEnumerator GetEnumerator()
        {
            return new MyNumerator(this);
        }
        public struct MyNumerator : IEnumerator
        {
            private MyCollection<T> _coll;
            private int _index;

            public MyNumerator(MyCollection<T> coll)
            {
                _coll = coll;
                _index = -1;
            }
            public object Current
            {
                get { return _coll[_index]; }
            }
            public bool MoveNext()
            {
                if (_index == _coll.Count - 1)
                {
                    Reset();
                    return false;
                }
                _index++;
                return true;
            }
            public void Reset()
            {
                _index = -1;
            }
        }
        public void Sort()
        {
            Array.Sort<T>(_arr, 0, _count);
        }
    }

    class Program
    {

    }
}
