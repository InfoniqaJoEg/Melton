﻿using Melton.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Melton.Helpers;
using System.CodeDom;

namespace Melton
{

    public partial class BoardUI : Form
    {
        Game game = (Game)Application.OpenForms["game"];
        Eigenschaften eigenschaften;
        Actions action;
        Button btn = new Button();
        Button btndragclass;
        Button btndropclass;
        Boss evilboss;
        Warrior krieger;
        Hunter hunter;
        Mage magier;
        Shaman schamane;
        List<MeltonCreature> playerlist = new List<MeltonCreature>();
        public Form parent { get; set; }
        public Boss EvilBoss
        {
            get { return evilboss; }
            set { evilboss = value; }
        }
        public Warrior Krieger
        {
            get { return krieger; }
            set { krieger = value; }
        }
        public Hunter Jaeger
        {
            get { return hunter; }
            set { hunter = value; }
        }
        public Mage Magier
        {
            get { return magier; }
            set { magier = value; }
        }
        public Shaman Schamane
        {
            get { return schamane; }
            set { schamane = value; }
        }
        public Button Btndragclass
        {
            get { return btndragclass; }
            set { btndragclass = value; }
        }
        public Button Btndropclass
        {
            get { return btndropclass; }
            set { btndropclass = value; }
        }
        public string Data { get; set; }
        public BoardUI(Form mdiParent)
        {
            InitializeComponent();
            flowLayoutPanel1.BackgroundImage = Properties.Resources.background;
            flowLayoutPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            parent = mdiParent;
            Boss EvilBoss1 = new Boss()
            {
                Attack = 40,
                AOE = 20,
                AttackSpeed = 4F,
                Health = 250,
                MaxHealth = 500,
                Name = "Boss"
            };
            EvilBoss = EvilBoss1;
            Warrior Krieger1 = new Warrior()
            {
                Attack = 20,
                DodgeValue = 0.2F,
                Health = 100,
                MaxHealth = 100,
                Name = "player.Krieger"
            };
            Krieger = Krieger1;
            Hunter Jaeger1 = new Hunter()
            {
                Attack = 30,
                AttackSpeed = 0.2F,
                Energy = 100,
                Health = 60,
                MaxHealth = 60,
                Name = "player.Jäger"
            };
            Jaeger = Jaeger1;
            Mage Magier1 = new Mage()
            {
                Attack = 50,
                AttackSpeed = 0.6F,
                Mana = 100,
                Health = 60,
                MaxHealth = 60,
                Name = "player.Magier"
            };
            Magier = Magier1;
            Shaman Schamane1 = new Shaman()
            {
                Attack = 10,
                AttackSpeed = 0.4F,
                HealingValue = 20,
                Mana = 100,
                Health = 80,
                MaxHealth = 80,
                Name = "player.Schamane"
            };
            Schamane = Schamane1;
            EvilBoss.Position = 22;
            Krieger.Position = 56;
            Magier.Position = 57;
            Jaeger.Position = 59;
            Schamane.Position = 60;
            List<MeltonCreature> players = new List<MeltonCreature>();

            players.Add(Krieger1);
            players.Add(Jaeger1);
            players.Add(Magier1);
            players.Add(Schamane1);

            Eigenschaften eigform;
            Actions actform;
            foreach (MeltonCreature creature in players)
            {
                eigform = new Eigenschaften(this, creature);
                actform = new Actions(this, creature);
                eigform.Tag = creature;
                actform.Tag = creature;
                FormManager.GetInstance().AddEig(eigform, creature.Name);
                FormManager.GetInstance().AddAct(actform, creature.Name);

            }
            eigform = new Eigenschaften(this, EvilBoss1);
            actform = new Actions(this, EvilBoss1);
            eigform.Tag = EvilBoss1;
            actform.Tag = EvilBoss1;
            FormManager.GetInstance().AddEig(eigform, EvilBoss1.Name);
            FormManager.GetInstance().AddAct(actform, EvilBoss1.Name);
            ButtonArray();
        }
        private void ButtonArray()
        {
            for (int i = 0; i < 81; i++)
            {
                btn = new Button();

                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.Transparent;
                btn.Size = new Size(80, 80);
                btn.ForeColor = Color.Black;
                btn.AllowDrop= true;
                btn.MouseDown += btn_MouseDown;
                btn.DragEnter += btn_DragEnter;
                btn.DragDrop += btn_DragDrop;
                
                btn.Tag = (int)i;
                flowLayoutPanel1.Controls.Add(btn);
                if ((int)btn.Tag == EvilBoss.Position)
                {
                    btn.BackgroundImage = Properties.Resources.Boss;
                    btn.Name = EvilBoss.Name;
                }
                if ((int)btn.Tag == Krieger.Position)
                {
                    btn.BackgroundImage = Properties.Resources.Warrior;
                    btn.Name = Krieger.Name;
                }
                if ((int)btn.Tag == Magier.Position)
                {
                    btn.BackgroundImage = Properties.Resources.Mage;
                    btn.Name = Magier.Name;
                }
                if ((int)btn.Tag == Jaeger.Position)
                {
                    btn.BackgroundImage = Properties.Resources.Hunter;
                    btn.Name = Jaeger.Name;
                }
                if ((int)btn.Tag == Schamane.Position)
                {
                    btn.BackgroundImage = Properties.Resources.Druid;
                    btn.Name = Schamane.Name;
                }
                btn.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void BoardUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (eigenschaften != null)
                eigenschaften.Close();
            if (action != null)
                action.Close();
            Startmenu Menu = new Startmenu(game);
            Menu.MdiParent = game;
        }
        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btndrag = (Button)sender;
            Btndragclass = btndrag;
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(this, DragDropEffects.Copy);
            }
        }
        private void btn_DragEnter(object sender, DragEventArgs e)
        {
            Button btndrop = (Button)sender;
            Btndropclass = btndrop;
            e.Effect = DragDropEffects.Copy;
        }
        private void btn_DragDrop(object sender, DragEventArgs e)
        {
            Button btnPos = (Button)sender;
            if (btnPos.Name.Contains("player") || btnPos.Name.Contains("Boss"))
            {
                if (eigenschaften != null)
                    eigenschaften.Hide();
                if (action != null)
                    action.Hide();
                eigenschaften = FormManager.GetInstance().GetEig(btnPos.Name);
                action = FormManager.GetInstance().GetAct(btnPos.Name);
                eigenschaften.MdiParent = parent;
                action.MdiParent = parent;
                action.Show();
                eigenschaften.Show();
                eigenschaften.Location = new Point(this.Location.X + 776, this.Location.Y);
                action.Location = new Point(this.Location.X, this.Location.Y + 776);
            }
            else
            {
                if (action != null)
                    action.Hide();
                if (eigenschaften != null)
                    eigenschaften.Hide();
            }
            Btndragclass.Tag = Btndropclass.Tag;
            Btndropclass.BackgroundImage = Btndragclass.BackgroundImage;
            Btndropclass.Name = Btndragclass.Name;
            if(btnPos.Name.Contains("Krieger"))
            {
                Krieger.Position = (int)btnPos.Tag;
            }
            if (btnPos.Name.Contains("Magier"))
            {
                Magier.Position = (int)btnPos.Tag;
            }
            if (btnPos.Name.Contains("Jäger"))
            {
                Jaeger.Position = (int)btnPos.Tag;
            }
            if (btnPos.Name.Contains("Schamane"))
            {
                Schamane.Position = (int)btnPos.Tag;
            }
        }
    }
}
