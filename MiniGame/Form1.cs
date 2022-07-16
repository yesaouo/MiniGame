using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGame
{
    public partial class Form1 : Form
    {
        public int hp1, hp2, pp1, pp2, p2_choose;

        private void button1_Click(object sender, EventArgs e)
        {
            Battle(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Battle(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Battle(3);
        }

        public void Print()
        {
            label1.Text = hp1.ToString();
            label2.Text = pp1.ToString();
            label3.Text = hp2.ToString();
            label4.Text = pp2.ToString();
        }

        public void Scroll()
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        public string Choose(int choose)
        {
            switch (choose)
            {
                case 1: return "攻擊";
                case 2: return "防禦";
                default: return "回魔";
            }
        }

        public async void Battle(int p1_choose)
        {
            Random random = new Random();
            p2_choose = random.Next(1, 4);
            if (p2_choose != 3)
            {
                switch (pp2)
                {
                    case 5:
                        if (random.Next(1, 7) < 3) p2_choose = 3;
                        break;
                    case 4:
                        if (random.Next(1, 7) < 4) p2_choose = 3;
                        break;
                    case 3:
                        if (random.Next(1, 7) < 5) p2_choose = 3;
                        break;
                    case 2:
                        if (random.Next(1, 7) < 6) p2_choose = 3;
                        break;
                    default:
                        if (pp2 == 0 || pp2 == 1) p2_choose = 3;
                        break;
                }
            }
            textBox1.Text += "\r\n你選擇" + Choose(p1_choose) + ",對手選擇" + Choose(p2_choose);
            int dice1 = random.Next(1, 7), dice2 = random.Next(1, 7);
            textBox1.Text += "\r\n擲骰中.";
            Scroll(); await Task.Delay(1000);
            textBox1.Text += "擲骰中..";
            Scroll(); await Task.Delay(1000);
            textBox1.Text += "擲骰中...";
            Scroll(); await Task.Delay(1000);
            textBox1.Text += "\r\n你得到的點數是" + dice1 + "\r\n對手得到的點數是" + dice2;
            Scroll(); await Task.Delay(3000);
            textBox1.Text += "\r\n\r\n";
            int result1 = 0, result2 = 0;
            if (p1_choose == 1 && p2_choose == 1)
            {
                if (dice1 > dice2) result2 = dice1;
                if (dice1 < dice2) result1 = dice2;
                textBox1.Text += "你對對手造成了" + result2 + "點傷害\r\n對手對你造成了" + result1 + "點傷害";
            }
            if (p1_choose == 2 && p2_choose == 2) textBox1.Text += "雙方皆損失了" + dice1 + "點能量";
            if (p1_choose == 1 && p2_choose == 2)
            {
                if (dice1 > dice2) { result2 = dice1 - dice2; textBox1.Text += "你對對手造成了" + result2 + "點傷害"; }
                if (dice1 < dice2) { result1 = dice2 - dice1; textBox1.Text += "對手對你造成了" + result1 + "點傷害，並回復了" + result1 + "點血量"; hp2 += result1; }
                if (dice1 == dice2) textBox1.Text += "雙方皆損失了" + dice1 + "點能量";
            }
            if (p1_choose == 2 && p2_choose == 1)
            {
                if (dice1 < dice2) { result1 = dice2 - dice1; textBox1.Text += "對手對你造成了" + result1 + "點傷害"; }
                if (dice1 > dice2) { result2 = dice1 - dice2; textBox1.Text += "你對對手造成了" + result2 + "點傷害，並回復了" + result2 + "點血量"; hp1 += result2; }
                if (dice1 == dice2) textBox1.Text += "雙方皆損失了" + dice1 + "點能量";
            }
            if (p1_choose == 3 && p2_choose == 3) { textBox1.Text += "你回復了" + dice1 + "點能量\r\n對手回復了" + dice2 + "點能量"; pp1 += dice1 * 2; pp2 += dice2 * 2; }
            if (p1_choose == 2 && p2_choose == 3) { textBox1.Text += "對手回復了" + dice2 + "點能量\r\n你白白損失了" + dice1 + "點能量"; pp2 += dice2 * 2; }
            if (p1_choose == 3 && p2_choose == 2) { textBox1.Text += "你回復了" + dice1 + "點能量\r\n對手白白損失了" + dice2 + "點能量"; pp1 += dice1 * 2; }
            if (p1_choose == 1 && p2_choose == 3) { result2 = dice1; textBox1.Text += "你對對手造成了" + result2 + "點傷害\r\n對手回復了" + dice2 + "點能量"; pp2 += dice2 * 2; }
            if (p1_choose == 3 && p2_choose == 1) { result1 = dice2; textBox1.Text += "你回復了" + dice1 + "點能量\r\n對手對你造成了" + result1 + "點傷害"; pp1 += dice1 * 2; }
            hp2 -= result2; hp1 -= result1; pp1 -= dice1; pp2 -= dice2;
            Scroll(); await Task.Delay(3000);
            Print();
            if (hp1 > 0 && hp2 > 0 && pp1 >= 0 && pp2 >= 0)
            {
                textBox1.Text = "Player1現在有" + hp1 + "點血量，" + pp1 + "點能量\r\nPlayer2現在有" + hp2 + "點血量，" + pp2 + "點能量";
                return;
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            if (hp1 <= 0) textBox1.Text += "\r\n抱歉，你已經死了!";
            else if (pp1 < 0) textBox1.Text += "\r\n死因:精盡人亡";
            else textBox1.Text += "\r\nYou Win!";
            textBox1.Text += "\r\n本遊戲由夜颯製作，感謝您的遊玩";
            Scroll();
            textBox1.Enabled = false;
        }

        public Form1()
        {
            InitializeComponent();
            hp1 = 24;
            hp2 = 24;
            pp1 = 12;
            pp2 = 12;
            Print();
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Text = "Player1現在有" + hp1 + "點血量，" + pp1 + "點能量\r\nPlayer2現在有" + hp2 + "點血量，" + pp2 + "點能量\r\n選擇指令來開始遊戲";
        }
    }
}
