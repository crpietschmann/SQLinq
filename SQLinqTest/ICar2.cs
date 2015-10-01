//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq;

namespace SQLinqTest
{
    [SQLinqTable("Car")]
    public interface ICar2
    {
        [SQLinqColumn("Wheel_Diameter")]
        int WheelDiameter { get; set; }
    }
}
