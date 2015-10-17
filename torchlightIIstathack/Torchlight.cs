using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace torchlightIIstathack
{
    public class Torchlight : BaseCheatable
    {
        private const string BASEADDRESS = "274666C"; //Hex
        private const int OFFSET1 = 0x2C;//Offset for stat data structure
        private enum STAT_OFFSETS //Enum for stat offsets
        {
            //Four integers (16bytes)
            Dexterity = 0x56c,
            Strength = 0x570,
            Vitality = 0x574,
            Focus = 0x578
        }

        public int Dexterity
        {
            get;
            set;
        }
        public int Strength
        {
            get;
            set;
        }
        public int Vitality
        {
            get;
            set;
        }
        public int Focus
        {
            get;
            set;
        }

        private enum PLAYER_OFFSETS
        {
            SkillPoints = 0x5A4,
            PlayerLevel = 0x110
        }

        public int PlayerLevel
        {
            get;
            set;
        }

        private enum SKILL_OFFSETS
        {
            SkillPoints = 0x5A4
        }

        public int SkillPoints
        {
            get;
            set;
        }

        public Torchlight() : base("Torchlight2")
        {
            FetchStats();
            FetchSkills();
        }

        public Torchlight(string ProcessName) : base(ProcessName)
        {
            FetchStats();
            FetchSkills();
        }

        private void FetchStats()//Needs a reahaul to cater for greater flexibility
        {
            int BytesRead;
            byte[] temp;
            int[] Values = new int[4];

            Memory memory = new Memory();
            memory.ReadProcess = this.TargetProcess;

            memory.Open();
            int BaseAddr = Addr.ToDec(Torchlight.BASEADDRESS); //The static address of the pointer (#1)
            int[] Offsets = { OFFSET1, (int)STAT_OFFSETS.Dexterity }; //Offsets from bottom to top (#2) [starting with bottom of struct]
            
            for (int i = 0; i < 4; i++)//Loop through and read 4 values
            {
                temp = memory.PointerRead((IntPtr)BaseAddr, (uint)16, Offsets, out BytesRead);
                if (BytesRead > 0)
                {
                    Values[i] = BitConverter.ToInt32(temp, 0);
                    Offsets[1] += 4; //Increment STAT_OFFSET by 4bytes
                }
            }
            memory.CloseHandle();

            this.Dexterity = Values[0];
            this.Strength = Values[1];
            this.Focus = Values[2];
            this.Vitality = Values[3];

            //Lazy addition to new variable "Player Level"
            int read;
            this.PlayerLevel = this.ReadInt((int)PLAYER_OFFSETS.PlayerLevel, out read) ? read : -1;           
        }

        private void FetchSkills()
        {
            //Read and then assign skillpoint value, -1 if failed
            int read;
            this.SkillPoints = ReadInt((int)SKILL_OFFSETS.SkillPoints, out read) ? read : -1;
        }
        private bool WriteValue(int value, int offset)
        {
            int BytesWritten;
            byte[] bValue = BitConverter.GetBytes(value);

            Memory memory = new Memory();
            memory.ReadProcess = this.TargetProcess;
            memory.Open();
            int BaseAddr = Addr.ToDec(Torchlight.BASEADDRESS);
            int[] Offsets = { OFFSET1, (int)offset};

            //Write value to pointer address
            string Written_Address = memory.PointerWrite((IntPtr)BaseAddr, 
                                  bValue, //The value to write (as bytes)
                                  Offsets, //Our offsets
                                  out BytesWritten); //Stores the # of Written Bytes
            memory.CloseHandle();
            if (BytesWritten != bValue.Length)
            {
                //Exception!
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ReadInt(int offset, out int read)
        {
            int BytesRead;
            byte[] temp;
            int value;
            read = 0;

            Memory memory = new Memory();
            memory.ReadProcess = this.TargetProcess;

            memory.Open();
            int BaseAddr = Addr.ToDec(Torchlight.BASEADDRESS); //The static address of the pointer (#1)
            int[] Offsets = { OFFSET1, offset }; //Offsets from bottom to top (#2) [starting with bottom of struct]
            temp = memory.PointerRead((IntPtr)BaseAddr, (uint)16, Offsets, out BytesRead);
            value = BitConverter.ToInt32(temp, 0);
            memory.CloseHandle();

            if (BytesRead > 0)
            {
                read = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ApplyStats()
        {
            Dictionary<string, bool> results = new Dictionary<string,bool>();
            //UGH MANUALLY TYPED IT
            //Write Each Stat Value to Memory
            results.Add("Dexterity", WriteValue(this.Dexterity, (int)STAT_OFFSETS.Dexterity));
            results.Add("Strength", WriteValue(this.Strength, (int)STAT_OFFSETS.Strength));
            results.Add("Focus", WriteValue(this.Focus, (int)STAT_OFFSETS.Focus));
            results.Add("Vitality", WriteValue(this.Vitality, (int)STAT_OFFSETS.Vitality));
            results.Add("PlayerLevel", WriteValue(this.PlayerLevel, (int)PLAYER_OFFSETS.PlayerLevel));

            foreach (var item in results)
            {
                if (!item.Value)
                {
                    throw new Exception("Failed to write one or more stats to memory");
                }
            }
            FetchStats();
        }

        public void ApplySkills()
        {
            Dictionary<string, bool> results = new Dictionary<string, bool>();
            results.Add("Skill Points", WriteValue(this.SkillPoints, (int)SKILL_OFFSETS.SkillPoints));

            foreach (var item in results)
            {
                if (!item.Value)
                {
                    throw new Exception("Failed to write one or more stats to memory");
                }
            }
            FetchSkills();
        }


    }
}
