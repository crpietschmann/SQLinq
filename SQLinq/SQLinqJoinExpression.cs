////Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
////Licensed under the GNU Library General Public License (LGPL)
////License can be found here: http://sqlinq.codeplex.com/license

//using System;
//using System.Linq.Expressions;
//using System.Collections.Generic;

//namespace SQLinq
//{
//    public interface ISQLinqJoinExpression
//    {
//        string Process(Dictionary<string, object> parameters);

//        Expression OuterKeySelector { get; }
//        Expression InnerKeySelector { get; }
//        Expression ResultSelector { get; }
//    }

//    public class SQLinqJoinExpression<TOuter, TInner, TKey, TResult> : ISQLinqJoinExpression
//    {
//        public SQLinq<TOuter> Outer { get; set; }
//        public SQLinq<TInner> Inner { get; set; }
//        public Expression<Func<TOuter, TKey>> OuterKeySelector { get; set; }
//        public Expression<Func<TInner, TKey>> InnerKeySelector { get; set; }
//        public Expression<Func<TOuter, TInner, TResult>> ResultSelector { get; set; }

//        public string Process(Dictionary<string, object> parameters)
//        {
//            var innerTable = this.Inner.GetTableName(true);
//            var inner = this.Inner.ProcessExpression(this.InnerKeySelector, parameters);
//            var outer = this.Outer.ProcessExpression(this.OuterKeySelector, parameters);

//            return string.Format("JOIN {0} ON {2} = {1}", innerTable, inner, outer);
//        }

//        Expression ISQLinqJoinExpression.OuterKeySelector
//        {
//            get { return this.OuterKeySelector; }
//        }

//        Expression ISQLinqJoinExpression.InnerKeySelector
//        {
//            get { return this.InnerKeySelector; }
//        }

//        Expression ISQLinqJoinExpression.ResultSelector
//        {
//            get { return this.ResultSelector; }
//        }
//    }
//}
