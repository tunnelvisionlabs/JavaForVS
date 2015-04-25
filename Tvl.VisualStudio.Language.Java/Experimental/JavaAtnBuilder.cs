namespace Tvl.VisualStudio.Language.Java.Experimental
{
    using System.Collections.Generic;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    internal class JavaAtnBuilder : NetworkBuilder
    {
        private readonly RuleBindings _ruleBindings;
        private readonly List<RuleBinding> _rules;

        public JavaAtnBuilder()
        {
            _ruleBindings = new RuleBindings();
            _rules =
                new List<RuleBinding>()
                {
                    Bindings.CompilationUnit,
                    Bindings.PackageDeclaration,
                    Bindings.ImportDeclaration,
                    Bindings.TypeDeclaration,
                    Bindings.ClassOrInterfaceDeclaration,
                    Bindings.Modifiers,
                    Bindings.VariableModifiers,
                    Bindings.ClassDeclaration,
                    Bindings.NormalClassDeclaration,
                    Bindings.NormalClassExtends,
                    Bindings.ImplementsTypeList,
                    Bindings.ExtendsTypeList,
                    Bindings.TypeParameters,
                    Bindings.TypeParameter,
                    Bindings.TypeBound,
                    Bindings.EnumDeclaration,
                    Bindings.EnumHeader,
                    Bindings.EnumBody,
                    Bindings.EnumConstants,
                    Bindings.EnumConstant,
                    Bindings.EnumBodyDeclarations,
                    Bindings.InterfaceDeclaration,
                    Bindings.NormalInterfaceDeclaration,
                    Bindings.InterfaceHeader,
                    Bindings.TypeList,
                    Bindings.ClassBody,
                    Bindings.InterfaceBody,
                    Bindings.ClassBodyDeclaration,
                    Bindings.MemberDecl,
                    Bindings.MethodDeclaration,
                    Bindings.ConstructorMethodBody,
                    Bindings.MethodBody,
                    Bindings.ThrowsSpec,
                    Bindings.FieldDeclaration,
                    Bindings.VariableDeclarator,
                    Bindings.InterfaceBodyDeclaration,
                    Bindings.InterfaceMethodDeclaration,
                    Bindings.InterfaceFieldDeclaration,
                    Bindings.Type,
                    Bindings.ClassOrInterfaceType,
                    Bindings.GenericIdentifier,
                    Bindings.PrimitiveType,
                    Bindings.TypeArguments,
                    Bindings.TypeArgument,
                    Bindings.QualifiedNameList,
                    Bindings.FormalParameters,
                    Bindings.FormalParameterDecls,
                    Bindings.NormalParameterDecl,
                    Bindings.EllipsisParameterDecl,
                    Bindings.ExplicitConstructorInvocation,
                    Bindings.QualifiedName,
                    Bindings.Annotations,
                    Bindings.Annotation,
                    Bindings.ElementValuePairs,
                    Bindings.ElementValuePair,
                    Bindings.ElementValue,
                    Bindings.ElementValueArrayInitializer,
                    Bindings.AnnotationTypeDeclaration,
                    Bindings.AnnotationInterfaceHeader,
                    Bindings.AnnotationTypeBody,
                    Bindings.AnnotationTypeElementDeclaration,
                    Bindings.AnnotationMethodDeclaration,
                    Bindings.Block,
                    Bindings.StaticBlock,
                    Bindings.BlockStatement,
                    Bindings.LocalVariableDeclarationStatement,
                    Bindings.LocalVariableDeclaration,
                    Bindings.Statement,
                    Bindings.SwitchBlockStatementGroups,
                    Bindings.SwitchBlockStatementGroup,
                    Bindings.SwitchLabel,
                    Bindings.TryStatement,
                    Bindings.FinallyBlock,
                    Bindings.Catches,
                    Bindings.CatchClause,
                    Bindings.FormalParameter,
                    Bindings.ForStatement,
                    Bindings.ForInit,
                    Bindings.ParExpression,
                    Bindings.ExpressionList,
                    Bindings.Expression,
                    Bindings.SymbolDefinitionIdentifier,
                    Bindings.SymbolReferenceIdentifier,

                    Bindings.AssignmentOperator,
                    Bindings.ConditionalExpression,
                    Bindings.ConditionalOrExpression,
                    Bindings.ConditionalAndExpression,
                    Bindings.InclusiveOrExpression,
                    Bindings.ExclusiveOrExpression,
                    Bindings.AndExpression,
                    Bindings.EqualityExpression,
                    Bindings.InstanceOfExpression,
                    Bindings.RelationalExpression,
                    Bindings.RelationalOp,
                    Bindings.ShiftExpression,
                    Bindings.ShiftOp,
                    Bindings.AdditiveExpression,
                    Bindings.MultiplicativeExpression,
                    Bindings.UnaryExpression,
                    Bindings.UnaryExpressionNotPlusMinus,
                    Bindings.CastExpression,

                    Bindings.Primary,
                    Bindings.SuperSuffix,
                    Bindings.IdentifierSuffix,
                    Bindings.Selector,
                    Bindings.Creator,
                    Bindings.ArrayCreator,
                    Bindings.VariableInitializer,
                    Bindings.ArrayInitializer,
                    Bindings.CreatedName,
                    Bindings.InnerCreator,
                    Bindings.ClassCreatorRest,
                    Bindings.NonWildcardTypeArguments,
                    Bindings.Arguments,
                    Bindings.Literal,

                    Bindings.ClassHeader,
                };
        }

        protected RuleBindings Bindings
        {
            get
            {
                return _ruleBindings;
            }
        }

        protected override IList<RuleBinding> Rules
        {
            get
            {
                return _rules;
            }
        }

        protected sealed override void BindRules()
        {
            BindRulesImpl();
            _rules.RemoveAll(i => i.StartState.OutgoingTransitions.Count == 0);
        }

        protected virtual void BindRulesImpl()
        {
            TryBindRule(Bindings.CompilationUnit, this.CompilationUnit());
            TryBindRule(Bindings.PackageDeclaration, this.PackageDeclaration());
            TryBindRule(Bindings.ImportDeclaration, this.ImportDeclaration());
            TryBindRule(Bindings.TypeDeclaration, this.TypeDeclaration());
            TryBindRule(Bindings.ClassOrInterfaceDeclaration, this.ClassOrInterfaceDeclaration());
            TryBindRule(Bindings.Modifiers, this.Modifiers());
            TryBindRule(Bindings.VariableModifiers, this.VariableModifiers());
            TryBindRule(Bindings.ClassDeclaration, this.ClassDeclaration());
            TryBindRule(Bindings.NormalClassDeclaration, this.NormalClassDeclaration());
            TryBindRule(Bindings.NormalClassExtends, this.NormalClassExtends());
            TryBindRule(Bindings.ImplementsTypeList, this.ImplementsTypeList());
            TryBindRule(Bindings.ExtendsTypeList, this.ExtendsTypeList());
            TryBindRule(Bindings.TypeParameters, this.TypeParameters());
            TryBindRule(Bindings.TypeParameter, this.TypeParameter());
            TryBindRule(Bindings.TypeBound, this.TypeBound());
            TryBindRule(Bindings.EnumDeclaration, this.EnumDeclaration());
            TryBindRule(Bindings.EnumHeader, this.EnumHeader());
            TryBindRule(Bindings.EnumBody, this.EnumBody());
            TryBindRule(Bindings.EnumConstants, this.EnumConstants());
            TryBindRule(Bindings.EnumConstant, this.EnumConstant());
            TryBindRule(Bindings.EnumBodyDeclarations, this.EnumBodyDeclarations());
            TryBindRule(Bindings.InterfaceDeclaration, this.InterfaceDeclaration());
            TryBindRule(Bindings.NormalInterfaceDeclaration, this.NormalInterfaceDeclaration());
            TryBindRule(Bindings.InterfaceHeader, this.InterfaceHeader());
            TryBindRule(Bindings.TypeList, this.TypeList());
            TryBindRule(Bindings.ClassBody, this.ClassBody());
            TryBindRule(Bindings.InterfaceBody, this.InterfaceBody());
            TryBindRule(Bindings.ClassBodyDeclaration, this.ClassBodyDeclaration());
            TryBindRule(Bindings.MemberDecl, this.MemberDecl());
            TryBindRule(Bindings.MethodDeclaration, this.MethodDeclaration());
            TryBindRule(Bindings.ConstructorMethodBody, this.ConstructorMethodBody());
            TryBindRule(Bindings.MethodBody, this.MethodBody());
            TryBindRule(Bindings.ThrowsSpec, this.ThrowsSpec());
            TryBindRule(Bindings.FieldDeclaration, this.FieldDeclaration());
            TryBindRule(Bindings.VariableDeclarator, this.VariableDeclarator());
            TryBindRule(Bindings.InterfaceBodyDeclaration, this.InterfaceBodyDeclaration());
            TryBindRule(Bindings.InterfaceMethodDeclaration, this.InterfaceMethodDeclaration());
            TryBindRule(Bindings.InterfaceFieldDeclaration, this.InterfaceFieldDeclaration());
            TryBindRule(Bindings.Type, this.Type());
            TryBindRule(Bindings.ClassOrInterfaceType, this.ClassOrInterfaceType());
            TryBindRule(Bindings.GenericIdentifier, this.GenericIdentifier());
            TryBindRule(Bindings.PrimitiveType, this.PrimitiveType());
            TryBindRule(Bindings.TypeArguments, this.TypeArguments());
            TryBindRule(Bindings.TypeArgument, this.TypeArgument());
            TryBindRule(Bindings.QualifiedNameList, this.QualifiedNameList());
            TryBindRule(Bindings.FormalParameters, this.FormalParameters());
            TryBindRule(Bindings.FormalParameterDecls, this.FormalParameterDecls());
            TryBindRule(Bindings.NormalParameterDecl, this.NormalParameterDecl());
            TryBindRule(Bindings.EllipsisParameterDecl, this.EllipsisParameterDecl());
            TryBindRule(Bindings.ExplicitConstructorInvocation, this.ExplicitConstructorInvocation());
            TryBindRule(Bindings.QualifiedName, this.QualifiedName());
            TryBindRule(Bindings.Annotations, this.Annotations());
            TryBindRule(Bindings.Annotation, this.Annotation());
            TryBindRule(Bindings.ElementValuePairs, this.ElementValuePairs());
            TryBindRule(Bindings.ElementValuePair, this.ElementValuePair());
            TryBindRule(Bindings.ElementValue, this.ElementValue());
            TryBindRule(Bindings.ElementValueArrayInitializer, this.ElementValueArrayInitializer());
            TryBindRule(Bindings.AnnotationTypeDeclaration, this.AnnotationTypeDeclaration());
            TryBindRule(Bindings.AnnotationInterfaceHeader, this.AnnotationInterfaceHeader());
            TryBindRule(Bindings.AnnotationTypeBody, this.AnnotationTypeBody());
            TryBindRule(Bindings.AnnotationTypeElementDeclaration, this.AnnotationTypeElementDeclaration());
            TryBindRule(Bindings.AnnotationMethodDeclaration, this.AnnotationMethodDeclaration());
            TryBindRule(Bindings.Block, this.Block());
            TryBindRule(Bindings.StaticBlock, this.StaticBlock());
            TryBindRule(Bindings.BlockStatement, this.BlockStatement());
            TryBindRule(Bindings.LocalVariableDeclarationStatement, this.LocalVariableDeclarationStatement());
            TryBindRule(Bindings.LocalVariableDeclaration, this.LocalVariableDeclaration());
            TryBindRule(Bindings.Statement, this.Statement());
            TryBindRule(Bindings.SwitchBlockStatementGroups, this.SwitchBlockStatementGroups());
            TryBindRule(Bindings.SwitchBlockStatementGroup, this.SwitchBlockStatementGroup());
            TryBindRule(Bindings.SwitchLabel, this.SwitchLabel());
            TryBindRule(Bindings.TryStatement, this.TryStatement());
            TryBindRule(Bindings.FinallyBlock, this.FinallyBlock());
            TryBindRule(Bindings.Catches, this.Catches());
            TryBindRule(Bindings.CatchClause, this.CatchClause());
            TryBindRule(Bindings.FormalParameter, this.FormalParameter());
            TryBindRule(Bindings.ForStatement, this.ForStatement());
            TryBindRule(Bindings.ForInit, this.ForInit());
            TryBindRule(Bindings.ParExpression, this.ParExpression());
            TryBindRule(Bindings.ExpressionList, this.ExpressionList());
            TryBindRule(Bindings.Expression, this.Expression());
            TryBindRule(Bindings.SymbolDefinitionIdentifier, this.SymbolDefinitionIdentifier());
            TryBindRule(Bindings.SymbolReferenceIdentifier, this.SymbolReferenceIdentifier());

            TryBindRule(Bindings.AssignmentOperator, this.AssignmentOperator());
            TryBindRule(Bindings.ConditionalExpression, this.ConditionalExpression());
            TryBindRule(Bindings.ConditionalOrExpression, this.ConditionalOrExpression());
            TryBindRule(Bindings.ConditionalAndExpression, this.ConditionalAndExpression());
            TryBindRule(Bindings.InclusiveOrExpression, this.InclusiveOrExpression());
            TryBindRule(Bindings.ExclusiveOrExpression, this.ExclusiveOrExpression());
            TryBindRule(Bindings.AndExpression, this.AndExpression());
            TryBindRule(Bindings.EqualityExpression, this.EqualityExpression());
            TryBindRule(Bindings.InstanceOfExpression, this.InstanceOfExpression());
            TryBindRule(Bindings.RelationalExpression, this.RelationalExpression());
            TryBindRule(Bindings.RelationalOp, this.RelationalOp());
            TryBindRule(Bindings.ShiftExpression, this.ShiftExpression());
            TryBindRule(Bindings.ShiftOp, this.ShiftOp());
            TryBindRule(Bindings.AdditiveExpression, this.AdditiveExpression());
            TryBindRule(Bindings.MultiplicativeExpression, this.MultiplicativeExpression());
            TryBindRule(Bindings.UnaryExpression, this.UnaryExpression());
            TryBindRule(Bindings.UnaryExpressionNotPlusMinus, this.UnaryExpressionNotPlusMinus());
            TryBindRule(Bindings.CastExpression, this.CastExpression());

            TryBindRule(Bindings.Primary, this.Primary());
            TryBindRule(Bindings.SuperSuffix, this.SuperSuffix());
            TryBindRule(Bindings.IdentifierSuffix, this.IdentifierSuffix());
            TryBindRule(Bindings.Selector, this.Selector());
            TryBindRule(Bindings.Creator, this.Creator());
            TryBindRule(Bindings.ArrayCreator, this.ArrayCreator());
            TryBindRule(Bindings.VariableInitializer, this.VariableInitializer());
            TryBindRule(Bindings.ArrayInitializer, this.ArrayInitializer());
            TryBindRule(Bindings.CreatedName, this.CreatedName());
            TryBindRule(Bindings.InnerCreator, this.InnerCreator());
            TryBindRule(Bindings.ClassCreatorRest, this.ClassCreatorRest());
            TryBindRule(Bindings.NonWildcardTypeArguments, this.NonWildcardTypeArguments());
            TryBindRule(Bindings.Arguments, this.Arguments());
            TryBindRule(Bindings.Literal, this.Literal());

            TryBindRule(Bindings.ClassHeader, this.ClassHeader());

            Bindings.CompilationUnit.IsStartRule = true;
        }

        protected virtual Nfa CompilationUnit()
        {
            return Nfa.Sequence(
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Optional(Nfa.Rule(Bindings.Annotations)),
                        Nfa.Rule(Bindings.PackageDeclaration))),
                Nfa.Closure(Nfa.Rule(Bindings.ImportDeclaration)),
                Nfa.Closure(Nfa.Rule(Bindings.TypeDeclaration)),
                Nfa.Match(Java2Lexer.EOF));
        }

        protected virtual Nfa PackageDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.PACKAGE),
                Nfa.Rule(Bindings.QualifiedName),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa ImportDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.IMPORT),
                Nfa.Optional(Nfa.Match(Java2Lexer.STATIC)),
                Nfa.Rule(Bindings.QualifiedName),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.DOT),
                        Nfa.Match(Java2Lexer.STAR))),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa TypeDeclaration()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.ClassOrInterfaceDeclaration),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa ClassOrInterfaceDeclaration()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.ClassDeclaration),
                Nfa.Rule(Bindings.InterfaceDeclaration));
        }

        protected virtual Nfa Modifiers()
        {
            return Nfa.Closure(
                Nfa.Choice(
                    Nfa.Rule(Bindings.Annotation),
                    Nfa.MatchAny(
                        Java2Lexer.PUBLIC,
                        Java2Lexer.PROTECTED,
                        Java2Lexer.PRIVATE,
                        Java2Lexer.STATIC,
                        Java2Lexer.ABSTRACT,
                        Java2Lexer.FINAL,
                        Java2Lexer.NATIVE,
                        Java2Lexer.SYNCHRONIZED,
                        Java2Lexer.TRANSIENT,
                        Java2Lexer.VOLATILE,
                        Java2Lexer.STRICTFP)));
        }

        protected virtual Nfa VariableModifiers()
        {
            return Nfa.Closure(
                Nfa.Choice(
                    Nfa.Match(Java2Lexer.FINAL),
                    Nfa.Rule(Bindings.Annotation)));
        }

        protected virtual Nfa ClassDeclaration()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.NormalClassDeclaration),
                Nfa.Rule(Bindings.EnumDeclaration));
        }

        protected virtual Nfa NormalClassDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ClassHeader),
                Nfa.Rule(Bindings.ClassBody));
        }

        protected virtual Nfa NormalClassExtends()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.EXTENDS),
                Nfa.Rule(Bindings.Type));
        }

        protected virtual Nfa ImplementsTypeList()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.IMPLEMENTS),
                Nfa.Rule(Bindings.TypeList));
        }

        protected virtual Nfa ExtendsTypeList()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.EXTENDS),
                Nfa.Rule(Bindings.TypeList));
        }

        protected virtual Nfa TypeParameters()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LT),
                Nfa.Rule(Bindings.TypeParameter),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.TypeParameter))),
                Nfa.Match(Java2Lexer.GT));
        }

        protected virtual Nfa TypeParameter()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.EXTENDS),
                        Nfa.Rule(Bindings.TypeBound))));
        }

        protected virtual Nfa TypeBound()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Type),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.AMP),
                        Nfa.Rule(Bindings.Type))));
        }

        protected virtual Nfa EnumDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.EnumHeader),
                Nfa.Rule(Bindings.EnumBody));
        }

        protected virtual Nfa EnumHeader()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Match(Java2Lexer.ENUM),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Optional(Nfa.Rule(Bindings.ImplementsTypeList)));
        }

        protected virtual Nfa EnumBody()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Optional(Nfa.Rule(Bindings.EnumConstants)),
                Nfa.Optional(Nfa.Match(Java2Lexer.COMMA)),
                Nfa.Optional(Nfa.Rule(Bindings.EnumBodyDeclarations)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa EnumConstants()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.EnumConstant),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.EnumConstant))));
        }

        protected virtual Nfa EnumConstant()
        {
            return Nfa.Sequence(
                Nfa.Optional(Nfa.Rule(Bindings.Annotations)),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Optional(Nfa.Rule(Bindings.Arguments)),
                Nfa.Optional(Nfa.Rule(Bindings.ClassBody)));
        }

        protected virtual Nfa EnumBodyDeclarations()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.SEMI),
                Nfa.Closure(Nfa.Rule(Bindings.ClassBodyDeclaration)));
        }

        protected virtual Nfa InterfaceDeclaration()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.NormalInterfaceDeclaration),
                Nfa.Rule(Bindings.AnnotationTypeDeclaration));
        }

        protected virtual Nfa NormalInterfaceDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.InterfaceHeader),
                Nfa.Rule(Bindings.InterfaceBody));
        }

        protected virtual Nfa InterfaceHeader()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Match(Java2Lexer.INTERFACE),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Optional(Nfa.Rule(Bindings.TypeParameters)),
                Nfa.Optional(Nfa.Rule(Bindings.ExtendsTypeList)));
        }

        protected virtual Nfa TypeList()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Type),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.Type))));
        }

        protected virtual Nfa ClassBody()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Closure(Nfa.Rule(Bindings.ClassBodyDeclaration)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa InterfaceBody()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Closure(Nfa.Rule(Bindings.InterfaceBodyDeclaration)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa ClassBodyDeclaration()
        {
            return Nfa.Choice(
                Nfa.Match(Java2Lexer.SEMI),
                Nfa.Rule(Bindings.StaticBlock),
                Nfa.Rule(Bindings.Block),
                Nfa.Rule(Bindings.MemberDecl));
        }

        protected virtual Nfa MemberDecl()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.FieldDeclaration),
                Nfa.Rule(Bindings.MethodDeclaration),
                Nfa.Rule(Bindings.ClassDeclaration),
                Nfa.Rule(Bindings.InterfaceDeclaration));
        }

        protected virtual Nfa MethodDeclaration()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Rule(Bindings.Modifiers),
                    Nfa.Optional(Nfa.Rule(Bindings.TypeParameters)),
                    Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                    Nfa.Rule(Bindings.FormalParameters),
                    Nfa.Optional(Nfa.Rule(Bindings.ThrowsSpec)),
                    Nfa.Rule(Bindings.ConstructorMethodBody)),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.Modifiers),
                    Nfa.Optional(Nfa.Rule(Bindings.TypeParameters)),
                    Nfa.Choice(
                        Nfa.Rule(Bindings.Type),
                        Nfa.Match(Java2Lexer.VOID)),
                    Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                    Nfa.Rule(Bindings.FormalParameters),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Match(Java2Lexer.RBRACKET))),
                    Nfa.Optional(Nfa.Rule(Bindings.ThrowsSpec)),
                    Nfa.Choice(
                        Nfa.Rule(Bindings.MethodBody),
                        Nfa.Match(Java2Lexer.SEMI))));
        }

        protected virtual Nfa ConstructorMethodBody()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Optional(Nfa.Rule(Bindings.ExplicitConstructorInvocation)),
                Nfa.Closure(Nfa.Rule(Bindings.BlockStatement)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa MethodBody()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Closure(Nfa.Rule(Bindings.BlockStatement)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa ThrowsSpec()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.THROWS),
                Nfa.Rule(Bindings.QualifiedNameList));
        }

        protected virtual Nfa FieldDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Rule(Bindings.VariableDeclarator),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.VariableDeclarator))),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa VariableDeclarator()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.LBRACKET),
                        Nfa.Match(Java2Lexer.RBRACKET))),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.EQ),
                        Nfa.Rule(Bindings.VariableInitializer))));
        }

        protected virtual Nfa InterfaceBodyDeclaration()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.InterfaceFieldDeclaration),
                Nfa.Rule(Bindings.InterfaceMethodDeclaration),
                Nfa.Rule(Bindings.InterfaceDeclaration),
                Nfa.Rule(Bindings.ClassDeclaration),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa InterfaceMethodDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Optional(Nfa.Rule(Bindings.TypeParameters)),
                Nfa.Choice(
                    Nfa.Rule(Bindings.Type),
                    Nfa.Match(Java2Lexer.VOID)),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Rule(Bindings.FormalParameters),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.LBRACKET),
                        Nfa.Match(Java2Lexer.RBRACKET))),
                Nfa.Optional(Nfa.Rule(Bindings.ThrowsSpec)),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa InterfaceFieldDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Rule(Bindings.VariableDeclarator),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.VariableDeclarator))),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa Type()
        {
            return Nfa.Sequence(
                Nfa.Choice(
                    Nfa.Rule(Bindings.ClassOrInterfaceType),
                    Nfa.Rule(Bindings.PrimitiveType)),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.LBRACKET),
                        Nfa.Match(Java2Lexer.RBRACKET))));
        }

        protected virtual Nfa ClassOrInterfaceType()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.GenericIdentifier),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.DOT),
                        Nfa.Rule(Bindings.GenericIdentifier))));
        }

        protected virtual Nfa GenericIdentifier()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                Nfa.Optional(Nfa.Rule(Bindings.TypeArguments)));
        }

        protected virtual Nfa PrimitiveType()
        {
            return Nfa.MatchAny(
                Java2Lexer.BOOLEAN,
                Java2Lexer.CHAR,
                Java2Lexer.BYTE,
                Java2Lexer.SHORT,
                Java2Lexer.INT,
                Java2Lexer.LONG,
                Java2Lexer.FLOAT,
                Java2Lexer.DOUBLE);
        }

        protected virtual Nfa TypeArguments()
        {
            return Nfa.Sequence(
                Nfa.MatchAny(Java2Lexer.LT),
                Nfa.Rule(Bindings.TypeArgument),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.TypeArgument))),
                Nfa.Match(Java2Lexer.GT));
        }

        protected virtual Nfa TypeArgument()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.Type),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.QUES),
                    Nfa.Optional(
                        Nfa.Sequence(
                            Nfa.MatchAny(Java2Lexer.EXTENDS, Java2Lexer.SUPER),
                            Nfa.Rule(Bindings.Type)))));
        }

        protected virtual Nfa QualifiedNameList()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.QualifiedName),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.QualifiedName))));
        }

        protected virtual Nfa FormalParameters()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LPAREN),
                Nfa.Optional(Nfa.Rule(Bindings.FormalParameterDecls)),
                Nfa.Match(Java2Lexer.RPAREN));
        }

        protected virtual Nfa FormalParameterDecls()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.EllipsisParameterDecl),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.NormalParameterDecl),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.COMMA),
                            Nfa.Rule(Bindings.NormalParameterDecl)))),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.NormalParameterDecl),
                    Nfa.Match(Java2Lexer.COMMA),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Rule(Bindings.NormalParameterDecl),
                            Nfa.Match(Java2Lexer.COMMA))),
                    Nfa.Rule(Bindings.EllipsisParameterDecl)));
        }

        protected virtual Nfa NormalParameterDecl()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.VariableModifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.LBRACKET),
                        Nfa.Match(Java2Lexer.RBRACKET))));
        }

        protected virtual Nfa EllipsisParameterDecl()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.VariableModifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Match(Java2Lexer.ELLIPSIS),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier));
        }

        protected virtual Nfa ExplicitConstructorInvocation()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Optional(Nfa.Rule(Bindings.NonWildcardTypeArguments)),
                    Nfa.MatchAny(Java2Lexer.THIS, Java2Lexer.SUPER),
                    Nfa.Rule(Bindings.Arguments),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.Primary),
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Optional(Nfa.Rule(Bindings.NonWildcardTypeArguments)),
                    Nfa.Match(Java2Lexer.SUPER),
                    Nfa.Rule(Bindings.Arguments),
                    Nfa.Match(Java2Lexer.SEMI)));
        }

        protected virtual Nfa QualifiedName()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.DOT),
                        Nfa.Rule(Bindings.SymbolReferenceIdentifier))));
        }

        protected virtual Nfa Annotations()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Annotation),
                Nfa.Closure(Nfa.Rule(Bindings.Annotation)));
        }

        protected virtual Nfa Annotation()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.MONKEYS_AT),
                Nfa.Rule(Bindings.QualifiedName),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.LPAREN),
                        Nfa.Optional(
                            Nfa.Choice(
                                Nfa.Rule(Bindings.ElementValuePairs),
                                Nfa.Rule(Bindings.ElementValue))),
                        Nfa.Match(Java2Lexer.RPAREN))));
        }

        protected virtual Nfa ElementValuePairs()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ElementValuePair),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.ElementValuePair))));
        }

        protected virtual Nfa ElementValuePair()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                Nfa.Match(Java2Lexer.EQ),
                Nfa.Rule(Bindings.ElementValue));
        }

        protected virtual Nfa ElementValue()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.ConditionalExpression),
                Nfa.Rule(Bindings.Annotation),
                Nfa.Rule(Bindings.ElementValueArrayInitializer));
        }

        protected virtual Nfa ElementValueArrayInitializer()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Rule(Bindings.ElementValue),
                        Nfa.Closure(
                            Nfa.Sequence(
                                Nfa.Match(Java2Lexer.COMMA),
                                Nfa.Rule(Bindings.ElementValue))))),
                Nfa.Optional(Nfa.Match(Java2Lexer.COMMA)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa AnnotationTypeDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.AnnotationInterfaceHeader),
                Nfa.Rule(Bindings.AnnotationTypeBody));
        }

        protected virtual Nfa AnnotationInterfaceHeader()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Match(Java2Lexer.MONKEYS_AT),
                Nfa.Match(Java2Lexer.INTERFACE),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier));
        }

        protected virtual Nfa AnnotationTypeBody()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Closure(Nfa.Rule(Bindings.AnnotationTypeElementDeclaration)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa AnnotationTypeElementDeclaration()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.AnnotationMethodDeclaration),
                Nfa.Rule(Bindings.InterfaceFieldDeclaration),
                Nfa.Rule(Bindings.NormalClassDeclaration),
                Nfa.Rule(Bindings.NormalInterfaceDeclaration),
                Nfa.Rule(Bindings.EnumDeclaration),
                Nfa.Rule(Bindings.AnnotationTypeDeclaration),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa AnnotationMethodDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Match(Java2Lexer.LPAREN),
                Nfa.Match(Java2Lexer.RPAREN),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.DEFAULT),
                        Nfa.Rule(Bindings.ElementValue))),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa Block()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Closure(Nfa.Rule(Bindings.BlockStatement)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa StaticBlock()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.STATIC),
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Closure(Nfa.Rule(Bindings.BlockStatement)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa BlockStatement()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.LocalVariableDeclarationStatement),
                Nfa.Rule(Bindings.ClassOrInterfaceDeclaration),
                Nfa.Rule(Bindings.Statement));
        }

        protected virtual Nfa LocalVariableDeclarationStatement()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.LocalVariableDeclaration),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa LocalVariableDeclaration()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.VariableModifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Rule(Bindings.VariableDeclarator),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.VariableDeclarator))));
        }

        protected virtual Nfa Statement()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.Block),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.ASSERT),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Optional(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.COLON),
                            Nfa.Rule(Bindings.Expression))),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.IF),
                    Nfa.Rule(Bindings.ParExpression),
                    Nfa.Rule(Bindings.Statement),
                    Nfa.Optional(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.ELSE),
                            Nfa.Rule(Bindings.Statement)))),
                Nfa.Rule(Bindings.ForStatement),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.WHILE),
                    Nfa.Rule(Bindings.ParExpression),
                    Nfa.Rule(Bindings.Statement)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DO),
                    Nfa.Rule(Bindings.Statement),
                    Nfa.Match(Java2Lexer.WHILE),
                    Nfa.Rule(Bindings.ParExpression),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Rule(Bindings.TryStatement),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.SWITCH),
                    Nfa.Rule(Bindings.ParExpression),
                    Nfa.Match(Java2Lexer.LBRACE),
                    Nfa.Rule(Bindings.SwitchBlockStatementGroups),
                    Nfa.Match(Java2Lexer.RBRACE)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.SYNCHRONIZED),
                    Nfa.Rule(Bindings.ParExpression),
                    Nfa.Rule(Bindings.Block)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.RETURN),
                    Nfa.Optional(Nfa.Rule(Bindings.Expression)),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.THROW),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Sequence(
                    Nfa.MatchAny(Java2Lexer.BREAK, Java2Lexer.CONTINUE),
                    Nfa.Optional(Nfa.Rule(Bindings.SymbolReferenceIdentifier)),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.SEMI)),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                    Nfa.Match(Java2Lexer.COLON),
                    Nfa.Rule(Bindings.Statement)),
                Nfa.Match(Java2Lexer.SEMI));
        }

        protected virtual Nfa SwitchBlockStatementGroups()
        {
            return Nfa.Closure(Nfa.Rule(Bindings.SwitchBlockStatementGroup));
        }

        protected virtual Nfa SwitchBlockStatementGroup()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.SwitchLabel),
                Nfa.Closure(Nfa.Rule(Bindings.BlockStatement)));
        }

        protected virtual Nfa SwitchLabel()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.CASE),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.COLON)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DEFAULT),
                    Nfa.Match(Java2Lexer.COLON)));
        }

        protected virtual Nfa TryStatement()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.TRY),
                Nfa.Rule(Bindings.Block),
                Nfa.Choice(
                    Nfa.Sequence(
                        Nfa.Rule(Bindings.Catches),
                        Nfa.Rule(Bindings.FinallyBlock)),
                    Nfa.Rule(Bindings.Catches),
                    Nfa.Rule(Bindings.FinallyBlock)));
        }

        protected virtual Nfa FinallyBlock()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.FINALLY),
                Nfa.Rule(Bindings.Block));
        }

        protected virtual Nfa Catches()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.CatchClause),
                Nfa.Closure(Nfa.Rule(Bindings.CatchClause)));
        }

        protected virtual Nfa CatchClause()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.CATCH),
                Nfa.Match(Java2Lexer.LPAREN),
                Nfa.Rule(Bindings.FormalParameter),
                Nfa.Match(Java2Lexer.RPAREN),
                Nfa.Rule(Bindings.Block));
        }

        protected virtual Nfa FormalParameter()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.VariableModifiers),
                Nfa.Rule(Bindings.Type),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.LBRACKET),
                        Nfa.Match(Java2Lexer.RBRACKET))));
        }

        protected virtual Nfa ForStatement()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.FOR),
                    Nfa.Match(Java2Lexer.LPAREN),
                    Nfa.Rule(Bindings.VariableModifiers),
                    Nfa.Rule(Bindings.Type),
                    Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                    Nfa.Match(Java2Lexer.COLON),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.RPAREN),
                    Nfa.Rule(Bindings.Statement)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.FOR),
                    Nfa.Match(Java2Lexer.LPAREN),
                    Nfa.Optional(Nfa.Rule(Bindings.ForInit)),
                    Nfa.Match(Java2Lexer.SEMI),
                    Nfa.Optional(Nfa.Rule(Bindings.Expression)),
                    Nfa.Match(Java2Lexer.SEMI),
                    Nfa.Optional(Nfa.Rule(Bindings.ExpressionList)),
                    Nfa.Match(Java2Lexer.RPAREN),
                    Nfa.Rule(Bindings.Statement)));
        }

        protected virtual Nfa ForInit()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.LocalVariableDeclaration),
                Nfa.Rule(Bindings.ExpressionList));
        }

        protected virtual Nfa ParExpression()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LPAREN),
                Nfa.Rule(Bindings.Expression),
                Nfa.Match(Java2Lexer.RPAREN));
        }

        protected virtual Nfa ExpressionList()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Expression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.COMMA),
                        Nfa.Rule(Bindings.Expression))));
        }

        protected virtual Nfa Expression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ConditionalExpression),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Rule(Bindings.AssignmentOperator),
                        Nfa.Rule(Bindings.Expression))));
        }

        protected virtual Nfa SymbolDefinitionIdentifier()
        {
            return Nfa.Match(Java2Lexer.IDENTIFIER);
        }

        protected virtual Nfa SymbolReferenceIdentifier()
        {
            return Nfa.Match(Java2Lexer.IDENTIFIER);
        }

        protected virtual Nfa AssignmentOperator()
        {
            return Nfa.Choice(
                Nfa.MatchAny(
                    Java2Lexer.EQ,
                    Java2Lexer.PLUSEQ,
                    Java2Lexer.SUBEQ,
                    Java2Lexer.STAREQ,
                    Java2Lexer.SLASHEQ,
                    Java2Lexer.AMPEQ,
                    Java2Lexer.BAREQ,
                    Java2Lexer.CARETEQ,
                    Java2Lexer.PERCENTEQ),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LT),
                    Nfa.Match(Java2Lexer.LT),
                    Nfa.Match(Java2Lexer.EQ)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.GT),
                    Nfa.Match(Java2Lexer.GT),
                    Nfa.Optional(Nfa.Match(Java2Lexer.GT)),
                    Nfa.MatchAny(Java2Lexer.EQ)));
        }

        protected virtual Nfa ConditionalExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ConditionalOrExpression),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.QUES),
                        Nfa.Rule(Bindings.Expression),
                        Nfa.Match(Java2Lexer.COLON),
                        Nfa.Rule(Bindings.ConditionalExpression))));
        }

        protected virtual Nfa ConditionalOrExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ConditionalAndExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.BARBAR),
                        Nfa.Rule(Bindings.ConditionalAndExpression))));
        }

        protected virtual Nfa ConditionalAndExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.InclusiveOrExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.AMPAMP),
                        Nfa.Rule(Bindings.InclusiveOrExpression))));
        }

        protected virtual Nfa InclusiveOrExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ExclusiveOrExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.BAR),
                        Nfa.Rule(Bindings.ExclusiveOrExpression))));
        }

        protected virtual Nfa ExclusiveOrExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.AndExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.CARET),
                        Nfa.Rule(Bindings.AndExpression))));
        }

        protected virtual Nfa AndExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.EqualityExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.AMP),
                        Nfa.Rule(Bindings.EqualityExpression))));
        }

        protected virtual Nfa EqualityExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.InstanceOfExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.MatchAny(Java2Lexer.EQEQ, Java2Lexer.BANGEQ),
                        Nfa.Rule(Bindings.InstanceOfExpression))));
        }

        protected virtual Nfa InstanceOfExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.RelationalExpression),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Match(Java2Lexer.INSTANCEOF),
                        Nfa.Rule(Bindings.Type))));
        }

        protected virtual Nfa RelationalExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.ShiftExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Rule(Bindings.RelationalOp),
                        Nfa.Rule(Bindings.ShiftExpression))));
        }

        protected virtual Nfa RelationalOp()
        {
            return Nfa.Sequence(
                Nfa.MatchAny(Java2Lexer.LT, Java2Lexer.GT),
                Nfa.Optional(Nfa.Match(Java2Lexer.EQ)));
        }

        protected virtual Nfa ShiftExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.AdditiveExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.Rule(Bindings.ShiftOp),
                        Nfa.Rule(Bindings.AdditiveExpression))));
        }

        protected virtual Nfa ShiftOp()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LT),
                    Nfa.Match(Java2Lexer.LT)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.GT),
                    Nfa.Match(Java2Lexer.GT),
                    Nfa.Optional(Nfa.Match(Java2Lexer.GT))));
        }

        protected virtual Nfa AdditiveExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.MultiplicativeExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.MatchAny(Java2Lexer.PLUS, Java2Lexer.SUB),
                        Nfa.Rule(Bindings.MultiplicativeExpression))));
        }

        protected virtual Nfa MultiplicativeExpression()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.UnaryExpression),
                Nfa.Closure(
                    Nfa.Sequence(
                        Nfa.MatchAny(Java2Lexer.STAR, Java2Lexer.SLASH, Java2Lexer.PERCENT),
                        Nfa.Rule(Bindings.UnaryExpression))));
        }

        protected virtual Nfa UnaryExpression()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.MatchAny(Java2Lexer.PLUS, Java2Lexer.SUB, Java2Lexer.PLUSPLUS, Java2Lexer.SUBSUB),
                    Nfa.Rule(Bindings.UnaryExpression)),
                Nfa.Rule(Bindings.UnaryExpressionNotPlusMinus));
        }

        protected virtual Nfa UnaryExpressionNotPlusMinus()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.MatchAny(Java2Lexer.TILDE, Java2Lexer.BANG),
                    Nfa.Rule(Bindings.UnaryExpression)),
                Nfa.Rule(Bindings.CastExpression),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.Primary),
                    Nfa.Closure(Nfa.Rule(Bindings.Selector)),
                    Nfa.Optional(Nfa.MatchAny(Java2Lexer.PLUSPLUS, Java2Lexer.SUBSUB))));
        }

        protected virtual Nfa CastExpression()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LPAREN),
                    Nfa.Rule(Bindings.PrimitiveType),
                    Nfa.Match(Java2Lexer.RPAREN),
                    Nfa.Rule(Bindings.UnaryExpression)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LPAREN),
                    Nfa.Rule(Bindings.Type),
                    Nfa.Match(Java2Lexer.RPAREN),
                    Nfa.Rule(Bindings.UnaryExpressionNotPlusMinus)));
        }

        protected virtual Nfa Primary()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.ParExpression),
                Nfa.Sequence(
                    Nfa.Choice(
                        Nfa.Match(Java2Lexer.THIS),
                        Nfa.Rule(Bindings.SymbolReferenceIdentifier)),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.DOT),
                            Nfa.Rule(Bindings.SymbolReferenceIdentifier))),
                    Nfa.Optional(Nfa.Rule(Bindings.IdentifierSuffix))),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.SUPER),
                    Nfa.Rule(Bindings.SuperSuffix)),
                Nfa.Rule(Bindings.Literal),
                Nfa.Rule(Bindings.Creator),
                Nfa.Sequence(
                    Nfa.Rule(Bindings.PrimitiveType),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Match(Java2Lexer.RBRACKET))),
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Match(Java2Lexer.CLASS)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.VOID),
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Match(Java2Lexer.CLASS)));
        }

        protected virtual Nfa SuperSuffix()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.Arguments),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Optional(Nfa.Rule(Bindings.TypeArguments)),
                    Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                    Nfa.Optional(Nfa.Rule(Bindings.Arguments))));
        }

        protected virtual Nfa IdentifierSuffix()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LBRACKET),
                    Nfa.Match(Java2Lexer.RBRACKET),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Match(Java2Lexer.RBRACKET))),
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Match(Java2Lexer.CLASS)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LBRACKET),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.RBRACKET),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Rule(Bindings.Expression),
                            Nfa.Match(Java2Lexer.RBRACKET)))),
                Nfa.Rule(Bindings.Arguments),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.MatchAny(Java2Lexer.CLASS, Java2Lexer.THIS)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Rule(Bindings.NonWildcardTypeArguments),
                    Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                    Nfa.Rule(Bindings.Arguments)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Match(Java2Lexer.SUPER),
                    Nfa.Rule(Bindings.Arguments)),
                Nfa.Rule(Bindings.InnerCreator));
        }

        protected virtual Nfa Selector()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                    Nfa.Optional(Nfa.Rule(Bindings.Arguments))),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Match(Java2Lexer.THIS)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.DOT),
                    Nfa.Match(Java2Lexer.SUPER),
                    Nfa.Rule(Bindings.SuperSuffix)),
                Nfa.Rule(Bindings.InnerCreator),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.LBRACKET),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.RBRACKET)));
        }

        protected virtual Nfa Creator()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.NEW),
                    Nfa.Rule(Bindings.NonWildcardTypeArguments),
                    Nfa.Rule(Bindings.ClassOrInterfaceType),
                    Nfa.Rule(Bindings.ClassCreatorRest)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.NEW),
                    Nfa.Rule(Bindings.ClassOrInterfaceType),
                    Nfa.Rule(Bindings.ClassCreatorRest)),
                Nfa.Rule(Bindings.ArrayCreator));
        }

        protected virtual Nfa ArrayCreator()
        {
            return Nfa.Choice(
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.NEW),
                    Nfa.Rule(Bindings.CreatedName),
                    Nfa.Match(Java2Lexer.LBRACKET),
                    Nfa.Match(Java2Lexer.RBRACKET),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Match(Java2Lexer.RBRACKET))),
                    Nfa.Rule(Bindings.ArrayInitializer)),
                Nfa.Sequence(
                    Nfa.Match(Java2Lexer.NEW),
                    Nfa.Rule(Bindings.CreatedName),
                    Nfa.Match(Java2Lexer.LBRACKET),
                    Nfa.Rule(Bindings.Expression),
                    Nfa.Match(Java2Lexer.RBRACKET),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Rule(Bindings.Expression),
                            Nfa.Match(Java2Lexer.RBRACKET))),
                    Nfa.Closure(
                        Nfa.Sequence(
                            Nfa.Match(Java2Lexer.LBRACKET),
                            Nfa.Match(Java2Lexer.RBRACKET)))));
        }

        protected virtual Nfa VariableInitializer()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.ArrayInitializer),
                Nfa.Rule(Bindings.Expression));
        }

        protected virtual Nfa ArrayInitializer()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LBRACE),
                Nfa.Optional(
                    Nfa.Sequence(
                        Nfa.Rule(Bindings.VariableInitializer),
                        Nfa.Closure(
                            Nfa.Sequence(
                                Nfa.Match(Java2Lexer.COMMA),
                                Nfa.Rule(Bindings.VariableInitializer))))),
                Nfa.Optional(Nfa.Match(Java2Lexer.COMMA)),
                Nfa.Match(Java2Lexer.RBRACE));
        }

        protected virtual Nfa CreatedName()
        {
            return Nfa.Choice(
                Nfa.Rule(Bindings.ClassOrInterfaceType),
                Nfa.Rule(Bindings.PrimitiveType));
        }

        protected virtual Nfa InnerCreator()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.DOT),
                Nfa.Match(Java2Lexer.NEW),
                Nfa.Optional(Nfa.Rule(Bindings.NonWildcardTypeArguments)),
                Nfa.Rule(Bindings.SymbolReferenceIdentifier),
                Nfa.Optional(Nfa.Rule(Bindings.TypeArguments)),
                Nfa.Rule(Bindings.ClassCreatorRest));
        }

        protected virtual Nfa ClassCreatorRest()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Arguments),
                Nfa.Optional(Nfa.Rule(Bindings.ClassBody)));
        }

        protected virtual Nfa NonWildcardTypeArguments()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LT),
                Nfa.Rule(Bindings.TypeList),
                Nfa.Match(Java2Lexer.GT));
        }

        protected virtual Nfa Arguments()
        {
            return Nfa.Sequence(
                Nfa.Match(Java2Lexer.LPAREN),
                Nfa.Optional(Nfa.Rule(Bindings.ExpressionList)),
                Nfa.Match(Java2Lexer.RPAREN));
        }

        protected virtual Nfa Literal()
        {
            return Nfa.MatchAny(
                Java2Lexer.INTLITERAL,
                Java2Lexer.LONGLITERAL,
                Java2Lexer.FLOATLITERAL,
                Java2Lexer.DOUBLELITERAL,
                Java2Lexer.CHARLITERAL,
                Java2Lexer.STRINGLITERAL,
                Java2Lexer.TRUE,
                Java2Lexer.FALSE,
                Java2Lexer.NULL);
        }

        protected virtual Nfa ClassHeader()
        {
            return Nfa.Sequence(
                Nfa.Rule(Bindings.Modifiers),
                Nfa.Match(Java2Lexer.CLASS),
                Nfa.Rule(Bindings.SymbolDefinitionIdentifier),
                Nfa.Optional(Nfa.Rule(Bindings.TypeParameters)),
                Nfa.Optional(Nfa.Rule(Bindings.NormalClassExtends)),
                Nfa.Optional(Nfa.Rule(Bindings.ImplementsTypeList)));
        }

        public static class RuleNames
        {
            public const string CompilationUnit = "CompilationUnit";
            public const string PackageDeclaration = "PackageDeclaration";
            public const string ImportDeclaration = "ImportDeclaration";
            public const string TypeDeclaration = "TypeDeclaration";
            public const string ClassOrInterfaceDeclaration = "ClassOrInterfaceDeclaration";
            public const string Modifiers = "Modifiers";
            public const string VariableModifiers = "VariableModifiers";
            public const string ClassDeclaration = "ClassDeclaration";
            public const string NormalClassDeclaration = "NormalClassDeclaration";
            public const string NormalClassExtends = "NormalClassExtends";
            public const string ImplementsTypeList = "ImplementsTypeList";
            public const string ExtendsTypeList = "ExtendsTypeList";
            public const string TypeParameters = "TypeParameters";
            public const string TypeParameter = "TypeParameter";
            public const string TypeBound = "TypeBound";
            public const string EnumDeclaration = "EnumDeclaration";
            public const string EnumHeader = "EnumHeader";
            public const string EnumBody = "EnumBody";
            public const string EnumConstants = "EnumConstants";
            public const string EnumConstant = "EnumConstant";
            public const string EnumBodyDeclarations = "EnumBodyDeclarations";
            public const string InterfaceDeclaration = "InterfaceDeclaration";
            public const string NormalInterfaceDeclaration = "NormalInterfaceDeclaration";
            public const string InterfaceHeader = "InterfaceHeader";
            public const string TypeList = "TypeList";
            public const string ClassBody = "ClassBody";
            public const string InterfaceBody = "InterfaceBody";
            public const string ClassBodyDeclaration = "ClassBodyDeclaration";
            public const string MemberDecl = "MemberDecl";
            public const string MethodDeclaration = "MethodDeclaration";
            public const string ConstructorMethodBody = "ConstructorMethodBody";
            public const string MethodBody = "MethodBody";
            public const string ThrowsSpec = "ThrowsSpec";
            public const string FieldDeclaration = "FieldDeclaration";
            public const string VariableDeclarator = "VariableDeclarator";
            public const string InterfaceBodyDeclaration = "InterfaceBodyDeclaration";
            public const string InterfaceMethodDeclaration = "InterfaceMethodDeclaration";
            public const string InterfaceFieldDeclaration = "InterfaceFieldDeclaration";
            public const string Type = "Type";
            public const string ClassOrInterfaceType = "ClassOrInterfaceType";
            public const string GenericIdentifier = "GenericIdentifier";
            public const string PrimitiveType = "PrimitiveType";
            public const string TypeArguments = "TypeArguments";
            public const string TypeArgument = "TypeArgument";
            public const string QualifiedNameList = "QualifiedNameList";
            public const string FormalParameters = "FormalParameters";
            public const string FormalParameterDecls = "FormalParameterDecls";
            public const string NormalParameterDecl = "NormalParameterDecl";
            public const string EllipsisParameterDecl = "EllipsisParameterDecl";
            public const string ExplicitConstructorInvocation = "ExplicitConstructorInvocation";
            public const string QualifiedName = "QualifiedName";
            public const string Annotations = "Annotations";
            public const string Annotation = "Annotation";
            public const string ElementValuePairs = "ElementValuePairs";
            public const string ElementValuePair = "ElementValuePair";
            public const string ElementValue = "ElementValue";
            public const string ElementValueArrayInitializer = "ElementValueArrayInitializer";
            public const string AnnotationTypeDeclaration = "AnnotationTypeDeclaration";
            public const string AnnotationInterfaceHeader = "AnnotationInterfaceHeader";
            public const string AnnotationTypeBody = "AnnotationTypeBody";
            public const string AnnotationTypeElementDeclaration = "AnnotationTypeElementDeclaration";
            public const string AnnotationMethodDeclaration = "AnnotationMethodDeclaration";
            public const string Block = "Block";
            public const string StaticBlock = "StaticBlock";
            public const string BlockStatement = "BlockStatement";
            public const string LocalVariableDeclarationStatement = "LocalVariableDeclarationStatement";
            public const string LocalVariableDeclaration = "LocalVariableDeclaration";
            public const string Statement = "Statement";
            public const string SwitchBlockStatementGroups = "SwitchBlockStatementGroups";
            public const string SwitchBlockStatementGroup = "SwitchBlockStatementGroup";
            public const string SwitchLabel = "SwitchLabel";
            public const string TryStatement = "TryStatement";
            public const string FinallyBlock = "FinallyBlock";
            public const string Catches = "Catches";
            public const string CatchClause = "CatchClause";
            public const string FormalParameter = "FormalParameter";
            public const string ForStatement = "ForStatement";
            public const string ForInit = "ForInit";
            public const string ParExpression = "ParExpression";
            public const string ExpressionList = "ExpressionList";
            public const string Expression = "Expression";
            public const string SymbolDefinitionIdentifier = "SymbolDefinitionIdentifier";
            public const string SymbolReferenceIdentifier = "SymbolReferenceIdentifier";

            public const string AssignmentOperator = "AssignmentOperator";
            public const string ConditionalExpression = "ConditionalExpression";
            public const string ConditionalOrExpression = "ConditionalOrExpression";
            public const string ConditionalAndExpression = "ConditionalAndExpression";
            public const string InclusiveOrExpression = "InclusiveOrExpression";
            public const string ExclusiveOrExpression = "ExclusiveOrExpression";
            public const string AndExpression = "AndExpression";
            public const string EqualityExpression = "EqualityExpression";
            public const string InstanceOfExpression = "InstanceOfExpression";
            public const string RelationalExpression = "RelationalExpression";
            public const string RelationalOp = "RelationalOp";
            public const string ShiftExpression = "ShiftExpression";
            public const string ShiftOp = "ShiftOp";
            public const string AdditiveExpression = "AdditiveExpression";
            public const string MultiplicativeExpression = "MultiplicativeExpression";
            public const string UnaryExpression = "UnaryExpression";
            public const string UnaryExpressionNotPlusMinus = "UnaryExpressionNotPlusMinus";
            public const string CastExpression = "CastExpression";

            public const string Primary = "Primary";
            public const string SuperSuffix = "SuperSuffix";
            public const string IdentifierSuffix = "IdentifierSuffix";
            public const string Selector = "Selector";
            public const string Creator = "Creator";
            public const string ArrayCreator = "ArrayCreator";
            public const string VariableInitializer = "VariableInitializer";
            public const string ArrayInitializer = "ArrayInitializer";
            public const string CreatedName = "CreatedName";
            public const string InnerCreator = "InnerCreator";
            public const string ClassCreatorRest = "ClassCreatorRest";
            public const string NonWildcardTypeArguments = "NonWildcardTypeArguments";
            public const string Arguments = "Arguments";
            public const string Literal = "Literal";

            public const string ClassHeader = "ClassHeader";
        }

        protected class RuleBindings
        {
            public readonly RuleBinding CompilationUnit = new RuleBinding(RuleNames.CompilationUnit);
            public readonly RuleBinding PackageDeclaration = new RuleBinding(RuleNames.PackageDeclaration);
            public readonly RuleBinding ImportDeclaration = new RuleBinding(RuleNames.ImportDeclaration);
            public readonly RuleBinding TypeDeclaration = new RuleBinding(RuleNames.TypeDeclaration);
            public readonly RuleBinding ClassOrInterfaceDeclaration = new RuleBinding(RuleNames.ClassOrInterfaceDeclaration);
            public readonly RuleBinding Modifiers = new RuleBinding(RuleNames.Modifiers);
            public readonly RuleBinding VariableModifiers = new RuleBinding(RuleNames.VariableModifiers);
            public readonly RuleBinding ClassDeclaration = new RuleBinding(RuleNames.ClassDeclaration);
            public readonly RuleBinding NormalClassDeclaration = new RuleBinding(RuleNames.NormalClassDeclaration);
            public readonly RuleBinding NormalClassExtends = new RuleBinding(RuleNames.NormalClassExtends);
            public readonly RuleBinding ImplementsTypeList = new RuleBinding(RuleNames.ImplementsTypeList);
            public readonly RuleBinding ExtendsTypeList = new RuleBinding(RuleNames.ExtendsTypeList);
            public readonly RuleBinding TypeParameters = new RuleBinding(RuleNames.TypeParameters);
            public readonly RuleBinding TypeParameter = new RuleBinding(RuleNames.TypeParameter);
            public readonly RuleBinding TypeBound = new RuleBinding(RuleNames.TypeBound);
            public readonly RuleBinding EnumDeclaration = new RuleBinding(RuleNames.EnumDeclaration);
            public readonly RuleBinding EnumHeader = new RuleBinding(RuleNames.EnumHeader);
            public readonly RuleBinding EnumBody = new RuleBinding(RuleNames.EnumBody);
            public readonly RuleBinding EnumConstants = new RuleBinding(RuleNames.EnumConstants);
            public readonly RuleBinding EnumConstant = new RuleBinding(RuleNames.EnumConstant);
            public readonly RuleBinding EnumBodyDeclarations = new RuleBinding(RuleNames.EnumBodyDeclarations);
            public readonly RuleBinding InterfaceDeclaration = new RuleBinding(RuleNames.InterfaceDeclaration);
            public readonly RuleBinding NormalInterfaceDeclaration = new RuleBinding(RuleNames.NormalInterfaceDeclaration);
            public readonly RuleBinding InterfaceHeader = new RuleBinding(RuleNames.InterfaceHeader);
            public readonly RuleBinding TypeList = new RuleBinding(RuleNames.TypeList);
            public readonly RuleBinding ClassBody = new RuleBinding(RuleNames.ClassBody);
            public readonly RuleBinding InterfaceBody = new RuleBinding(RuleNames.InterfaceBody);
            public readonly RuleBinding ClassBodyDeclaration = new RuleBinding(RuleNames.ClassBodyDeclaration);
            public readonly RuleBinding MemberDecl = new RuleBinding(RuleNames.MemberDecl);
            public readonly RuleBinding MethodDeclaration = new RuleBinding(RuleNames.MethodDeclaration);
            public readonly RuleBinding ConstructorMethodBody = new RuleBinding(RuleNames.ConstructorMethodBody);
            public readonly RuleBinding MethodBody = new RuleBinding(RuleNames.MethodBody);
            public readonly RuleBinding ThrowsSpec = new RuleBinding(RuleNames.ThrowsSpec);
            public readonly RuleBinding FieldDeclaration = new RuleBinding(RuleNames.FieldDeclaration);
            public readonly RuleBinding VariableDeclarator = new RuleBinding(RuleNames.VariableDeclarator);
            public readonly RuleBinding InterfaceBodyDeclaration = new RuleBinding(RuleNames.InterfaceBodyDeclaration);
            public readonly RuleBinding InterfaceMethodDeclaration = new RuleBinding(RuleNames.InterfaceMethodDeclaration);
            public readonly RuleBinding InterfaceFieldDeclaration = new RuleBinding(RuleNames.InterfaceFieldDeclaration);
            public readonly RuleBinding Type = new RuleBinding(RuleNames.Type);
            public readonly RuleBinding ClassOrInterfaceType = new RuleBinding(RuleNames.ClassOrInterfaceType);
            public readonly RuleBinding GenericIdentifier = new RuleBinding(RuleNames.GenericIdentifier);
            public readonly RuleBinding PrimitiveType = new RuleBinding(RuleNames.PrimitiveType);
            public readonly RuleBinding TypeArguments = new RuleBinding(RuleNames.TypeArguments);
            public readonly RuleBinding TypeArgument = new RuleBinding(RuleNames.TypeArgument);
            public readonly RuleBinding QualifiedNameList = new RuleBinding(RuleNames.QualifiedNameList);
            public readonly RuleBinding FormalParameters = new RuleBinding(RuleNames.FormalParameters);
            public readonly RuleBinding FormalParameterDecls = new RuleBinding(RuleNames.FormalParameterDecls);
            public readonly RuleBinding NormalParameterDecl = new RuleBinding(RuleNames.NormalParameterDecl);
            public readonly RuleBinding EllipsisParameterDecl = new RuleBinding(RuleNames.EllipsisParameterDecl);
            public readonly RuleBinding ExplicitConstructorInvocation = new RuleBinding(RuleNames.ExplicitConstructorInvocation);
            public readonly RuleBinding QualifiedName = new RuleBinding(RuleNames.QualifiedName);
            public readonly RuleBinding Annotations = new RuleBinding(RuleNames.Annotations);
            public readonly RuleBinding Annotation = new RuleBinding(RuleNames.Annotation);
            public readonly RuleBinding ElementValuePairs = new RuleBinding(RuleNames.ElementValuePairs);
            public readonly RuleBinding ElementValuePair = new RuleBinding(RuleNames.ElementValuePair);
            public readonly RuleBinding ElementValue = new RuleBinding(RuleNames.ElementValue);
            public readonly RuleBinding ElementValueArrayInitializer = new RuleBinding(RuleNames.ElementValueArrayInitializer);
            public readonly RuleBinding AnnotationTypeDeclaration = new RuleBinding(RuleNames.AnnotationTypeDeclaration);
            public readonly RuleBinding AnnotationInterfaceHeader = new RuleBinding(RuleNames.AnnotationInterfaceHeader);
            public readonly RuleBinding AnnotationTypeBody = new RuleBinding(RuleNames.AnnotationTypeBody);
            public readonly RuleBinding AnnotationTypeElementDeclaration = new RuleBinding(RuleNames.AnnotationTypeElementDeclaration);
            public readonly RuleBinding AnnotationMethodDeclaration = new RuleBinding(RuleNames.AnnotationMethodDeclaration);
            public readonly RuleBinding Block = new RuleBinding(RuleNames.Block);
            public readonly RuleBinding StaticBlock = new RuleBinding(RuleNames.StaticBlock);
            public readonly RuleBinding BlockStatement = new RuleBinding(RuleNames.BlockStatement);
            public readonly RuleBinding LocalVariableDeclarationStatement = new RuleBinding(RuleNames.LocalVariableDeclarationStatement);
            public readonly RuleBinding LocalVariableDeclaration = new RuleBinding(RuleNames.LocalVariableDeclaration);
            public readonly RuleBinding Statement = new RuleBinding(RuleNames.Statement);
            public readonly RuleBinding SwitchBlockStatementGroups = new RuleBinding(RuleNames.SwitchBlockStatementGroups);
            public readonly RuleBinding SwitchBlockStatementGroup = new RuleBinding(RuleNames.SwitchBlockStatementGroup);
            public readonly RuleBinding SwitchLabel = new RuleBinding(RuleNames.SwitchLabel);
            public readonly RuleBinding TryStatement = new RuleBinding(RuleNames.TryStatement);
            public readonly RuleBinding FinallyBlock = new RuleBinding(RuleNames.FinallyBlock);
            public readonly RuleBinding Catches = new RuleBinding(RuleNames.Catches);
            public readonly RuleBinding CatchClause = new RuleBinding(RuleNames.CatchClause);
            public readonly RuleBinding FormalParameter = new RuleBinding(RuleNames.FormalParameter);
            public readonly RuleBinding ForStatement = new RuleBinding(RuleNames.ForStatement);
            public readonly RuleBinding ForInit = new RuleBinding(RuleNames.ForInit);
            public readonly RuleBinding ParExpression = new RuleBinding(RuleNames.ParExpression);
            public readonly RuleBinding ExpressionList = new RuleBinding(RuleNames.ExpressionList);
            public readonly RuleBinding Expression = new RuleBinding(RuleNames.Expression);
            public readonly RuleBinding SymbolDefinitionIdentifier = new RuleBinding(RuleNames.SymbolDefinitionIdentifier);
            public readonly RuleBinding SymbolReferenceIdentifier = new RuleBinding(RuleNames.SymbolReferenceIdentifier);

            public readonly RuleBinding AssignmentOperator = new RuleBinding(RuleNames.AssignmentOperator);
            public readonly RuleBinding ConditionalExpression = new RuleBinding(RuleNames.ConditionalExpression);
            public readonly RuleBinding ConditionalOrExpression = new RuleBinding(RuleNames.ConditionalOrExpression);
            public readonly RuleBinding ConditionalAndExpression = new RuleBinding(RuleNames.ConditionalAndExpression);
            public readonly RuleBinding InclusiveOrExpression = new RuleBinding(RuleNames.InclusiveOrExpression);
            public readonly RuleBinding ExclusiveOrExpression = new RuleBinding(RuleNames.ExclusiveOrExpression);
            public readonly RuleBinding AndExpression = new RuleBinding(RuleNames.AndExpression);
            public readonly RuleBinding EqualityExpression = new RuleBinding(RuleNames.EqualityExpression);
            public readonly RuleBinding InstanceOfExpression = new RuleBinding(RuleNames.InstanceOfExpression);
            public readonly RuleBinding RelationalExpression = new RuleBinding(RuleNames.RelationalExpression);
            public readonly RuleBinding RelationalOp = new RuleBinding(RuleNames.RelationalOp);
            public readonly RuleBinding ShiftExpression = new RuleBinding(RuleNames.ShiftExpression);
            public readonly RuleBinding ShiftOp = new RuleBinding(RuleNames.ShiftOp);
            public readonly RuleBinding AdditiveExpression = new RuleBinding(RuleNames.AdditiveExpression);
            public readonly RuleBinding MultiplicativeExpression = new RuleBinding(RuleNames.MultiplicativeExpression);
            public readonly RuleBinding UnaryExpression = new RuleBinding(RuleNames.UnaryExpression);
            public readonly RuleBinding UnaryExpressionNotPlusMinus = new RuleBinding(RuleNames.UnaryExpressionNotPlusMinus);
            public readonly RuleBinding CastExpression = new RuleBinding(RuleNames.CastExpression);

            public readonly RuleBinding Primary = new RuleBinding(RuleNames.Primary);
            public readonly RuleBinding SuperSuffix = new RuleBinding(RuleNames.SuperSuffix);
            public readonly RuleBinding IdentifierSuffix = new RuleBinding(RuleNames.IdentifierSuffix);
            public readonly RuleBinding Selector = new RuleBinding(RuleNames.Selector);
            public readonly RuleBinding Creator = new RuleBinding(RuleNames.Creator);
            public readonly RuleBinding ArrayCreator = new RuleBinding(RuleNames.ArrayCreator);
            public readonly RuleBinding VariableInitializer = new RuleBinding(RuleNames.VariableInitializer);
            public readonly RuleBinding ArrayInitializer = new RuleBinding(RuleNames.ArrayInitializer);
            public readonly RuleBinding CreatedName = new RuleBinding(RuleNames.CreatedName);
            public readonly RuleBinding InnerCreator = new RuleBinding(RuleNames.InnerCreator);
            public readonly RuleBinding ClassCreatorRest = new RuleBinding(RuleNames.ClassCreatorRest);
            public readonly RuleBinding NonWildcardTypeArguments = new RuleBinding(RuleNames.NonWildcardTypeArguments);
            public readonly RuleBinding Arguments = new RuleBinding(RuleNames.Arguments);
            public readonly RuleBinding Literal = new RuleBinding(RuleNames.Literal);

            public readonly RuleBinding ClassHeader = new RuleBinding(RuleNames.ClassHeader);
        }
    }
}
