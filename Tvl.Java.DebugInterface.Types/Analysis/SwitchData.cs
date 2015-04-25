namespace Tvl.Java.DebugInterface.Types.Analysis
{
    public abstract class SwitchData
    {
        private readonly int _defaultValue;

        public SwitchData(int defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public int DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }
    }
}
