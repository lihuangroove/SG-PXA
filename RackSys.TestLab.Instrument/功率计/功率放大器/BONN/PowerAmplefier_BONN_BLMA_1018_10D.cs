﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RackSys.TestLab.Instrument
{
    public class PowerAmplefier_BONN_BLMA_1018_10D: PowerAmplefierBase
    {
        public PowerAmplefier_BONN_BLMA_1018_10D(string inAddress)
            : base(inAddress)
        {
            
        }

        /// <summary>
        /// 打开、关闭功放的射频输出
        /// </summary>
        /// <param name="isOn"></param>
        public override void RFOnOff(bool isOn)
        {   string ONOff=isOn? "AMP_ON":"AMP_OFF";
            this.Send(ONOff);

            System.Threading.Thread.Sleep(500);
            string status=this.Query("AMP?");


            System.Threading.Thread.Sleep(500);
            if (status != ONOff)
            {
                throw new Exception("BONN功放处于锁定状态，请手动将RF输出关闭");

            }
            
        }

        /// <summary>
        /// 查询RF是否开启
        /// </summary>
        /// <param name="isOn"></param>
        //public override string QueryRFStatus()
        //{
        //    return this.Query("AMP?");            
        //}

        public override bool RFOutputEnabled
        {
            get
            {
                return this.Query("AMP?") == "AMP_ON";
            }
        }


        /// <summary>
        /// 设置功放工作的频带
        /// band1:1~2G
        /// band2:2~6G
        /// band3:6~18G
        /// </summary>
        /// <param name="bandId"></param>
        public override void SetBand(int bandId)
        {
            switch (bandId)
            {
                case 1: this.Send("SW01_1"); System.Threading.Thread.Sleep(500); break;
                case 2: this.Send("SW01_2"); System.Threading.Thread.Sleep(500); break;
                case 3: this.Send("SW01_3"); System.Threading.Thread.Sleep(500); break;
                default: throw new Exception("错误的频带设置");
            
            }
        }

        public override int GetBands()
        {
            return 3;
        }

        public override void WaitOpc()
        {
            Thread.Sleep(200);
        }

        public override double GetBandStartById(int bandId)
        {
            switch (bandId)
            {
                case 1: return 1e9;
                case 2: return 2e9;
                case 3: return 6e9; 
                default: throw new Exception("错误的频带设置");

            }
        }

        public override double GetBandEndById(int bandId)
        {
            switch (bandId)
            {
                case 1: return 2e9;
                case 2: return 6e9;
                case 3: return 18e9;
                default: throw new Exception("错误的频带设置");

            }
        }

        public override int GetBandByFre(double Freq)
        {
            int tempBand = -1;
            for (int i = 0; i < this.GetBands(); i++)
            {
                if (Freq >= GetBandStartById(i + 1) && Freq <= GetBandEndById(i + 1))
                {
                    tempBand = i + 1;
                    break;
                }
            }
                // throw new NotImplementedException();
                return tempBand;
        }
    }
}
