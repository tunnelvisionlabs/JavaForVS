namespace Tvl.VisualStudio.Text
{
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.Text;

    [ContractClass(typeof(Contracts.ICommenterContracts))]
    public interface ICommenter
    {
        /// <summary>
        /// Comments out spans of code.
        /// </summary>
        /// <param name="spans">The collection of spans to comment out.</param>
        /// <returns>A collection of spans encompassing the resulting comments.</returns>
        NormalizedSnapshotSpanCollection CommentSpans(NormalizedSnapshotSpanCollection spans);

        /// <summary>
        /// Uncomments spans of code.
        /// </summary>
        /// <param name="spans">The collection of spans to uncomment.</param>
        /// <returns>A collection of spans encompassing the resulting uncommented code.</returns>
        NormalizedSnapshotSpanCollection UncommentSpans(NormalizedSnapshotSpanCollection spans);
    }
}
