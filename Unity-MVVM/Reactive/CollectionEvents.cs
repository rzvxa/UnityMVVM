using System;
using System.Collections.Generic;

namespace UnityMVVM.Reactive
{
    public struct CollectionAddEvent : IEquatable<CollectionAddEvent>
    {
        public int Index { get; }

        public object Value { get; }

        public CollectionAddEvent(int index, object value) : this()
        {
            Index = index;
            Value = value;
        }

        public override string ToString()
        {
            return $"Index:{Index} Value:{Value}";
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ EqualityComparer<object>.Default.GetHashCode(Value) << 2;
        }

        public bool Equals(CollectionAddEvent other)
        {
            return Index.Equals(other.Index) && EqualityComparer<object>.Default.Equals(Value, other.Value);
        }

    }

    public struct CollectionRemoveEvent : IEquatable<CollectionRemoveEvent>
    {
        public int Index { get; }

        public object Value { get; }

        public CollectionRemoveEvent(int index, object value) : this()
        {
            Index = index;
            Value = value;
        }

        public override string ToString()
        {
            return $"Index:{Index} Value:{Value}";
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ EqualityComparer<object>.Default.GetHashCode(Value) << 2;
        }

        public bool Equals(CollectionRemoveEvent other)
        {
            return Index.Equals(other.Index) && EqualityComparer<object>.Default.Equals(Value, other.Value);
        }
    }

    public struct CollectionReplaceEvent : IEquatable<CollectionReplaceEvent>
    {
        public int Index { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        public CollectionReplaceEvent(int index, object oldValue, object newValue)
            : this()
        {
            Index = index;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override string ToString()
        {
            return $"Index:{Index} OldValue:{OldValue} NewValue:{NewValue}";
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ EqualityComparer<object>.Default.GetHashCode(OldValue) << 2 ^ EqualityComparer<object>.Default.GetHashCode(NewValue) >> 2;
        }

        public bool Equals(CollectionReplaceEvent other)
        {
            return Index.Equals(other.Index)
                && EqualityComparer<object>.Default.Equals(OldValue, other.OldValue)
                && EqualityComparer<object>.Default.Equals(NewValue, other.NewValue);
        }
    }
    
    public struct CollectionMoveEvent : IEquatable<CollectionMoveEvent>
    {
        public int OldIndex { get; private set; }
        public int NewIndex { get; private set; }
        public object Value { get; private set; }

        public CollectionMoveEvent(int oldIndex, int newIndex, object value)
            : this()
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            Value = value;
        }

        public override string ToString()
        {
            return $"OldIndex:{OldIndex} NewIndex:{NewIndex} Value:{Value}";
        }

        public override int GetHashCode()
        {
            return OldIndex.GetHashCode() ^ NewIndex.GetHashCode() << 2 ^ EqualityComparer<object>.Default.GetHashCode(Value) >> 2;
        }

        public bool Equals(CollectionMoveEvent other)
        {
            return OldIndex.Equals(other.OldIndex) && NewIndex.Equals(other.NewIndex) && EqualityComparer<object>.Default.Equals(Value, other.Value);
        }
    }
}
