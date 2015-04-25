namespace Tvl.VisualStudio.Language.Java
{
    using System;
    using Antlr.Runtime;

    //@lexer::header {
    //    package com.sun.tools.javac.antlr;
    //    import com.sun.tools.javac.code.Source;
    //    import com.sun.tools.javac.util.Context;
    //    import com.sun.tools.javac.util.Log;
    //    import com.sun.tools.javac.util.Name;
    //    import com.sun.tools.javac.util.Names;
    //    import com.sun.tools.javac.util.Position;
    //}

    partial class Java2Lexer
    {
        public Java2Lexer(JavaUnicodeStream input)
            : this((ICharStream)input)
        {
        }

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

        public override ICharStream CharStream
        {
            get
            {
                return base.CharStream;
            }

            set
            {
                JavaUnicodeStream unicodeStream = value as JavaUnicodeStream;
                if (unicodeStream == null && value != null)
                    unicodeStream = new JavaUnicodeStream(value);

                base.CharStream = unicodeStream;
            }
        }

        private bool IsValidSurrogateIdentifierStart(char high, char low)
        {
            throw new NotImplementedException();
            //if (Character.isSurrogatePair(high, low) && Character.isJavaIdentifierStart(Character.toCodePoint(high, low)))
            //{
            //    return true;
            //}

            //return false;
        }

        private bool IsValidSurrogateIdentifierPart(char high, char low)
        {
            throw new NotImplementedException();
            //if (Character.isSurrogatePair(high, low) && Character.isJavaIdentifierPart(Character.toCodePoint(high, low)))
            //{
            //    return true;
            //}

            //return false;
        } 

        //    /* NOTE: Lots of the code below should be put into a super class. But Antlr does not have 
        //     * superClass option support for combined lexer grammar yet. 
        //     */

        //    protected Context context;
        //    protected Names names;
        //    protected Source source;
        //    protected Log log;    

        //    /** Whether or not to keep a line map. Not used now. */
        //    private boolean keepLineMap;

        //    /** Radix of number tokens. */
        //    private int radix = 10;

        //    /** Buffer holding String and char literal's content. */
        //    StringBuffer stringBuffer = new StringBuffer();

        //    public JavaLexer(CharStream input, com.sun.tools.javac.util.Context context, boolean keepLineMap){
        //        super(input,new RecognizerSharedState());
        //        this.context = context;
        //        this.log = Log.instance(context);
        //        this.names = Names.instance(context);
        //        this.source = Source.instance(context);
        //        this.keepLineMap = keepLineMap;    
        //    }

        //    public Name stringToName(String s) {
        //        return names.fromString(s);
        //    }    

        //    protected char digitsToChar(int base, int... cs) {
        //        int ret = 0;
        //        if (cs == null || cs.length == 0) {
        //            return 0;
        //        }
        //        for (int i = 0; i < cs.length; i++) {
        //            ret = ret * base + Character.digit(cs[i], base);
        //        }
        //        return (char) ret;
        //    }  

        //    /**
        //     * Override Antlr default implementation, no recovering.  
        //     */
        //    public void reportError(RecognitionException e) {
        //        // super.recover(e);  no recovering
        //        e.printStackTrace();
        //        log.error("illegal.unicode.esc");// TODO Antlr - exception process in lexer
        //    }

        //    /**
        //     * Override default implementation. Nothing will be printed out.
        //     */
        //    public void displayRecognitionError(String[] tokenNames, RecognitionException e) {
        //    }    

        //    protected boolean enumSupported() {
        //        return source.allowGenerics();
        //    }

        //    protected boolean assertSupported() {
        //        return source.allowAsserts();
        //    }

        //    private boolean isValidSurrogateIdentifierStart(char high, char low){
        //        if (Character.isSurrogatePair(high, low) && Character.isJavaIdentifierStart(Character.toCodePoint(high, low))){
        //            return true;
        //        } 
        //        return false;
        //    }
        //    private boolean isValidSurrogateIdentifierPart(char high, char low){
        //        if (Character.isSurrogatePair(high, low) && Character.isJavaIdentifierPart(Character.toCodePoint(high, low))){
        //            return true;
        //        }
        //        return false;
        //    } 

        //    /**
        //     * Overridden default Antlr implementation to generated AntlrToken. 
        //     * Default implementation generates CommonToken. 
        //     */    
        //    public Token emit(){
        //        AntlrJavacToken antlrToken = new AntlrJavacToken(input, state.type, state.channel, state.tokenStartCharIndex, getCharIndex() - 1);
        //        antlrToken.setLine(state.tokenStartLine);
        //        antlrToken.setText(state.text);
        //        antlrToken.setCharPositionInLine(state.tokenStartCharPositionInLine);
        //        emit(antlrToken);
        //        /* Code above is copied from antlr. */

        //        int stype = state.type;
        //        switch (stype) {
        //        case LONGLITERAL :
        //        case INTLITERAL :
        //        case FLOATLITERAL :
        //        case DOUBLELITERAL :
        //            String stringVal = antlrToken.getText();
        //            if(radix == 16){
        //                stringVal = stringVal.substring(2,stringVal.length());
        //            }else if(radix == 8 && "0".equals(stringVal) == false){
        //                stringVal = stringVal.substring(1,stringVal.length());
        //            }
        //            if(stype == LONGLITERAL){
        //                stringVal = stringVal.substring(0,stringVal.length()-1);
        //            }
        //            antlrToken.stringVal = stringVal;
        //            antlrToken.radix = radix;
        //            return antlrToken;

        //        case STRINGLITERAL :
        //        case CHARLITERAL :
        //            antlrToken.stringVal = stringBuffer.toString();
        //            stringBuffer.delete(0,stringBuffer.length());
        //            return antlrToken;

        //        case IDENTIFIER :
        //            antlrToken.name = stringToName(antlrToken.getText());
        //            return antlrToken;
        //        }// end switch
        //        return antlrToken;
        //    }// end emit()   
    }
}
