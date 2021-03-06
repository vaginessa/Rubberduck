﻿using System;
using Rubberduck.VBEditor.SafeComWrappers.Abstract;
using VB = Microsoft.VB6.Interop.VBIDE;

namespace Rubberduck.VBEditor.SafeComWrappers.VB6
{
    public class CodePane : SafeComWrapper<VB.CodePane>, ICodePane
    {
        public CodePane(VB.CodePane codePane)
            : base(codePane)
        {
        }

        public ICodePanes Collection
        {
            get { return new CodePanes(IsWrappingNullReference ? null : Target.Collection); }
        }

        public IVBE VBE
        {
            get { return new VBE(IsWrappingNullReference ? null : Target.VBE); }
        }

        public IWindow Window
        {
            get { return new Window(IsWrappingNullReference ? null : Target.Window); }
        }

        public int TopLine
        {
            get { return IsWrappingNullReference ? 0 : Target.TopLine; }
            set { Target.TopLine = value; }
        }

        public int CountOfVisibleLines
        {
            get { return IsWrappingNullReference ? 0 : Target.CountOfVisibleLines; }
        }
        
        public ICodeModule CodeModule
        {
            get { return new CodeModule(IsWrappingNullReference ? null : Target.CodeModule); }
        }

        public CodePaneView CodePaneView
        {
            get { return IsWrappingNullReference ? 0 : (CodePaneView)Target.CodePaneView; }
        }

        private Selection GetSelection()
        {
            int startLine;
            int startColumn;
            int endLine;
            int endColumn;
            Target.GetSelection(out startLine, out startColumn, out endLine, out endColumn);

            if (endLine > startLine && endColumn == 1)
            {
                endLine -= 1;
                endColumn = CodeModule.GetLines(endLine, 1).Length;
            }

            return new Selection(startLine, startColumn, endLine, endColumn);
        }

        public Selection Selection
        {
            get { return GetSelection(); }
            set { SetSelection(value.StartLine, value.StartColumn, value.EndLine, value.EndColumn); }
        }

        public QualifiedSelection? GetQualifiedSelection()
        {
            if (IsWrappingNullReference)
            {
                return null;
            }

            var selection = GetSelection();
            if (selection.IsEmpty())
            {
                return null;
            }

            var component = CodeModule.Parent;
            var moduleName = new QualifiedModuleName(component);
            return new QualifiedSelection(moduleName, selection);
        }

        private void SetSelection(int startLine, int startColumn, int endLine, int endColumn)
        {
            Target.SetSelection(startLine, startColumn, endLine, endColumn);
            ForceFocus();
        }

        private void ForceFocus()
        {
            Show();

            var window = VBE.MainWindow;
            var mainWindowHandle = window.Handle();
            var caption = window.Caption;
            var childWindowFinder = new NativeMethods.ChildWindowFinder(caption);

            NativeMethods.EnumChildWindows(mainWindowHandle, childWindowFinder.EnumWindowsProcToChildWindowByCaption);
            var handle = childWindowFinder.ResultHandle;

            if (handle != IntPtr.Zero)
            {
                NativeMethods.ActivateWindow(handle, mainWindowHandle);
            }
        }

        public void Show()
        {
            Target.Show();
        }

        public override bool Equals(ISafeComWrapper<VB.CodePane> other)
        {
            return IsEqualIfNull(other) || (other != null && ReferenceEquals(other.Target, Target));
        }

        public bool Equals(ICodePane other)
        {
            return Equals(other as SafeComWrapper<VB.CodePane>);
        }

        public override int GetHashCode()
        {
            return IsWrappingNullReference ? 0 : Target.GetHashCode();
        }
    }
}