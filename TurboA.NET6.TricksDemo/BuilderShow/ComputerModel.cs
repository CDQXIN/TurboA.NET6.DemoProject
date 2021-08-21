using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.NET6.TricksDemo.BuilderShow
{
    public class Computer
    {
        private string CPU;//必须
        private string RAM;//必须
        private int USBCount;//可选
        private string Keyboard;//可选
        private string Display;//可选

        #region 多构造函数
        public Computer(string cpu, string ram) : this(cpu, ram, 0)
        {

        }
        public Computer(string cpu, string ram, int usbCount) : this(cpu, ram, usbCount, "罗技键盘")
        {

        }
        public Computer(string cpu, string ram, int usbCount, string keyboard) : this(cpu, ram, usbCount, keyboard, "三星显示器")
        {

        }
        public Computer(string cpu, string ram, int usbCount, string keyboard, string display)
        {
            this.CPU = cpu;
            this.RAM = ram;
            this.USBCount = usbCount;
            this.Keyboard = keyboard;
            this.Display = display;
        }
        #endregion

        #region 多初始化方法
        public void SetCPU(string cpu)
        {
            this.CPU = cpu;
        }
        public void SetRAM(string ram)
        {
            this.RAM = ram;
        }
        public void SetUSBCount(int usbCount)
        {
            this.USBCount = usbCount;
        }
        public void SetKeyboard(string keyboard)
        {
            this.Keyboard = keyboard;
        }
        public void SetDisplay(string display)
        {
            this.Display = display;
        }
        #endregion

        #region 建造者
        public Computer(ComputerBuilder builder)
        {
            this.CPU = builder.CPU;
            this.RAM = builder.RAM;
            this.USBCount = builder.USBCount;
            this.Keyboard = builder.Keyboard;
            this.Display = builder.Display;
        }
        #endregion
    }

    public class ComputerBuilder
    {
        internal string CPU;//必须
        internal string RAM;//必须
        internal int USBCount;//可选
        internal string Keyboard;//可选
        internal string Display;//可选

        public ComputerBuilder(string cpu, string ram)
        {
            this.CPU = cpu;
            this.RAM = ram;
        }

        public ComputerBuilder SetUSBCount(int usbCount)
        {
            this.USBCount = usbCount;
            return this;
        }
        public ComputerBuilder SetKeyboard(string keyboard)
        {
            this.Keyboard = keyboard;
            return this;
        }
        public ComputerBuilder SetDisplay(string display)
        {
            this.Display = display;
            return this;
        }
        public Computer Build()
        {
            //这里还可以很麻烦
            return new Computer(this);
        }
    }
}
