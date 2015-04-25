namespace Tvl.Java.DebugInterface.Types.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Tvl.Collections;

    public sealed class LookupSwitchData : SwitchData
    {
        private readonly ImmutableList<KeyValuePair<int, int>> _pairs;

        public LookupSwitchData(int defaultValue, IEnumerable<KeyValuePair<int, int>> pairs)
            : base(defaultValue)
        {
            Contract.Requires<ArgumentNullException>(pairs != null, "pairs");
            _pairs = new ImmutableList<KeyValuePair<int, int>>(pairs);
        }

        public ImmutableList<KeyValuePair<int, int>> Pairs
        {
            get
            {
                return _pairs;
            }
        }
    }
}
