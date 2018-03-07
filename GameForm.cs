using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DvTicTacToe
{
    public partial class GameForm : Form
    {
        private Main Owner;
        private bool isServer;
        private bool isMyTurn = false;
        private int[][] board = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };// 0=netral, 1=server, 2=clint
        private SocketManagement con;
        private string[] mapping = { "", "X", "O" };// 0=netral, 1=server, 2=clint
        private bool isFinished = false;
        private bool isWinner = false;

        public GameForm(Main owner, bool isServer, SocketManagement con)
        {
            this.isMyTurn = isServer;
            this.Owner = owner;
            this.isServer = isServer;
            new ResizeForBorderlessForm(this) { AllowResizeAll = false, AllowMove = true };
            InitializeComponent();
            this.con = con;
        }

        private void ReSetBoard()
        {
            p00.Text = mapping[board[0][0]];
            p01.Text = mapping[board[0][1]];
            p02.Text = mapping[board[0][2]];
            p10.Text = mapping[board[1][0]];
            p11.Text = mapping[board[1][1]];
            p12.Text = mapping[board[1][2]];
            p20.Text = mapping[board[2][0]];
            p21.Text = mapping[board[2][1]];
            p22.Text = mapping[board[2][2]];
        }

        private void CheckBoard()
        {
            if (!this.InvokeRequired)
            {
                // V Check
                if (board[0][0] != 0 && board[0][1] != 0 && board[0][1] != 0 && board[0][0] == board[0][1] && board[0][1] == board[0][2] && board[0][0] == board[0][2])
                {
                    // V0
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) isWinner = true;
                }
                else if (board[1][0] != 0 && board[1][1] != 0 && board[1][1] != 0 && board[1][0] == board[1][1] && board[1][1] == board[1][2] && board[1][0] == board[1][2])
                {
                    // V1
                    isFinished = true;
                    if ((isServer && board[1][0] == 1) || (!isServer && board[1][0] == 2)) isWinner = true;
                }
                else if (board[2][0] != 0 && board[2][1] != 0 && board[2][1] != 0 && board[2][0] == board[2][1] && board[2][1] == board[2][2] && board[2][0] == board[2][2])
                {
                    // V2
                    isFinished = true;
                    if ((isServer && board[2][0] == 1) || (!isServer && board[2][0] == 2)) isWinner = true;
                }
                // H Check
                else if (board[0][0] != 0 && board[1][0] != 0 && board[2][0] != 0 && board[0][0] == board[1][0] && board[1][0] == board[2][0] && board[0][0] == board[2][0])
                {
                    // H0
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) isWinner = true;
                }
                else if (board[0][1] != 0 && board[1][1] != 0 && board[2][1] != 0 && board[0][1] == board[1][1] && board[1][1] == board[2][1] && board[0][1] == board[2][1])
                {
                    // H1
                    isFinished = true;
                    if ((isServer && board[0][1] == 1) || (!isServer && board[0][1] == 2)) isWinner = true;
                }
                else if (board[0][2] != 0 && board[1][2] != 0 && board[2][2] != 0 && board[0][2] == board[1][2] && board[1][2] == board[2][2] && board[0][2] == board[2][2])
                {
                    // H2
                    isFinished = true;
                    if ((isServer && board[0][2] == 1) || (!isServer && board[0][2] == 2)) isWinner = true;
                }
                // D Check
                else if (board[0][0] != 0 && board[1][1] != 0 && board[2][2] != 0 && board[0][0] == board[1][1] && board[1][1] == board[2][2] && board[0][0] == board[2][2])
                {
                    // D->
                    isFinished = true;
                    if ((isServer && board[0][0] == 1) || (!isServer && board[0][0] == 2)) isWinner = true;
                }
                else if (board[0][2] != 0 && board[1][1] != 0 && board[2][0] != 0 && board[2][0] == board[1][1] && board[1][1] == board[0][2] && board[2][0] == board[0][2])
                {
                    // D<-
                    isFinished = true;
                    if ((isServer && board[1][1] == 1) || (!isServer && board[1][1] == 2)) isWinner = true;
                }
                if (isFinished)
                {
                    SetEnabled(true);
                    isMyTurn = false;
                    ReSetBoard();
                    WaitingPanel.Hide();
                    if (isWinner) MessageBox.Show(null, "You Win!!", "Result Screen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show(null, "You Lose!!", "Result Screen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(0);
                }
            }
            else this.Invoke((MethodInvoker)delegate { CheckBoard(); });
        }

        private void CheckTurn()
        {
            if (!this.InvokeRequired)
            {
                if (isMyTurn && isFinished == false)
                {
                    WaitingPanel.Hide();
                    SetEnabled(true);
                }
                else
                {
                    WaitingPanel.Show();
                    SetEnabled(false);
                    GetDataFromOthers();
                }
                ReSetBoard();
            }
            else this.Invoke((MethodInvoker)delegate { CheckTurn(); });
        }

        private void SetEnabled(bool value)
        {
            p00.Enabled = value;
            p01.Enabled = value;
            p02.Enabled = value;
            p10.Enabled = value;
            p11.Enabled = value;
            p12.Enabled = value;
            p20.Enabled = value;
            p21.Enabled = value;
            p22.Enabled = value;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            CheckTurn();
        }

        private void GetDataFromOthers()
        {
            Task.Factory.StartNew(() => {
                board = con.getBoard();
                isMyTurn = true;
                CheckBoard();
                CheckTurn();
            });
        }

        private void SetBoardBasedOnButtonName(string code)
        {
            // 0=netral, 1=server, 2=clint
            char[] realCodeInChar = code.Substring(1).ToCharArray();
            board[Int32.Parse("" + realCodeInChar[0])][Int32.Parse("" + realCodeInChar[1])] = isServer ? 1 : 2;
        }

        private void p_click(object sender, EventArgs e)
        {
            if (isMyTurn && isFinished == false)
            {
                if (((Button)sender).Text == "")
                {
                    SetBoardBasedOnButtonName(((Button)sender).Name);
                    con.sendBoard(board);
                    isMyTurn = false;
                    CheckBoard();
                    CheckTurn();
                }
                else MessageBox.Show("Please select another box");
            }
        }

        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
