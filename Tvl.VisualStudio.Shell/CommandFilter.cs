namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;

    using IOleCommandTarget = Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget;
    using OLECMD = Microsoft.VisualStudio.OLE.Interop.OLECMD;
    using OLECMDEXECOPT = Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT;
    using OleConstants = Microsoft.VisualStudio.OLE.Interop.Constants;
    using VsMenus = Microsoft.VisualStudio.Shell.VsMenus;

    [ComVisible(true)]
    public abstract class CommandFilter : IOleCommandTarget, IDisposable
    {
        private bool _connected;
        private IOleCommandTarget _next;

        protected CommandFilter()
        {
        }

        public bool Enabled
        {
            get
            {
                ThrowIfDisposed();
                return _connected;
            }
            set
            {
                ThrowIfDisposed();
                if (_connected == value)
                    return;

                if (value)
                {
                    _next = Connect();
                    _connected = value;
                }
                else
                {
                    try
                    {
                        Disconnect();
                    }
                    catch (Exception e)
                    {
                        if (!IsDisposing || ErrorHandler.IsCriticalException(e))
                            throw;
                    }
                    finally
                    {
                        _next = null;
                        _connected = value;
                    }
                }
            }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public bool IsDisposing
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (IsDisposing)
                throw new InvalidOperationException();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            IsDisposing = true;
            try
            {
                if (!IsDisposed)
                {
                    Enabled = false;
                }
            }
            finally
            {
                IsDisposed = true;
                IsDisposing = false;
            }
        }

        protected abstract IOleCommandTarget Connect();

        protected abstract void Disconnect();

        private void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        private int ExecCommand(ref Guid guidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            int rc = VSConstants.S_OK;

            if (!HandlePreExec(ref guidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut) && _next != null)
            {
                // Pass it along the chain.
                rc = this.InnerExec(ref guidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
                if (!ErrorHandler.Succeeded(rc))
                    return rc;

                HandlePostExec(ref guidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            return rc;
        }

        protected virtual bool HandlePreExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            return false;
        }

        private int InnerExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (_next != null)
                return _next.Exec(ref commandGroup, commandId, executionOptions, pvaIn, pvaOut);

            return (int)OleConstants.OLECMDERR_E_NOTSUPPORTED;
        }

        protected virtual void HandlePostExec(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
        }

        protected virtual int QueryParameterList(ref Guid commandGroup, uint commandId, uint executionOptions, IntPtr pvaIn, IntPtr pvaOut)
        {
            return (int)OleConstants.OLECMDERR_E_NOTSUPPORTED;
        }

        protected virtual CommandStatus QueryCommandStatus(ref Guid group, uint id)
        {
            return CommandStatus.None;
        }

        int IOleCommandTarget.Exec(ref Guid guidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ushort lo = (ushort)(nCmdexecopt & (uint)0xffff);
            ushort hi = (ushort)(nCmdexecopt >> 16);

            switch (lo)
            {
            case (ushort)OLECMDEXECOPT.OLECMDEXECOPT_SHOWHELP:
                if ((nCmdexecopt >> 16) == VsMenus.VSCmdOptQueryParameterList)
                {
                    return QueryParameterList(ref guidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
                }
                break;

            default:
                return ExecCommand(ref guidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }

            return (int)OleConstants.OLECMDERR_E_NOTSUPPORTED;
        }

        int IOleCommandTarget.QueryStatus(ref Guid guidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            Guid cmdGroup = guidCmdGroup;
            for (uint i = 0; i < cCmds; i++)
            {
                CommandStatus status = QueryCommandStatus(ref cmdGroup, prgCmds[i].cmdID);
                if (status == CommandStatus.None && _next != null)
                {
                    if (_next != null)
                        return _next.QueryStatus(ref cmdGroup, cCmds, prgCmds, pCmdText);
                    else
                        return (int)OleConstants.OLECMDERR_E_NOTSUPPORTED;
                }

                prgCmds[i].cmdf = (uint)status;
            }

            return VSConstants.S_OK;
        }
    }
}
