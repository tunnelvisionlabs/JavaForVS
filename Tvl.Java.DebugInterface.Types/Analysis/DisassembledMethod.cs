namespace Tvl.Java.DebugInterface.Types.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Tvl.Collections;

    public class DisassembledMethod
    {
        private readonly ImmutableList<JavaInstruction> _instructions;
        private readonly ImmutableList<SwitchData> _switchData;

        public DisassembledMethod(IList<JavaInstruction> instructions, IList<SwitchData> switchData)
        {
            Contract.Requires<ArgumentNullException>(instructions != null, "instructions");
            Contract.Requires<ArgumentNullException>(switchData != null, "switchData");

            _instructions = new ImmutableList<JavaInstruction>(instructions);
            _switchData = new ImmutableList<SwitchData>(switchData);
        }

        public ImmutableList<JavaInstruction> Instructions
        {
            get
            {
                return _instructions;
            }
        }

        public ImmutableList<SwitchData> SwitchData
        {
            get
            {
                return _switchData;
            }
        }
    }
}
