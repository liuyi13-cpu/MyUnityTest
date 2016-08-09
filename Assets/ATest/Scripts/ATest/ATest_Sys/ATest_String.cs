using System;
using System.Text;

namespace Gc.Game.Test
{
    public class ATest_String : ATest_Base
    {
        const int MAX_NUM = 100000;
        SpeedString velocity_string = new SpeedString(MAX_NUM*2);
#pragma warning disable 414
        readonly int aaa = 10;
#pragma warning restore 414
        public override void Test()
        {
//             MAX_NUM = 11;    /// ERR
//             aaa = 11;        /// ERR

            // SpeedString
            velocity_string.Clear();
            DateTime dt = DateTime.Now;
            for (int i = 0; i < MAX_NUM; i++)
            {
                velocity_string.Append(1);
                // velocity_string.Append("M");
            }

            double time = (DateTime.Now - dt).TotalSeconds;
            Debugger.Log("SpeedString:" + time);
//             velocity_string.Clear();
//             velocity_string.Append("200");
// 
//             Debugger.Log(velocity_string.ToString());

            // StringBuilder
            StringBuilder sb = new StringBuilder(MAX_NUM*2);

            dt = DateTime.Now;

            for (int i = 0; i < MAX_NUM; i++)
            {
                sb.Append(1);
                // sb.Append("M");
            }

            time = (DateTime.Now - dt).TotalSeconds;
            dt = DateTime.Now;
            Debugger.Log("StringBuilder:" + time);

            // string
            string str = "";
            for (int i = 0; i < MAX_NUM; i++)
            {
                str += 1;
                // str += "M";
            }

            time = (DateTime.Now - dt).TotalSeconds;
            dt = DateTime.Now;
            Debugger.Log("string:" + time);
        }
    }
}