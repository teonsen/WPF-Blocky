﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class BinaryTreeLeftExpression : Expression, IAssignment
    {
        public Expression Node { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Node", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".left"));
                return desc;
            }
        }

        public override string Type => "BinaryTreeLeftNode";

        public Completion Assign(ExecutionEnvironment enviroment, object value)
        {
            if (value != null && !(value is BinaryTree))
                return Completion.Exception("value is not a binary tree type", this);
            if (Node == null)
                return Completion.Exception("Null Exception", this);
            if ((Node is Identifier))
            {
                string var = (Node as Identifier).Variable;
                if (!enviroment.HasValue(var))
                {
                    return Completion.Exception("varaible was not defined", Node);
                }
                BinaryTree tree = enviroment.GetValue(var) as BinaryTree;
                if (tree == null)
                {
                    return Completion.Exception("variable is not binary tree", Node);
                }
                tree.Left = value as BinaryTree;
                return new Completion(value);
            }
            else
            {
                var c = Node.Execute(enviroment);
                if (!c.IsValue)
                    return c;
                if (c.ReturnValue is BinaryTree)
                {
                    (c.ReturnValue as BinaryTree).Left = value as BinaryTree;
                    return new Completion(value);
                }
                else
                    return Completion.Exception("value is not binary tree", Node);
            }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Node == null)
                return Completion.Exception("Null Exception", this);
            if ((Node is Identifier))
            {
                string var = (Node as Identifier).Variable;
                if (!enviroment.HasValue(var))
                {
                    return Completion.Exception("varaible was not defined", Node);
                }
                BinaryTree tree = enviroment.GetValue(var) as BinaryTree;
                if (tree == null)
                {
                    return Completion.Exception("variable is not binary tree", Node);
                }
                return new Completion(tree.Left);
            }
            else
            {
                var c = Node.Execute(enviroment);
                if (!c.IsValue)
                    return c;
                if (c.ReturnValue is BinaryTree)
                {
                    return new Completion((c.ReturnValue as BinaryTree).Left);
                }
                else
                    return Completion.Exception("value is not binary tree", Node);
            }
        }
    }
}
