using System;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// TestDicForeach = TestDicEnumeratorUsing 每次会有一个48b的GC alloc
/// 生成try finally代码里会有一个boxing装箱的过程
/// </summary>
public class ATest_Dictionary : ATest_Base
{
    const int MAX_NUM = 1;
    Dictionary<int, string> testDic = new Dictionary<int, string>();

    public override void Test()
    {
        for (int i = 0; i < MAX_NUM; i++)
        {
            string key = i.ToString();
            testDic.Add(i, key);
        }
    }

    public override void UpdateEx()
    {
        base.UpdateEx();

        TestDicForeach();
        TestDicEnumerator();
        TestDicEnumeratorUsing();
    }

    void TestDicForeach()
    {
        foreach (var item in testDic)
        {
            var key = item.Key;
            var value = item.Value;
        }
    }

    void TestDicEnumerator()
    {
        var enumerator = testDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var key = enumerator.Current.Key;
            var value = enumerator.Current.Value;
        }
        enumerator.Dispose();
    }

    void TestDicEnumeratorUsing()
    {
        using (var enumerator = testDic.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                var key = enumerator.Current.Key;
                var value = enumerator.Current.Value;
            }
        }
    }
}