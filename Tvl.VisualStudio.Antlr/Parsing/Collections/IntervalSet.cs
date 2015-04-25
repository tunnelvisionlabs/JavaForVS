namespace Tvl.VisualStudio.Language.Parsing.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    using ArgumentNullException = System.ArgumentNullException;
    using Math = System.Math;
    using NotImplementedException = System.NotImplementedException;
    using StringBuilder = System.Text.StringBuilder;
    using TokenTypes = Antlr.Runtime.TokenTypes;

    /** A set of integers that relies on ranges being common to do
     *  "run-length-encoded" like compression (if you view an IntSet like
     *  a BitSet with runs of 0s and 1s).  Only ranges are recorded so that
     *  a few ints up near value 1000 don't cause massive bitsets, just two
     *  integer intervals.
     *
     *  element values may be negative.  Useful for sets of EPSILON and EOF.
     *
     *  0..9 char range is index pair ['\u0030','\u0039'].
     *  Multiple ranges are encoded with multiple index pairs.  Isolated
     *  elements are encoded with an index pair where both intervals are the same.
     *
     *  The ranges are ordered and disjoint so that 2..6 appears before 101..103.
     */
    public class IntervalSet : ISet<int>
    {
        public static readonly Interval CompleteInterval = Interval.FromBounds(0, char.MaxValue);
        public static readonly IntervalSet COMPLETE_SET = IntervalSet.Of(CompleteInterval);

        /** The list of sorted, disjoint intervals. */
        private IList<Interval> intervals;

        /** Create a set with no elements */
        public IntervalSet()
        {
            intervals = new List<Interval>(2); // most sets are 1 or 2 elements
        }

        public IntervalSet(IList<Interval> intervals)
        {
            List<Interval> orderedIntervals = intervals.OrderBy(i => i.Start).ToList();
            for (int i = 0; i < orderedIntervals.Count - 1; i++)
            {
                Interval? union = orderedIntervals[i].Union(orderedIntervals[i + 1]);
                if (union != null)
                {
                    orderedIntervals[i] = union.Value;
                    orderedIntervals.RemoveAt(i + 1);
                    i--; // repeat this index
                }
            }

            this.intervals = orderedIntervals;
        }

        public int Count
        {
            get
            {
                return intervals.Sum(interval => interval.EndInclusive - interval.Start + 1);
            }
        }

        public IList<Interval> Intervals
        {
            get
            {
                return intervals;
            }
        }

        public int MaxElement
        {
            get
            {
                if (IsNil)
                    return TokenTypes.Invalid;

                Interval last = (Interval)intervals[intervals.Count - 1];
                return last.EndInclusive;
            }
        }

        /** Return minimum element >= 0 */
        public int MinElement
        {
            get
            {
                if (IsNil)
                    return TokenTypes.Invalid;

                int n = intervals.Count;
                for (int i = 0; i < n; i++)
                {
                    Interval I = (Interval)intervals[i];
                    int a = I.Start;
                    int b = I.EndInclusive;
                    for (int v = a; v <= b; v++)
                    {
                        if (v >= 0)
                            return v;
                    }
                }

                return TokenTypes.Invalid;
            }
        }

        /** If this set is a single integer, return it otherwise TokenTypes.Invalid */
        public int SingleElement
        {
            get
            {
                if (intervals != null && intervals.Count == 1)
                {
                    Interval I = (Interval)intervals[0];
                    if (I.Start == I.EndInclusive)
                        return I.Start;
                }

                return TokenTypes.Invalid;
            }
        }

        /** Create a set with a single element, el. */
        public static IntervalSet Of(int a)
        {
            return Of(new Interval(a, a));
        }

        /** Create a set with all ints within range [a..EndInclusive] (inclusive) */
        public static IntervalSet Of(int a, int b)
        {
            return Of(new Interval(a, b));
        }

        public static IntervalSet Of(Interval interval)
        {
            IntervalSet s = new IntervalSet(new List<Interval> { interval });
            return s;
        }

        /** Add a single element to the set.  An isolated element is stored
         *  as a range el..el.
         */
        public virtual void Add(int el)
        {
            Add(new Interval(el, 1));
        }

        /** Add interval; i.e., add all integers from a to b to set.
         *  If b&lt;a, do nothing.
         *  Keep list in sorted order (by left range value).
         *  If overlap, combine ranges.  For example,
         *  If this is {1..5, 10..20}, adding 6..7 yields
         *  {1..5, 6..7, 10..20}.  Adding 4..8 yields {1..8, 10..20}.
         */
        public virtual void Add(Interval interval)
        {
            AddImpl(interval);
        }

        // copy on write so we can cache a..Start intervals and sets of that
        public virtual void AddImpl(Interval addition)
        {
            //JSystem.@out.println("add "+addition+" to "+intervals.toString());
            if (addition.IsEmpty)
                return;

            // find position in list
            // Use iterators as we modify list in place
            //for ( ListIterator iter = intervals.listIterator(); iter.hasNext(); )
            for (int i = 0; i < intervals.Count; i++)
            {
                Interval r = intervals[i];
                if (addition.Equals(r))
                    return;

                Interval? union = addition.Union(r);
                if (union != null)
                {
                    // next to each other, make a single larger interval
                    intervals[i] = union.Value;

                    // make sure we didn't just create an interval that
                    // should be merged with next interval in list
                    if (i < intervals.Count - 1)
                    {
                        i++;
                        Interval next = intervals[i];
                        Interval? union2 = union.Value.Union(next);
                        if (union2 != null)
                        {
                            // if we bump up against or overlap next, merge
                            intervals.RemoveAt(i);
                            i--;
                            intervals[i] = union2.Value;
                        }
                    }
                    return;
                }

                if (addition.Start < r.Start)
                {
                    // insert before r
                    //iter.previous();
                    //iter.add( addition );
                    intervals.Insert(i, addition);
                    return;
                }

                // if disjoint and after r, a future iteration will handle it
            }
            // ok, must be after last interval (and disjoint from last interval)
            // just add it
            intervals.Add(addition);
        }

#if false
        protected virtual void Add( Interval addition )
        {
            //JSystem.@out.println("add "+addition+" to "+intervals.toString());
            if ( addition.EndInclusive < addition.Start )
            {
                return;
            }
            // find position in list
            //for (ListIterator iter = intervals.listIterator(); iter.hasNext();) {
            int n = intervals.Count;
            for ( int i = 0; i < n; i++ )
            {
                Interval r = (Interval)intervals[i];
                if ( addition.Equals( r ) )
                {
                    return;
                }
                if ( addition.adjacent( r ) || !addition.disjoint( r ) )
                {
                    // next to each other, make a single larger interval
                    Interval bigger = addition.union( r );
                    intervals[i] = bigger;
                    // make sure we didn't just create an interval that
                    // should be merged with next interval in list
                    if ( ( i + 1 ) < n )
                    {
                        i++;
                        Interval next = (Interval)intervals[i];
                        if ( bigger.adjacent( next ) || !bigger.disjoint( next ) )
                        {
                            // if we bump up against or overlap next, merge
                            intervals.RemoveAt( i ); // remove next one
                            i--;
                            intervals[i] = bigger.union( next ); // set to 3 merged ones
                        }
                    }
                    return;
                }
                if ( addition.startsBeforeDisjoint( r ) )
                {
                    // insert before r
                    intervals.Insert( i, addition );
                    return;
                }
                // if disjoint and after r, a future iteration will handle it
            }
            // ok, must be after last interval (and disjoint from last interval)
            // just add it
            intervals.Add( addition );
        }
#endif

        public virtual void UnionWith(IEnumerable<int> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            IntervalSet otherIntervalSet = other as IntervalSet;
            if (otherIntervalSet != null)
            {
                // walk set and add each interval
                foreach (Interval interval in otherIntervalSet.Intervals)
                    AddImpl(interval);

                return;
            }

            foreach (int i in other)
                Add(i);
        }

        /** Given the set of possible values (rather than, say UNICODE or MAXINT),
         *  return a new set containing all elements in vocabulary, but not in
         *  this.  The computation is (vocabulary - this).
         *
         *  'this' is assumed to be either a subset or equal to vocabulary.
         */
        public virtual IntervalSet Complement(Interval vocabulary)
        {
            if (vocabulary.EndInclusive < MinElement || vocabulary.Start > MaxElement)
            {
                // nothing in common with this set
                return null;
            }

            int n = intervals.Count;
            if (n == 0)
            {
                return IntervalSet.Of(vocabulary);
            }

            IntervalSet compl = new IntervalSet();

            Interval first = intervals[0];
            // add a range from 0 to first.Start constrained to vocab
            if (first.Start > vocabulary.Start)
            {
                compl.Intervals.Add(Interval.FromBounds(vocabulary.Start, first.Start - 1));
            }

            for (int i = 1; i < n; i++)
            {
                if (intervals[i - 1].EndInclusive >= vocabulary.EndInclusive)
                    break;

                if (intervals[i].Start <= vocabulary.Start)
                    continue;

                if (intervals[i - 1].EndInclusive == intervals[i].Start - 1)
                    continue;

                compl.Intervals.Add(Interval.FromBounds(Math.Max(vocabulary.Start, intervals[i - 1].EndInclusive + 1), Math.Min(vocabulary.EndInclusive, intervals[i].Start - 1)));

                //// from 2nd interval .. nth
                //Interval previous = intervals[i - 1];
                //Interval current = intervals[i];
                //IntervalSet s = IntervalSet.Of( previous.EndInclusive + 1, current.Start - 1 );
                //IntervalSet a = (IntervalSet)s.And( vocabularyIS );
                //compl.AddAll( a );
            }

            Interval last = intervals[n - 1];
            // add a range from last.EndInclusive to maxElement constrained to vocab
            if (last.EndInclusive < vocabulary.EndInclusive)
            {
                compl.Intervals.Add(Interval.FromBounds(last.EndInclusive + 1, vocabulary.EndInclusive));
                //IntervalSet s = IntervalSet.Of( last.EndInclusive + 1, maxElement );
                //IntervalSet a = (IntervalSet)s.And( vocabularyIS );
                //compl.AddAll( a );
            }

            return compl;
        }

        /** Compute this-other via this&amp;~other.
         *  Return a new set containing all elements in this but not in other.
         *  other is assumed to be a subset of this;
         *  anything that is in other but not in this will be ignored.
         */
        public virtual IntervalSet Except(IntervalSet other)
        {
            // assume the whole unicode range here for the complement
            // because it doesn't matter.  Anything beyond the max of this' set
            // will be ignored since we are doing this & ~other.  The intersection
            // will be empty.  The only problem would be when this' set max value
            // goes beyond MAX_CHAR_VALUE, but hopefully the constant MAX_CHAR_VALUE
            // will prevent this.
            return this.Intersect(other.Complement(CompleteInterval));
        }

#if false
        /** return a new set containing all elements in this but not in other.
         *  Intervals may have to be broken up when ranges in this overlap
         *  with ranges in other.  other is assumed to be a subset of this;
         *  anything that is in other but not in this will be ignored.
         *
         *  Keep around, but 10-20-2005, I decided to make complement work w/o
         *  subtract and so then subtract can simply be a&~b
         */
        public IIntSet Subtract( IIntSet other )
        {
            if ( other == null || !( other is IntervalSet ) )
            {
                return null; // nothing in common with null set
            }

            IntervalSet diff = new IntervalSet();

            // iterate down both interval lists
            var thisIter = this.intervals.GetEnumerator();
            var otherIter = ( (IntervalSet)other ).intervals.GetEnumerator();
            Interval mine = null;
            Interval theirs = null;
            if ( thisIter.MoveNext() )
            {
                mine = (Interval)thisIter.Current;
            }
            if ( otherIter.MoveNext() )
            {
                theirs = (Interval)otherIter.Current;
            }
            while ( mine != null )
            {
                //JSystem.@out.println("mine="+mine+", theirs="+theirs);
                // CASE 1: nothing in theirs removes a chunk from mine
                if ( theirs == null || mine.disjoint( theirs ) )
                {
                    // SUBCASE 1a: finished traversing theirs; keep adding mine now
                    if ( theirs == null )
                    {
                        // add everything in mine to difference since theirs done
                        diff.add( mine );
                        mine = null;
                        if ( thisIter.MoveNext() )
                        {
                            mine = (Interval)thisIter.Current;
                        }
                    }
                    else
                    {
                        // SUBCASE 1b: mine is completely to the left of theirs
                        // so we can add to difference; move mine, but not theirs
                        if ( mine.startsBeforeDisjoint( theirs ) )
                        {
                            diff.add( mine );
                            mine = null;
                            if ( thisIter.MoveNext() )
                            {
                                mine = (Interval)thisIter.Current;
                            }
                        }
                        // SUBCASE 1c: theirs is completely to the left of mine
                        else
                        {
                            // keep looking in theirs
                            theirs = null;
                            if ( otherIter.MoveNext() )
                            {
                                theirs = (Interval)otherIter.Current;
                            }
                        }
                    }
                }
                else
                {
                    // CASE 2: theirs breaks mine into two chunks
                    if ( mine.properlyContains( theirs ) )
                    {
                        // must add two intervals: stuff to left and stuff to right
                        diff.add( mine.Start, theirs.Start - 1 );
                        // don't actually add stuff to right yet as next 'theirs'
                        // might overlap with it
                        // The stuff to the right might overlap with next "theirs".
                        // so it is considered next
                        Interval right = new Interval( theirs.EndInclusive + 1, mine.EndInclusive );
                        mine = right;
                        // move theirs forward
                        theirs = null;
                        if ( otherIter.MoveNext() )
                        {
                            theirs = (Interval)otherIter.Current;
                        }
                    }

                    // CASE 3: theirs covers mine; nothing to add to diff
                    else if ( theirs.properlyContains( mine ) )
                    {
                        // nothing to add, theirs forces removal totally of mine
                        // just move mine looking for an overlapping interval
                        mine = null;
                        if ( thisIter.MoveNext() )
                        {
                            mine = (Interval)thisIter.Current;
                        }
                    }

                    // CASE 4: non proper overlap
                    else
                    {
                        // overlap, but not properly contained
                        diff.add( mine.differenceNotProperlyContained( theirs ) );
                        // update iterators
                        bool moveTheirs = true;
                        if ( mine.startsBeforeNonDisjoint( theirs ) ||
                             theirs.EndInclusive > mine.EndInclusive )
                        {
                            // uh oh, right of theirs extends past right of mine
                            // therefore could overlap with next of mine so don't
                            // move theirs iterator yet
                            moveTheirs = false;
                        }
                        // always move mine
                        mine = null;
                        if ( thisIter.MoveNext() )
                        {
                            mine = (Interval)thisIter.Current;
                        }
                        if ( moveTheirs )
                        {
                            theirs = null;
                            if ( otherIter.MoveNext() )
                            {
                                theirs = (Interval)otherIter.Current;
                            }
                        }
                    }
                }
            }
            return diff;
        }
#endif

        /** TODO: implement this! */
        public IntervalSet Union(IntervalSet a)
        {
            IntervalSet o = new IntervalSet();
            o.UnionWith(this);
            o.UnionWith(a);
            return o;
        }

        /** Return a new set with the intersection of this set with other.  Because
         *  the intervals are sorted, we can use an iterator for each list and
         *  just walk them together.  This is roughly O(min(n,m)) for interval
         *  list lengths n and m.
         */
        public IntervalSet Intersect(IntervalSet other)
        {
            if (other == null)
                return new IntervalSet();

            var myIntervals = this.intervals;
            var theirIntervals = ((IntervalSet)other).intervals;
            IntervalSet intersection = new IntervalSet();
            int mySize = myIntervals.Count;
            int theirSize = theirIntervals.Count;
            int i = 0;
            int j = 0;
            // iterate down both interval lists looking for nondisjoint intervals
            while (i < mySize && j < theirSize)
            {
                Interval mine = myIntervals[i];
                Interval theirs = theirIntervals[j];
                //JSystem.@out.println("mine="+mine+" and theirs="+theirs);
                if (StartsBeforeDisjoint(mine, theirs))
                {
                    // move this iterator looking for interval that might overlap
                    i++;
                }
                else if (StartsBeforeDisjoint(theirs, mine))
                {
                    // move other iterator looking for interval that might overlap
                    j++;
                }
                else if (mine.Contains(theirs))
                {
                    // overlap, add intersection, get next theirs
                    intersection.Intervals.Add(theirs);
                    j++;
                }
                else if (theirs.Contains(mine))
                {
                    // overlap, add intersection, get next mine
                    intersection.Intervals.Add(mine);
                    i++;
                }
                else if (mine.IntersectsWith(theirs))
                {
                    // overlap, add intersection
                    intersection.AddImpl(mine.Intersection(theirs).Value);
                    // Move the iterator of lower range [a..EndInclusive], but not
                    // the upper range as it may contain elements that will collide
                    // with the next iterator. So, if mine=[0..115] and
                    // theirs=[115..200], then intersection is 115 and move mine
                    // but not theirs as theirs may collide with the next range
                    // in thisIter.
                    // move both iterators to next ranges
                    if (StartsAfterNonDisjoint(mine, theirs))
                    {
                        j++;
                    }
                    else if (StartsAfterNonDisjoint(theirs, mine))
                    {
                        i++;
                    }
                }
            }

            return intersection;
        }

        private bool StartsBeforeDisjoint(Interval a, Interval b)
        {
            return a.Start < b.Start && !a.IntersectsWith(b);
        }

        private static bool StartsAfterNonDisjoint(Interval a, Interval b)
        {
            return a.Start >= b.Start && a.IntersectsWith(b);
        }

        /** Is el in any range of this set? */
        public virtual bool Contains(int el)
        {
            int n = intervals.Count;
            for (int i = 0; i < n; i++)
            {
                Interval I = (Interval)intervals[i];
                int a = I.Start;
                int b = I.EndInclusive;
                if (el < a)
                {
                    break; // list is sorted and el is before this interval; not here
                }
                if (el >= a && el <= b)
                {
                    return true; // found in this interval
                }
            }
            return false;
            /*
                    for (ListIterator iter = intervals.listIterator(); iter.hasNext();) {
                        Interval I = (Interval) iter.next();
                        if ( el<I.Start ) {
                            break; // list is sorted and el is before this interval; not here
                        }
                        if ( el>=I.Start && el<=I.EndInclusive ) {
                            return true; // found in this interval
                        }
                    }
                    return false;
                    */
        }

        /** return true if this set has no members */
        public virtual bool IsNil
        {
            get
            {
                return intervals == null || intervals.Count == 0;
            }
        }

        /** Are two IntervalSets equal?  Because all intervals are sorted
         *  and disjoint, equals is a simple linear walk over both lists
         *  to make sure they are the same.  Interval.equals() is used
         *  by the List.equals() method to check the ranges.
         */
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IntervalSet))
            {
                return false;
            }
            IntervalSet other = (IntervalSet)obj;
            return intervals.SequenceEqual(other.intervals);
        }

        public override int GetHashCode()
        {
            return intervals.GetHashCode();
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public virtual string ToString(IList<string> tokenNames)
        {
            if (this.intervals == null || this.intervals.Count == 0)
            {
                return "{}";
            }

            StringBuilder buf = new StringBuilder();
            if (this.intervals.Count > 1)
            {
                buf.Append("{");
            }

            foreach (Interval I in intervals)
            {
                if (I.Length == 0)
                    continue;

                // element separation
                if (buf.Length > 1)
                    buf.Append(", ");

                int a = I.Start;
                int b = I.EndInclusive;
                if (a == b)
                {
                    buf.Append(GetTokenDisplayName(tokenNames, a));
                }
                else
                {
                    buf.Append(GetTokenDisplayName(tokenNames, a) + ".." + GetTokenDisplayName(tokenNames, b));
                }
            }

            if (this.intervals.Count > 1)
            {
                buf.Append("}");
            }

            return buf.ToString();
        }

        private static string GetTokenDisplayName(IList<string> tokenNames, int symbol)
        {
            if (tokenNames == null || symbol < 0 || symbol >= tokenNames.Count)
                return symbol.ToString();

            return tokenNames[symbol];
        }

        public List<int> ToList()
        {
            int count = Count;
            List<int> list = new List<int>(count);

            foreach (Interval interval in intervals)
            {
                for (int i = interval.Start; i <= interval.EndInclusive; i++)
                    list.Add(i);
            }

            return list;
        }

        /** Get the ith element of ordered set.  Used only by RandomPhrase so
         *  don't bother to implement if you're not doing that for a new
         *  ANTLR code gen target.
         */
        public virtual int Get(int i)
        {
            int index = 0;
            foreach (Interval I in intervals)
            {
                int a = I.Start;
                int b = I.EndInclusive;
                for (int v = a; v <= b; v++)
                {
                    if (index == i)
                        return v;

                    index++;
                }
            }

            return -1;
        }

        public int[] ToArray()
        {
            int[] values = new int[Count];
            int n = intervals.Count;
            int j = 0;
            for (int i = 0; i < n; i++)
            {
                Interval I = (Interval)intervals[i];
                int a = I.Start;
                int b = I.EndInclusive;
                for (int v = a; v <= b; v++)
                {
                    values[j] = v;
                    j++;
                }
            }
            return values;
        }

#if false
        public Antlr.Runtime.BitSet ToRuntimeBitSet()
        {
            Antlr.Runtime.BitSet s =
                new Antlr.Runtime.BitSet(GetMaxElement() + 1);
            int n = intervals.Count;
            for (int i = 0; i < n; i++)
            {
                Interval I = (Interval)intervals[i];
                int a = I.Start;
                int b = I.EndInclusive;
                for (int v = a; v <= b; v++)
                {
                    s.Add(v);
                }
            }
            return s;
        }
#endif

        public virtual void Remove(int el)
        {
            throw new NotImplementedException();
        }

        /*
        protected void finalize()
        {
            super.finalize();
            JSystem.@out.println("size "+intervals.size()+" "+size());
        }
        */

        #region ISet<int> Members

        bool ISet<int>.Add(int item)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<int> other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection<int> Members

        void ICollection<int>.Add(int item)
        {
            Add(item);
        }

        void ICollection<int>.Clear()
        {
            intervals.Clear();
        }

        bool ICollection<int>.Contains(int item)
        {
            return Contains(item);
        }

        void ICollection<int>.CopyTo(int[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<int>.IsReadOnly
        {
            get
            {
                return intervals.IsReadOnly;
            }
        }

        bool ICollection<int>.Remove(int item)
        {
            Remove(item);
            return true;
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            return intervals.SelectMany(interval => Enumerable.Range(interval.Start, interval.EndInclusive - interval.Start + 1)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
