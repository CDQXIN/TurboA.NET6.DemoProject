﻿using Zhaoxi.NET6.Interface;
using System;

namespace Zhaoxi.NET6.Service
{
    public class TestServiceC : ITestServiceC
    {
        public TestServiceC(ITestServiceB iTestServiceB)
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }
        public void Show()
        {
            Console.WriteLine("C123456");
        }
    }
}
