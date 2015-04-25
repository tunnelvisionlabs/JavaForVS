namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using Antlr.Runtime;
    using Microsoft.VisualStudio.Text;
    using Tvl.VisualStudio.Language.Parsing;

    //    package com.sun.tools.javac.antlr;
    //    import com.sun.tools.javac.tree.JCTree.*;
    //    import com.sun.tools.javac.tree.JCTree;
    //    import com.sun.tools.javac.tree.TreeMaker;
    //    import com.sun.tools.javac.util.Context;
    //    import com.sun.tools.javac.util.*;
    //    import com.sun.tools.javac.code.*;
    //    import com.sun.tools.javac.code.TypeTags;

    partial class Java2Parser
    {
        public event EventHandler<ParseErrorEventArgs> ParseError;

        protected bool EnumSupported
        {
            get
            {
                return true;
                //return source.allowEnums;
            }
        }

        protected bool AssertSupported
        {
            get
            {
                return true;
                //return source.allowAsserts;
            }
        }

        protected bool VarArgSupported
        {
            get
            {
                return true;
                //return source.allowVarargs;
            }
        }

        protected bool ForeachSupported
        {
            get
            {
                return true;
                //return source.allowForeach;
            }
        }

        protected bool StaticImportSupported
        {
            get
            {
                return true;
                //return source.allowStaticImport;
            }
        }

        protected bool AnnotationSupported
        {
            get
            {
                return true;
                //return source.allowAnnotations;
            }
        }

        protected bool GenericSupported
        {
            get
            {
                return true;
                //return source.allowGenerics;
            }
        }

        public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
        {
            string header = GetErrorHeader(e);
            string message = GetErrorMessage(e, tokenNames);
            Span span = new Span();
            if (e.Token != null)
                span = Span.FromBounds(e.Token.StartIndex, e.Token.StopIndex + 1);

            ParseErrorEventArgs args = new ParseErrorEventArgs(message, span);
            OnParseError(args);

            base.DisplayRecognitionError(tokenNames, e);
        }

        protected virtual void OnParseError(ParseErrorEventArgs e)
        {
            var t = ParseError;
            if (t != null)
                t(this, e);
        }

#if false
        bool debug = true;

        // temp variables
        private int ti = 0;
        private string ts = null;

        public Java2Parser(ITokenStream input, AntlrParserFactory fac, bool keepDocComments, bool keepEndPosition, bool keepLineMap, char[] rawInput)
        {
            this(input);
            //NOTE: Don't try to put this in super class constructor, must call the antlr generated constructor first.
            super.init(fac, keepDocComments, keepEndPosition, keepLineMap, rawInput);
        }

        public JCCompilationUnit parseCompilationUnit()
        {
            if (super.isLexingError() == true)
            {
                // Errors should be logged in lexer already, no more parsing.
                return TREE_BLANK;
            }

            if (input.LA(1) == -1)
            {
                // Blank input, end of file reached.
                return TREE_BLANK;
            }

            try
            {
                return this.compilationUnit().tree;
            }
            catch (Exception e)
            {
                if (debug)
                {
                    e.printStackTrace();
                }
                //TODO: could be various reasons, how to track? 
                log.error(0, "premature.eof");
                return TREE_BLANK;
            }
        }

        protected bool enumSupported()
        {
            return source.allowGenerics();
        }

        protected bool assertSupported()
        {
            return source.allowAsserts();
        }

        protected bool varArgSupported()
        {
            return source.allowVarargs();
        }

        protected bool foreachSupported()
        {
            return source.allowForeach();
        }

        protected bool staticImportSupported()
        {
            return source.allowStaticImport();
        }

        protected bool annotationSupported()
        {
            return source.allowAnnotations();
        }

        protected bool genericSupported()
        {
            return source.allowGenerics();
        }

        protected void checkEnum(IToken token)
        {
            if (enumSupported() == false && enumErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "enums.not.supported.in.source", source.name);
                enumErrorDisplayed = true;
            }
        }

        protected void checkAssert(IToken token)
        {
            if (assertSupported() == false && assertErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "assert.as.identifier", source.name);
                assertErrorDisplayed = true;
            }
        }

        protected void checkVarArg(IToken token)
        {
            if (varArgSupported() == false && varArgErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "varargs.not.supported.in.source", source.name);
                varArgErrorDisplayed = true;
            }
        }

        protected void checkForeach(IToken token)
        {
            if (foreachSupported() == false && foreachErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "foreach.not.supported.in.source", source.name);
                foreachErrorDisplayed = true;
            }
        }

        protected void checkStaticImport(IToken token)
        {
            if (staticImportSupported() == false && staticImportErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "static.import.not.supported.in.source", source.name);
                staticImportErrorDisplayed = true;
            }
        }

        protected void checkAnnotation(IToken token)
        {
            if (annotationSupported() == false && annotationErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "annotations.not.supported.in.source", source.name);
                annotationErrorDisplayed = true;
            }
        }

        protected void checkGeneric(IToken token)
        {
            if (genericSupported() == false && genericErrorDisplayed == false)
            {
                log.error(((AntlrJavacToken)token).getStartIndex(), "generics.not.supported.in.source", source.name);
                genericErrorDisplayed = true;
            }
        }
#endif
    }
}
