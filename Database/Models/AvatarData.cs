using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Database.Providers;

namespace Database.Models
{
    public class AvatarData : DbContext
    {
        public DbSet<AvatarStat> AvatarStats { get; set; }
        public DbSet<AvatarLook> AvatarLooks { get; set; }
        public DbSet<ExtendSP> ExtendSkillPoints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=taeksi;User=root;Password=;");
        }
        public class Avatar
        {
            AvatarStat stats { get; set; }
            AvatarLook look { get; set; }
        }
        public class AvatarStat
        {
            public Avatar Avatar { get; set; }
            public int AvatarID { get; set; }

            public string sCharacterName { get; set; }
            public byte nGender { get; set; }
            public byte nSkin { get; set; }
            public int nFace { get; set; }
            public int nHair { get; set; }
            public long[] aliPetLockerSN { get; set; }
            public byte nLevel { get; set; }
            public short nJob { get; set; }

            public short nSTR { get; set; }
            public short nDEX { get; set; }
            public short nINT { get; set; }
            public short nLUK { get; set; }
            public int nHP { get; set; }
            public int nMHP { get; set; }
            public int nMP { get; set; }
            public int nMMP { get; set; }

            public short nAP { get; set; }
            public short nSP { get; set; }

            public int nEXP { get; set; }
            public short nPOP { get; set; }
            public int nMoney { get; set; }
            public int nTempEXP { get; set; }

            public List<ExtendSP> extendSP { get; set; }

            public int dwPosMap { get; set; }
            public byte nPortal { get; set; }
            public int nCheckSum { get; set; }            //Not sure where this is used
            public byte nItemCountCheckSum { get; set; }   //Not sure where this is used
            public int nPlaytime { get; set; }
            public short nSubJob { get; set; }
        }

        public class AvatarLook
        {
            public Avatar Avatar { get; set; }
            public int AvatarID { get; set; }

            public byte nGender { get; set; }
            public int nSkin { get; set; }
            public int nFace { get; set; }
            public int nWeaponStickerID { get; set; }
            //public List<CosmeticEquip> anHairEquip { get; set; }
            //public List<>
            public int[] anUnseenEquip { get; set; }
            public int[] anPetID { get; set; }
        }

        public class ExtendSP
        {
            public AvatarStat AvatarStat { get; set; }
            public int AvatarStatID { get; set; }

            public byte nJobLevel { get; set; }
            public byte nSP { get; set; }
        }
    }
}
