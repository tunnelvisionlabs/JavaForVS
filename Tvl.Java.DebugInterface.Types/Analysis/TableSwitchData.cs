namespace Tvl.Java.DebugInterface.Types.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Tvl.Collections;

    public sealed class TableSwitchData : SwitchData
    {
        private readonly int _lowValue;
        private readonly int _highValue;
        private readonly ImmutableList<int> _offsets;

        public TableSwitchData(int defaultValue, int lowValue, int highValue, IEnumerable<int> offsets)
            : base(defaultValue)
        {
            Contract.Requires<ArgumentNullException>(offsets != null, "offsets");
            Contract.Requires<ArgumentException>(highValue >= lowValue);

            _lowValue = lowValue;
            _highValue = highValue;
            _offsets = new ImmutableList<int>(offsets);
        }

        public int LowValue
        {
            get
            {
                return _lowValue;
            }
        }

        public int HighValue
        {
            get
            {
                return _highValue;
            }
        }

        public ImmutableList<int> Offsets
        {
            get
            {
                return _offsets;
            }
        }
    }
}
