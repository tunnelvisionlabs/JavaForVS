namespace Tvl.VisualStudio.Text.Tagging
{
    using Microsoft.VisualStudio.Text.Tagging;

    public static class PredefinedTextMarkerTags
    {
        public static readonly TextMarkerTag AddLine = new TextMarkerTag(PredefinedMarkerFormats.AddLine);
        public static readonly TextMarkerTag AddWord = new TextMarkerTag(PredefinedMarkerFormats.AddWord);
        public static readonly TextMarkerTag Blue = new TextMarkerTag(PredefinedMarkerFormats.Blue);
        public static readonly TextMarkerTag Bookmark = new TextMarkerTag(PredefinedMarkerFormats.Bookmark);
        public static readonly TextMarkerTag BraceHighlight = new TextMarkerTag(PredefinedMarkerFormats.BraceHighlight);
        public static readonly TextMarkerTag Breakpoint = new TextMarkerTag(PredefinedMarkerFormats.Breakpoint);
        public static readonly TextMarkerTag CurrentStatement = new TextMarkerTag(PredefinedMarkerFormats.CurrentStatement);
        public static readonly TextMarkerTag RemoveLine = new TextMarkerTag(PredefinedMarkerFormats.RemoveLine);
        public static readonly TextMarkerTag RemoveWord = new TextMarkerTag(PredefinedMarkerFormats.RemoveWord);
        public static readonly TextMarkerTag ReturnStatement = new TextMarkerTag(PredefinedMarkerFormats.ReturnStatement);
        public static readonly TextMarkerTag StepBackCurrentStatement = new TextMarkerTag(PredefinedMarkerFormats.StepBackCurrentStatement);
        public static readonly TextMarkerTag Vivid = new TextMarkerTag(PredefinedMarkerFormats.Vivid);
    }
}
