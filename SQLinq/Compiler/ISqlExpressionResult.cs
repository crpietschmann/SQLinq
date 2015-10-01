//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;

namespace SQLinq.Compiler
{
    public interface ISqlExpressionResult
    {
        IDictionary<string, object> Parameters { get; set; }
    }
}
