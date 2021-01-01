﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScratchNet
{
    public class TimedMoveStatement : Statement
    {
        public TimedMoveStatement()
        {
            Time = new Literal() { Raw = "2" };
        }
        public Expression Time { get; set; }
        public Expression X { get; set; }
        public Expression Y { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DateTime startTime;
            DateTime finishTime;
            double orgin_x;
            double orgin_y;
            double totalTime;
            if (X == null)
                return Completion.Void;
            double x = 0;
            try
            {
                var c = X.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                x = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", X);
            }
            if (Y == null)
                return Completion.Void;
            double y = 0;
            try
            {
                var c = Y.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                y = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", Y);
            }
            if (Time == null)
                return Completion.Void;
            double time = 0;
            try
            {
                var c = Time.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                time = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", Time);
            }

            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;

            if (x < 0)
                x = 0;
            if (x > CurrentEnviroment.ScreenWidth)
                x = CurrentEnviroment.ScreenWidth;
            if (y < 0)
                y = 0;
            if (y > CurrentEnviroment.ScreenHeight)
                y = CurrentEnviroment.ScreenHeight;
            orgin_x = sp.X;
            orgin_y = sp.Y;
            startTime = DateTime.Now;
            finishTime = DateTime.Now.AddSeconds(time);
            totalTime = (finishTime - startTime).TotalMilliseconds;

            while (true)
            {
                if (DateTime.Now > finishTime)
                    break;
                double duration = (DateTime.Now - startTime).TotalMilliseconds;

                sp.X = (int)(orgin_x + (x - orgin_x) * duration / totalTime);
                sp.Y = (int)(orgin_y + (y - orgin_y) * duration / totalTime);
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (enviroment.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
                Thread.Sleep(100);
            }

            sp.X = (int)x;
            sp.Y = (int)y;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (enviroment.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return Completion.Void;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_In")));
                desc.Add(new ExpressionDescriptor(this, "Time", "number"));
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_MoveXinSec")));
                desc.Add(new ExpressionDescriptor(this, "X", "number"));
                desc.Add(new TextItemDescriptor(this, ", Y "));
                desc.Add(new ExpressionDescriptor(this, "Y", "number"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "MoveStatement";
            }
        }


        public override BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public override bool IsClosing
        {
            get { return false; }
        }
    }
}
