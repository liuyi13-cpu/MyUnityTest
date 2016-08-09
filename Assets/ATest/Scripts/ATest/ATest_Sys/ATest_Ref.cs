using UnityEngine;
using System.Collections;
using System.Text;
using System;

namespace Gc.Game.Test
{
    public class ATest_Ref : ATest_Base
    {
        // 引用测试
        // ref out 把参数值类型变成应用类型
        // 基础类型，string，struct需要ref out
        // class不需要
        public override void Test()
        {
            int a = 0;
            int b = 0;
            int c;
            string str = "0";
            string str1 = "0";
            TestRefInt(a, ref b, out c, str, ref str1);
            Debugger.Log("a:" + a + " b:" + b + " c:" + c + " str:" + str + " str1:" + str1);

            int[] array = { 0, 1 };
            TestRefArray(array);
            int[] array1 = new int[10];
            TestRefArray(array1);

            Vector2 vec;
            vec.x = 1;
            vec.y = 1;
            Vector2 vec1 = new Vector2(2, 2);
            TestRefVec(vec, ref vec1);
            Debugger.Log(vec);
            Debugger.Log(vec1);

            MyStruct struct1 = new MyStruct();
            MyStruct struct2 = new MyStruct();
            TestRefVec(struct1, ref struct2);

            MyClass class1 = new MyClass();
            MyClass class2 = new MyClass();
            TestRefClass(class1, ref class2);
        }

        void TestRefInt(int a, ref int b, out int c, string str, ref string str1)
        {
            a = 10;
            b = 10;
            c = 10;
            str = "10";
            str1 = str1 + "10";
            // stra = "2"; 其实相当于 stra = new String('2');
        }

        void TestRefArray(params int[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = 10;
            }
        }

        void TestRefVec(Vector2 vec, ref Vector2 vec1)
        {
            vec.x = 10;
            vec1.x = 10;
        }

        // class是应用类型 默认继承object
        class MyClass
        {
            public int structObj_int;
        }

        // struct是值类型
        struct MyStruct
        {
            public int struct_int;
            public MyClass struct_obj;
        }

        void TestRefVec(MyStruct struct1, ref MyStruct struct2)
        {
            struct1.struct_int = 10;
            MyClass class1 = new MyClass();
            class1.structObj_int = 10;
            struct1.struct_obj = class1;

            struct2.struct_int = 20;
            struct2.struct_obj = new MyClass();
            struct2.struct_obj.structObj_int = 20;
        }

        void TestRefClass(MyClass struct1, ref MyClass struct2)
        {
            struct1.structObj_int = 10;
            struct2.structObj_int = 20;
        }
    }
}