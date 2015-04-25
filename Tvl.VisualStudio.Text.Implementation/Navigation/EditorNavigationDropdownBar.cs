namespace Tvl.VisualStudio.Text.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Editor;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Projection;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Tvl.VisualStudio.Shell;

    using Bitmap = System.Drawing.Bitmap;
    using BitmapSource = System.Windows.Media.Imaging.BitmapSource;
    using Dispatcher = System.Windows.Threading.Dispatcher;
    using FormatConvertedBitmap = System.Windows.Media.Imaging.FormatConvertedBitmap;
    using IConnectionPoint = Microsoft.VisualStudio.OLE.Interop.IConnectionPoint;
    using IConnectionPointContainer = Microsoft.VisualStudio.OLE.Interop.IConnectionPointContainer;
    using ImageList = System.Windows.Forms.ImageList;
    using ImageSource = System.Windows.Media.ImageSource;
    using Keyboard = System.Windows.Input.Keyboard;
    using PixelFormat = System.Drawing.Imaging.PixelFormat;
    using PixelFormats = System.Windows.Media.PixelFormats;
    using SpanTrackingMode = Microsoft.VisualStudio.Text.SpanTrackingMode;
    using Thread = System.Threading.Thread;
    using WeakEvents = Tvl.Events.WeakEvents;

    [ComVisible(true)]
    public class EditorNavigationDropdownBar : IEditorNavigationDropdownBarClient, IVsDropdownBarClient, IVsDropdownBarClientEx, IVsCodeWindowEvents, IVsTextViewEvents
    {
        private readonly IVsCodeWindow _codeWindow;
        private readonly IVsEditorAdaptersFactoryService _editorAdaptersFactory;
        private readonly IEnumerable<IEditorNavigationSource> _sources;
        private readonly IBufferGraphFactoryService _bufferGraphFactoryService;
        private readonly IJavaEditorNavigationTypeRegistryService _editorNavigationTypeRegistryService;
        private readonly Dispatcher _dispatcher;
        private readonly ImageList _imageList;
        private readonly Dictionary<WeakReference<ImageSource>, int> _glyphIndexes =
            new Dictionary<WeakReference<ImageSource>, int>();

        private uint _codeWindowEventsCookie;
        private readonly Dictionary<IVsTextView, uint> _textViewEventsCookies = new Dictionary<IVsTextView, uint>();

        private IWpfTextView _currentTextView;
        private IVsDropdownBar _dropdownBar;
        private readonly Tuple<IEditorNavigationType, List<IEditorNavigationTarget>>[] _navigationControls;
        private readonly IEditorNavigationTarget[] _selectedItem;

        private readonly Dictionary<IEditorNavigationTarget, IEditorNavigationTarget> _owners =
            new Dictionary<IEditorNavigationTarget, IEditorNavigationTarget>();

        public EditorNavigationDropdownBar(IVsCodeWindow codeWindow, IVsEditorAdaptersFactoryService editorAdaptersFactory, IEnumerable<IEditorNavigationSource> sources, IBufferGraphFactoryService bufferGraphFactoryService, IJavaEditorNavigationTypeRegistryService editorNavigationTypeRegistryService)
        {
            Contract.Requires<ArgumentNullException>(codeWindow != null, "codeWindow");
            Contract.Requires<ArgumentNullException>(editorAdaptersFactory != null, "editorAdaptersFactory");
            Contract.Requires<ArgumentNullException>(sources != null, "sources");
            Contract.Requires<ArgumentNullException>(bufferGraphFactoryService != null, "bufferGraphFactoryService");
            Contract.Requires<ArgumentNullException>(editorNavigationTypeRegistryService != null, "editorNavigationTypeRegistryService");

            this._codeWindow = codeWindow;
            this._editorAdaptersFactory = editorAdaptersFactory;
            this._sources = sources;
            this._bufferGraphFactoryService = bufferGraphFactoryService;
            this._editorNavigationTypeRegistryService = editorNavigationTypeRegistryService;
            this._currentTextView = editorAdaptersFactory.GetWpfTextView(codeWindow.GetLastActiveView());
            this._dispatcher = this._currentTextView.VisualElement.Dispatcher;
            this._imageList = new ImageList()
                {
                    ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
                };

            _navigationControls =
                this._sources
                .SelectMany(source => source.GetNavigationTypes())
                .Distinct()
                //.OrderBy(...)
                .Select(type => Tuple.Create(type, new List<IEditorNavigationTarget>()))
                .ToArray();

            _selectedItem = new IEditorNavigationTarget[_navigationControls.Length];

            if (this._navigationControls.Length == 0)
            {
                return;
            }

            IConnectionPointContainer connectionPointContainer = codeWindow as IConnectionPointContainer;
            if (connectionPointContainer != null)
            {
                Guid textViewEventsGuid = typeof(IVsCodeWindowEvents).GUID;
                IConnectionPoint connectionPoint;
                connectionPointContainer.FindConnectionPoint(ref textViewEventsGuid, out connectionPoint);
                if (connectionPoint != null)
                    connectionPoint.Advise(this, out _codeWindowEventsCookie);
            }

            IVsTextView primaryView = codeWindow.GetPrimaryView();
            if (primaryView != null)
                ((IVsCodeWindowEvents)this).OnNewView(primaryView);

            IVsTextView secondaryView = codeWindow.GetSecondaryView();
            if (secondaryView != null)
                ((IVsCodeWindowEvents)this).OnNewView(secondaryView);

            foreach (var source in this._sources)
            {
                source.NavigationTargetsChanged += WeakEvents.AsWeak(OnNavigationTargetsChanged, eh => source.NavigationTargetsChanged -= eh);
                UpdateNavigationTargets(source);
            }

            _currentTextView.Caret.PositionChanged += OnCaretPositionChanged;
        }

        public int DropdownCount
        {
            get
            {
                return _navigationControls.Length;
            }
        }

        public bool Disposed
        {
            get;
            private set;
        }

        public bool IsDisposing
        {
            get;
            private set;
        }

        public bool Updating
        {
            get;
            private set;
        }

        public bool Enabled
        {
            get
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (IsDisposing)
                throw new InvalidOperationException();

            try
            {
                IsDisposing = true;
                Dispose(true);
            }
            finally
            {
                IsDisposing = false;
            }

            GC.SuppressFinalize(this);
        }

        private Dispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;
            }
        }

        #region IVsDropdownBarClient Members

        public int GetComboAttributes(int iCombo, out uint pcEntries, out uint puEntryType, out IntPtr phImageList)
        {
            pcEntries = 0;
            puEntryType = 0;
            phImageList = IntPtr.Zero;
            if (!ValidateCombo(iCombo))
                return VSConstants.E_INVALIDARG;

            pcEntries = (uint)_navigationControls[iCombo].Item2.Count;
            puEntryType = (uint)(DROPDOWNENTRYTYPE.ENTRY_IMAGE | DROPDOWNENTRYTYPE.ENTRY_TEXT | DROPDOWNENTRYTYPE.ENTRY_ATTR);
            phImageList = _imageList.Handle;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the tooltip for an entire drop-down bar combination.
        /// </summary>
        /// <param name="iCombo">[in] The drop-down bar/Window combination.</param>
        /// <param name="pbstrText">[out] String containing the tooltip text.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public int GetComboTipText(int iCombo, out string pbstrText)
        {
            pbstrText = null;
            if (!ValidateCombo(iCombo))
                return VSConstants.E_INVALIDARG;

            pbstrText = _navigationControls[iCombo].Item1.Definition.DisplayName;
            return pbstrText != null ? VSConstants.S_OK : VSConstants.E_FAIL;
        }

        /// <summary>
        /// Returns text appearance attributes for a drop-down combination entry.
        /// </summary>
        /// <param name="iCombo">[in] The drop-down bar/Window combo.</param>
        /// <param name="iIndex">[in] Index of item of interest.</param>
        /// <param name="pAttr">[out] Font attribute. Values for pAttr are taken from the DROPDOWNFONTATTR enum.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        public int GetEntryAttributes(int iCombo, int iIndex, out uint pAttr)
        {
            pAttr = 0;
            if (!ValidateIndex(iCombo, iIndex))
                return VSConstants.E_INVALIDARG;

            IEditorNavigationTarget target = _navigationControls[iCombo].Item2[iIndex];

            pAttr = (uint)DROPDOWNFONTATTR.FONTATTR_PLAIN;
            if (target.IsBold)
                pAttr |= (uint)DROPDOWNFONTATTR.FONTATTR_BOLD;
            if (target.IsGray)
                pAttr |= (uint)DROPDOWNFONTATTR.FONTATTR_GRAY;
            if (target.IsItalic)
                pAttr |= (uint)DROPDOWNFONTATTR.FONTATTR_ITALIC;
            if (target.IsUnderlined)
                pAttr |= (uint)DROPDOWNFONTATTR.FONTATTR_UNDERLINE;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Returns the glyph associated with a combo entry.
        /// </summary>
        /// <param name="iCombo">[in] The drop-down bar/Window combo.</param>
        /// <param name="iIndex">[in] Index of item of interest.</param>
        /// <param name="piImageIndex">[out] Index of glyph in the image list.</param>
        /// <returns>If the method succeeds, it returns S_OK. If it fails, it returns an error code.</returns>
        /// <remarks>
        /// GetImage will only be called if you specified ENTRY_IMAGE for the entry attributes of the given combo. There are two expected return codes from GetImage:
        ///     S_OK: Draw the glyph indicated in *piImageIndex. Use this for entries that should have a glyph.
        ///     S_FALSE: Set aside space for a glyph, but don't draw anything. Use this for entries that don't have a glyph but might have sibling entries in the same combo with glyphs.
        ///     Other: Some other failure occurred.
        ///
        /// Glyphs in your image lists are assumed to be of the same height.
        /// </remarks>
        public int GetEntryImage(int iCombo, int iIndex, out int piImageIndex)
        {
            piImageIndex = 0;
            if (!ValidateIndex(iCombo, iIndex))
                return VSConstants.E_INVALIDARG;

            var targetGlyph = _navigationControls[iCombo].Item2[iIndex].Glyph;
            if (targetGlyph == null)
                return VSConstants.S_FALSE;

            int index;
            if (!_glyphIndexes.TryGetValue(new WeakReference<ImageSource>(targetGlyph), out index))
            {
                index = -1;

                // add the image to the image list
                BitmapSource bitmapSource = targetGlyph as BitmapSource;
                if (bitmapSource != null)
                {
                    if (bitmapSource.Format != PixelFormats.Pbgra32 && bitmapSource.Format != PixelFormats.Bgra32)
                    {
                        var formattedBitmapSource = new FormatConvertedBitmap();
                        formattedBitmapSource.BeginInit();
                        formattedBitmapSource.Source = bitmapSource;
                        formattedBitmapSource.DestinationFormat = PixelFormats.Pbgra32;
                        formattedBitmapSource.EndInit();

                        bitmapSource = formattedBitmapSource;
                    }

                    int bytesPerPixel = bitmapSource.Format.BitsPerPixel / 8;
                    byte[] data = new byte[bitmapSource.PixelWidth * bitmapSource.PixelHeight * bytesPerPixel];
                    int stride = bitmapSource.PixelWidth * bytesPerPixel;
                    bitmapSource.CopyPixels(data, stride, 0);
                    IntPtr nativeData = Marshal.AllocHGlobal(data.Length);
                    try
                    {
                        Marshal.Copy(data, 0, nativeData, data.Length);
                        PixelFormat pixelFormat = (bitmapSource.Format == PixelFormats.Bgra32) ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppPArgb;
                        Bitmap bitmap = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, stride, pixelFormat, nativeData);
                        _imageList.Images.Add(bitmap);
                        index = _imageList.Images.Count - 1;
                        _glyphIndexes.Add(new WeakReference<ImageSource>(targetGlyph), index);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(nativeData);
                    }
                }
            }

            if (index >= 0)
                piImageIndex = index;

            return index >= 0 ? VSConstants.S_OK : VSConstants.S_FALSE;
        }

        public int GetEntryText(int iCombo, int iIndex, out string ppszText)
        {
            ppszText = null;
            if (!ValidateIndex(iCombo, iIndex))
                return VSConstants.E_INVALIDARG;

            ppszText = _navigationControls[iCombo].Item2[iIndex].Name;
            return ppszText != null ? VSConstants.S_OK : VSConstants.E_FAIL;
        }

        public int OnComboGetFocus(int iCombo)
        {
            if (!ValidateCombo(iCombo))
                return VSConstants.E_INVALIDARG;

            return VSConstants.S_OK;
        }

        public int OnItemChosen(int iCombo, int iIndex)
        {
            if (!ValidateIndex(iCombo, iIndex))
                return VSConstants.E_INVALIDARG;

            if (Updating)
                return VSConstants.E_FAIL;

            try
            {
                IEditorNavigationTarget target = _navigationControls[iCombo].Item2[iIndex];
                if (target != null)
                {
                    // show the span, then adjust if necessary to make sure the seek portion is visible
                    var span = MapTo(target.Span, _currentTextView.TextSnapshot, SpanTrackingMode.EdgeInclusive);
                    _currentTextView.ViewScroller.EnsureSpanVisible(span, EnsureSpanVisibleOptions.AlwaysCenter | EnsureSpanVisibleOptions.ShowStart);

                    var seek = target.Seek.Snapshot == null ? target.Span : target.Seek;
                    seek = MapTo(seek, _currentTextView.TextSnapshot, SpanTrackingMode.EdgeInclusive);
                    _currentTextView.Caret.MoveTo(seek.Start);
                    _currentTextView.Selection.Select(seek, false);
                    _currentTextView.ViewScroller.EnsureSpanVisible(seek, EnsureSpanVisibleOptions.MinimumScroll | EnsureSpanVisibleOptions.ShowStart);
                    Keyboard.Focus(_currentTextView.VisualElement);
                }
            }
            catch (Exception ex)
            {
                if (ex.IsCritical())
                    throw;

                return Marshal.GetHRForException(ex);
            }

            return VSConstants.S_OK;
        }

        public int OnItemSelected(int iCombo, int iIndex)
        {
            if (!ValidateIndex(iCombo, iIndex))
                return VSConstants.E_INVALIDARG;

            return VSConstants.S_OK;
        }

        public int SetDropdownBar(IVsDropdownBar pDropdownBar)
        {
            _dropdownBar = pDropdownBar;
            return VSConstants.S_OK;
        }

        #endregion

        #region IVsDropdownBarClientEx Members

        public int GetEntryIndent(int iCombo, int iIndex, out uint pIndent)
        {
            pIndent = 0;
            if (!ValidateIndex(iCombo, iIndex))
                return VSConstants.E_INVALIDARG;

            pIndent = 0;
            return VSConstants.S_OK;
        }

        #endregion

        #region IVsCodeWindowEvents Members

        int IVsCodeWindowEvents.OnCloseView(IVsTextView pView)
        {
            if (pView == null)
                throw new ArgumentNullException("pView");

            uint cookie;
            if (_textViewEventsCookies.TryGetValue(pView, out cookie))
            {
                _textViewEventsCookies.Remove(pView);

                IConnectionPointContainer connectionPointContainer = pView as IConnectionPointContainer;
                if (connectionPointContainer != null)
                {
                    Guid textViewEventsGuid = typeof(IVsTextViewEvents).GUID;
                    IConnectionPoint connectionPoint;
                    connectionPointContainer.FindConnectionPoint(ref textViewEventsGuid, out connectionPoint);
                    if (connectionPoint != null)
                        connectionPoint.Unadvise(cookie);
                }
            }

            return VSConstants.S_OK;
        }

        int IVsCodeWindowEvents.OnNewView(IVsTextView pView)
        {
            if (pView == null)
                throw new ArgumentNullException("pView");

            IConnectionPointContainer connectionPointContainer = pView as IConnectionPointContainer;
            if (connectionPointContainer != null)
            {
                Guid textViewEventsGuid = typeof(IVsTextViewEvents).GUID;
                IConnectionPoint connectionPoint;
                connectionPointContainer.FindConnectionPoint(ref textViewEventsGuid, out connectionPoint);
                if (connectionPoint != null)
                {
                    uint cookie;
                    connectionPoint.Advise(this, out cookie);
                    if (cookie != 0)
                        _textViewEventsCookies.Add(pView, cookie);
                }
            }

            return VSConstants.S_OK;
        }

        #endregion

        #region IVsTextViewEvents Members

        void IVsTextViewEvents.OnChangeCaretLine(IVsTextView pView, int iNewLine, int iOldLine)
        {
        }

        void IVsTextViewEvents.OnChangeScrollInfo(IVsTextView pView, int iBar, int iMinUnit, int iMaxUnits, int iVisibleUnits, int iFirstVisibleUnit)
        {
        }

        void IVsTextViewEvents.OnKillFocus(IVsTextView pView)
        {
        }

        void IVsTextViewEvents.OnSetBuffer(IVsTextView pView, IVsTextLines pBuffer)
        {
        }

        void IVsTextViewEvents.OnSetFocus(IVsTextView pView)
        {
            IWpfTextView textView = pView != null ? _editorAdaptersFactory.GetWpfTextView(pView) : null;
            if (textView == _currentTextView)
            {
                IVsTextView activeView = _codeWindow.GetLastActiveView();
                if (activeView != pView)
                    return;

                return;
            }

            if (_currentTextView != null)
                _currentTextView.Caret.PositionChanged -= OnCaretPositionChanged;

            _currentTextView = textView;

            if (_currentTextView != null)
            {
                _currentTextView.Caret.PositionChanged += OnCaretPositionChanged;
                foreach (var source in this._sources)
                    UpdateNavigationTargets(source);
            }
        }

        #endregion

        private bool ValidateCombo(int combo)
        {
            return combo >= 0 && combo < _navigationControls.Length;
        }

        private bool ValidateIndex(int combo, int index)
        {
            return ValidateCombo(combo) && index >= 0 && index < _navigationControls[combo].Item2.Count;
        }

        private bool ComboBoxFilter(IEditorNavigationType navigationType, List<IEditorNavigationTarget> comboBox, IEditorNavigationTarget target)
        {
            if (navigationType == null || comboBox == null || target == null)
                return true;

            if (!navigationType.Definition.EnclosingTypes.Any())
                return true;

            IEditorNavigationTarget owner;
            if (!_owners.TryGetValue(target, out owner))
                return true;

            return _selectedItem.Contains(owner);
        }

        private void UpdateNavigationTargets(IEditorNavigationSource source)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");

            lock (this)
            {
                if (Updating)
                    return;
            }

            try
            {
                Updating = true;
                var targets = source.GetNavigationTargets().ToArray();
                Action action = () => UpdateNavigationTargets(targets);
                if (Dispatcher.Thread == Thread.CurrentThread)
                    action();
                else
                    Dispatcher.Invoke(action);
            }
            finally
            {
                Updating = false;
            }
        }

        private void UpdateNavigationTargets(IEnumerable<IEditorNavigationTarget> targets)
        {
            Contract.Requires<ArgumentNullException>(targets != null, "targets");

            foreach (var control in _navigationControls)
            {
                List<IEditorNavigationTarget> combo = control.Item2;
                lock (combo)
                {
                    control.Item2.Clear();
                }
            }

            foreach (var group in targets.GroupBy(target => target.EditorNavigationType))
            {
                var navigationControl = this._navigationControls.FirstOrDefault(control => control.Item1 == group.Key);
                if (navigationControl == null)
                    continue;

                var combo = navigationControl.Item2;
                lock (combo)
                {
                    combo.AddRange(group.OrderBy(i => i.Name, StringComparer.CurrentCultureIgnoreCase));
                }
            }

            _owners.Clear();
            foreach (var childControl in _navigationControls)
            {
                IEditorNavigationType navigationType = childControl.Item1;
                string enclosingType = navigationType.Definition.EnclosingTypes.FirstOrDefault();
                if (string.IsNullOrEmpty(enclosingType))
                    continue;

                foreach (var childItem in childControl.Item2)
                {
                    foreach (var control in _navigationControls.Where(i => i.Item1.IsOfType(enclosingType)))
                    {
                        IEditorNavigationTarget best =
                            control.Item2
                            .Where(i => i.Span.OverlapsWith(childItem.Span))
                            .OrderBy(i => i.Span.Length)
                            .FirstOrDefault();

                        if (best != null)
                        {
                            _owners[childItem] = best;
                            goto nextChild;
                        }
                    }

                nextChild:
                    continue;
                }
            }

            UpdateSelectedNavigationTargets();
        }

        private void OnNavigationTargetsChanged(object sender, EventArgs e)
        {
            IEditorNavigationSource source = (IEditorNavigationSource)sender;
            UpdateNavigationTargets(source);
        }

        private void UpdateSelectedNavigationTargets()
        {
            if (Thread.CurrentThread != Dispatcher.Thread)
            {
                Dispatcher.Invoke((Action)UpdateSelectedNavigationTargets);
                return;
            }

            try
            {
                UpdateSelectedNavigationTargetsImpl();
            }
            catch (Exception ex)
            {
                if (ex.IsCritical())
                    throw;
            }
        }

        private void UpdateSelectedNavigationTargetsImpl()
        {
            var currentPosition = _currentTextView.Caret.Position.BufferPosition;

            for (int i = 0; i < _navigationControls.Length; i++)
            {
                var control = _navigationControls[i];

                control.Item2.RemoveAll(target => !ComboBoxFilter(control.Item1, control.Item2, target));

                if (control.Item2.Count == 0)
                    continue;

                var oldSelectedItem = _selectedItem[i];

                var positionOrderedItems = control.Item2.OrderBy(j => j.Span.Start).ToArray();
                var newSelectedItem =
                    positionOrderedItems
                    .LastOrDefault(j => MapTo(j.Span, currentPosition.Snapshot, SpanTrackingMode.EdgeInclusive).Contains(currentPosition));

                if (newSelectedItem == null)
                {
                    // select the first item starting after the current position
                    newSelectedItem =
                        positionOrderedItems.FirstOrDefault(j => MapTo(j.Span, currentPosition.Snapshot, SpanTrackingMode.EdgeInclusive).Start > currentPosition);
                }

                if (newSelectedItem == null)
                {
                    // select the last item ending before the current position
                    newSelectedItem =
                        positionOrderedItems.LastOrDefault(j => MapTo(j.Span, currentPosition.Snapshot, SpanTrackingMode.EdgeInclusive).End < currentPosition);
                }

                if (newSelectedItem == null)
                {
                    // select the first item
                    newSelectedItem = control.Item2[0];
                }

                bool wasUpdating = Updating;
                try
                {
                    Updating = true;
                    int newIndex = control.Item2.IndexOf(newSelectedItem);
                    _selectedItem[i] = newSelectedItem;
                    if (_dropdownBar != null)
                        _dropdownBar.RefreshCombo(i, newIndex);
                }
                finally
                {
                    Updating = wasUpdating;
                }
            }
        }

        private SnapshotSpan MapTo(SnapshotSpan span, ITextSnapshot snapshot, SpanTrackingMode spanTrackingMode)
        {
            if (span.Snapshot.TextBuffer == snapshot.TextBuffer)
                return span.TranslateTo(snapshot, spanTrackingMode);

            IBufferGraph graph = _bufferGraphFactoryService.CreateBufferGraph(snapshot.TextBuffer);
            IMappingSpan mappingSpan = graph.CreateMappingSpan(span, spanTrackingMode);
            NormalizedSnapshotSpanCollection mapped = mappingSpan.GetSpans(snapshot);
            if (mapped.Count == 1)
                return mapped[0];

            return new SnapshotSpan(mapped[0].Start, mapped[mapped.Count - 1].End);
        }

        private void OnCaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            UpdateSelectedNavigationTargets();
        }
    }
}
