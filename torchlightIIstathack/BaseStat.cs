using System;
using System.Collections.Generic;
using System.Text;
namespace torchlightIIstathack
{
    public class BaseStat
    {
        int Offset;
        public int Value
        {
            get;
            set;
        }
        string Description;

        public BaseStat(int Offset, string Description)
        {
            this.Offset = Offset;
            this.Description = Description;
        }

        public void GetValue()
        {

        }
    }
}
